export const state = () => ({
	object: null,
	language: null,
	key: null,
	isActive: false,
});

export const getters = {
	getValue(state) {
		return state.object[state.key];
	},
};

export const actions = {};

export const mutations = {
	setObject(state, newValue) {
		state.object = newValue || null;
	},
	setLanguage(state, newValue) {
		state.language = newValue || null;
	},
	setKey(state, newValue) {
		state.key = newValue || null;
	},
	setActive(state, newValue) {
		state.isActive = !!newValue;
	},
	toggle(state, { object, key, language }) {
		state.isActive = !state.isActive;
	},
};
