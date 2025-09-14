import Vue from "vue";
export const state = () => ({
	id: null,
	handle: null,
	host: null,
	title: null,
	description: null,
	settings: null,
});

export const getters = {};

export const actions = {
	async loadBrand({ commit }, id) {
		try {
			commit("setBrandId", id);
			const response = await this.$axios.$get(`/brands/${id}`);
			commit("setBrand", response);
		} catch (err) {
			console.log(Object.keys(err));
		}
		return;
	},
};

export const mutations = {
	setBrand(state, brand) {
		state.handle = brand ? brand.handle : null;
		state.host = brand ? brand.host : null;
		state.title = brand ? brand.title : null;
		state.description = brand ? brand.description : null;
		Vue.set(state, "settings", brand ? brand.settings : null);
	},
	setBrandId(state, id) {
		state.id = id;
	},
};
