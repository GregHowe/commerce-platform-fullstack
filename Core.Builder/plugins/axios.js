import { cloneDeep as _cloneDeep } from "lodash";
const _ignoreBusyPropTargets = ["/pipeline/queue"];
const _ignoreSerializationUrls = [/media\/upload/gi];
const _serializationPropTargets = [
	"settings",
	"blocks",
	"navigation",
	"footer",
];
function _handleSerializeRequest(data) {
	if (Array.isArray(data)) {
		for (const idx in data) {
			_handleSerializeRequest(data[idx]);
		}
	} else if (typeof data === "object") {
		for (const prop in data) {
			if (_serializationPropTargets.indexOf(prop) >= 0) {
				if (typeof data[prop] === "object") {
					data[prop] = JSON.stringify(data[prop]);
				}
			} else {
				_handleSerializeRequest(data[prop]);
			}
		}
	}
}
function _handleDeserializeResponse(data) {
	if (
		Array.isArray(data) &&
		!_ignoreSerializationUrls.some((rx) => rx.test(data.url))
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

export default function ({ $axios, store, router }) {
	$axios.onRequest((config) => {
		if (
			typeof config.data === "object" &&
			!_ignoreSerializationUrls.some((rx) => rx.test(config.url))
		) {
			const dataClone = _cloneDeep(config.data);
			_handleSerializeRequest(dataClone);
			config.data = dataClone;
		}
		// stops from listing these twice
		if (!store.getters["interface/isLoading"](config.url)) {
			if (_ignoreBusyPropTargets.indexOf(config.url) === -1) {
				store.commit("interface/startLoading", config.url);
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
		store.commit("interface/stopLoading", response.config.url);
		return response;
	});

	$axios.onError((error) => {
		console.error("axios error", error);
		store.commit("interface/stopLoading", error?.response?.config?.url);
		if (
			!error?.response?.status == 401 ||
			(error?.response?.status == 400 &&
				error?.response?.config?.url == "/auth/refresh")
		) {
			console.log("unauthorized");
			store.commit(
				"interface/setAuthError",
				"Your session has expired, please log in again."
			);
			store.commit("interface/stopLoadingAll");
			router.push({ path: "/auth/login" });
		}

		if (!error?.response?.status == 404) {
			store.commit("interface/stopLoadingAll");
		}

		if (!error?.response) {
			router.push({ path: "auth/login" });
		}
	});
}
