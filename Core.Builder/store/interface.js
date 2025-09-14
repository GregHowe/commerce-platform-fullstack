const defaultNotification = {
	type: "",
	title: "",
	description: "",
	icon: "",
	color: "",
};

export const state = () => ({
	activeSidebarKey: null,
	isSettingsMaximized: false,
	busy: {
		loading: [],
		saving: [],
	},
	notification: defaultNotification,
	authError: "",
});

export const getters = {
	isBusy: (state) => (key) => {
		return (
			state.busy.loading.indexOf(key) >= 0 ||
			state.busy.saving.indexOf(key) >= 0
		);
	},
	isBusyAnything: (state) => {
		return state.busy.loading.length > 0 || state.busy.saving.length > 0;
	},
	isLoading: (state) => (key) => {
		return state.busy.loading.indexOf(key) >= 0;
	},
	isLoadingAnything: (state) => {
		return state.busy.loading.length > 0;
	},
	isSaving: (state) => (key) => {
		return state.busy.saving.indexOf(key) >= 0;
	},
	isSavingAnything: (state) => {
		return state.busy.saving.length > 0;
	},
};

export const actions = {
	SET_NOTIFICATION: ({ state, commit }, notification) => {
		let valid = true;
		Object.keys(state.notification).forEach((k) =>
			!notification[k] ? (valid = false) : ""
		);
		valid ? commit("setNotification", notification) : "";
		setTimeout(() => commit("setNotification", defaultNotification), 5000);
	},
};

export const mutations = {
	setActiveSidebarKey(state, key = null) {
		state.activeSidebarKey = key;
		//state.isSettingsMaximized = false;
	},
	toggleActiveSidebarKey(state, key = null) {
		if (state.activeSidebarKey === key) {
			//state.activeSidebarKey = null;
		} else {
			state.activeSidebarKey = key;
		}
		//state.isSettingsMaximized = false;
	},
	startLoading(state, key) {
		if (state.busy.loading.indexOf(key) === -1) {
			state.busy.loading.push(key);
		}
	},
	stopLoading(state, key) {
		const index = state.busy.loading.indexOf(key);
		if (index >= 0) {
			state.busy.loading.splice(index, 1);
		}
	},
	stopLoadingAll(state) {
		state.busy.loading = [];
	},
	startSaving(state, key) {
		if (state.busy.saving.indexOf(key) === -1) {
			state.busy.saving.push(key);
		}
	},
	stopSaving(state, key) {
		const index = state.busy.saving.indexOf(key);
		if (index >= 0) {
			state.busy.saving.slice(index, 1);
		}
	},
	toggleMaximizeSettings(state) {
		state.isSettingsMaximized = !state.isSettingsMaximized;
	},
	setMaximizeSettings(state, val) {
		state.isSettingsMaximized = !!val;
	},
	setNotification(state, notification) {
		state.notification = notification;
	},
	setAuthError(state, err) {
		state.authError = err || "";
	},
};
