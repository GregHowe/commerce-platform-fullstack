import { cloneDeep as _cloneDeep } from "lodash";
const _ignoreSerializationUrls = ["/library/media/upload"];
const _serializationPropTargets = [
	"settings",
	"blocks",
	"navigation",
	"footer",
];
const _ignoreBusyPropTargets = ["/pipeline/queue"];
function _handleSerializeRequest(data) {
	if (Array.isArray(data)) {
		for (const idx in data) {
			_handleSerializeRequest(data[idx]);
		}
	} else if (typeof data === "object") {
		for (const prop in data) {
			if (_serializationPropTargets.indexOf(prop) >= 0) {
				data[prop] = JSON.stringify(data[prop]);
			} else {
				_handleSerializeRequest(data[prop]);
			}
		}
	}
}
function _handleDeserializeResponse(data) {
	if (
		Array.isArray(data) &&
		_ignoreSerializationUrls.indexOf(data.url) === -1
	) {
		for (const idx in data) {
			_handleDeserializeResponse(data[idx]);
		}
	} else if (typeof data === "object") {
		for (const prop in data) {
			if (_serializationPropTargets.indexOf(prop) >= 0) {
				data[prop] = JSON.parse(data[prop]);
			} else {
				_handleDeserializeResponse(data[prop]);
			}
		}
	}
}

export default function ({ $axios, app }) {
	$axios.defaults.params = {
		code: process.env.API_CODE,
	};
	$axios.onRequest((config) => {
		if (
			typeof config.data === "object" &&
			_ignoreSerializationUrls.indexOf(config.url) === -1
		) {
			const dataClone = _cloneDeep(config.data);
			_handleSerializeRequest(dataClone);
			config.data = dataClone;
		}
		// stops from listing these twice
		if (!app.store.getters["interface/isLoading"](config.url)) {
			if (_ignoreBusyPropTargets.indexOf(config.url) === -1) {
				app.store.commit("interface/startLoading", config.url);
			}
		} else {
			//throw `Already loading ${config.url}`;
		}
		return config;
	});

	$axios.onResponse((response) => {
		if (typeof response.data === "object") {
			const dataClone = _cloneDeep(response.data);
			_handleDeserializeResponse(dataClone);
			response.data = dataClone;
		}
		app.store.commit("interface/stopLoading", response.config.url);
		return response;
	});

	$axios.onError((error) => {
		/*
	 	const code = parseInt(error.response && error.response.status);
	 	if (code === 400) {
	 		redirect("/400");
	 	}
		*/
		if (!error?.response?.status == 401) {
			console.log("unauthorized");
		}
		if (!error?.response?.status == 404) {
			app.store.commit("interface/stopLoadingAll");
		}
	});
}
