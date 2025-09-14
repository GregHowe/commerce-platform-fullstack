import Vue from "vue";
import { kebabCase as _kebabCase } from "lodash";
export const state = () => ({
	content: null,
	filters: [],
	categories: null,
});
export const getters = {
	getContentItem: (state) => (id) => {
		return state.content
			? state.content.find((content) => content.id === id)
			: null;
	},
};
export const actions = {
	async getCategories({ commit }) {
		const response = await this.$axios.$get("/library/categories");
		commit("setCategories", response);
	},

	async createCategory({ commit }, settings) {
		const { handle, type, title, description, clientCode, isPrivate } =
			settings;
		const response = await this.$axios.$post("/library/categories/create", {
			handle,
			type,
			title,
			description,
			clientCode,
			isPrivate,
			settings,
		});
		if (!response?.id) {
			return null;
		}
		commit("mergeCategory", response);
		return response;
	},

	async updateCategory({ commit }, category) {
		const response = await this.$axios.$put(
			`/library/categories/${category.id}`,
			category
		);
		if (response) {
			commit("mergeCategory", response);
		}
	},

	async getContent({ commit }) {
		const response = await this.$axios.$get("/library/presets");
		commit("setContent", response);
	},

	async getContentByType({ commit }, type) {
		const response = await this.$axios.$get(
			`/library/presets/type/${type}`
		);
		commit("setContent", response);
	},

	async createContent({ commit }, content) {
		try {
			const {
				type,
				handle,
				title,
				description,
				clientCode,
				category,
				...settings
			} = content;
			const response = await this.$axios.$post(
				"/library/presets/create",
				{
					type,
					handle: handle || _kebabCase(title || "untitled"),
					title: title || "Untitled",
					description: description || "",
					clientCode: clientCode || "",
					category: category || null,
					settings,
					isPrivate: false,
					isDeleted: false,
				}
			);
			if (response.id) {
				commit("addContent", response);
			}
			return response;
		} catch (err) {
			return err;
		}
	},

	async updateContent({ commit }, content) {
		try {
			const response = await this.$axios.$put(
				`/library/presets/${content.id}`,
				content
			);
			if (response.id) {
				commit("updateContentItem", response);
			}
			return response;
		} catch (err) {
			return err;
		}
	},

	async deleteContent({ dispatch, commit }, id) {
		await this.$axios.$delete(`/library/presets/${id}`, {});
		commit("deleteContentItem", id);
		dispatch("getContent");
	},
	addFilter({ commit }, filter) {
		const isSelected = true;
		commit("selectCategory", { filter, isSelected });

		commit("addFilter", filter);
	},
	deleteFilter({ commit }, filter) {
		const isSelected = false;
		commit("selectCategory", { filter, isSelected });

		commit("deleteFilter", filter);
	},
	deleteAllFilters({ state, commit }) {
		const isSelected = false;
		state.filters.forEach((filter) => {
			commit("selectCategory", { filter, isSelected });
		});
		commit("deleteAllFilters");
	},
};

export const mutations = {
	setContent(state, content) {
		state.content = content;
	},
	addContent(state, content) {
		state.content.push(content);
	},
	updateContentItem(state, content) {
		const itemIdx = state.content.findIndex((item) => {
			return item.id === content.id;
		});
		state.content[itemIdx] = content;
	},
	deleteContentItem(state, id) {
		const contentItems = state.content;
		const itemIdx = contentItems.findIndex((item) => {
			return item.id === id;
		});
		state.content = contentItems
			.slice(0, itemIdx)
			.concat(contentItems.slice(itemIdx + 1, contentItems.length));
	},
	deleteFilter(state, filter) {
		const filters = state.filters;
		const filterIdx = filters.findIndex((item) => {
			return item.id === filter.id;
		});
		state.filters = filters
			.slice(0, filterIdx)
			.concat(filters.slice(filterIdx + 1, filters.length));
	},
	deleteAllFilters(state) {
		state.filters = [];
	},
	addFilter(state, filter) {
		state.filters.push(filter);
	},
	setCategories(state, categories) {
		Vue.set(state, "categories", categories);
	},
	mergeCategory(state, category) {
		const idx = state.categories.findIndex((c) => c.id === category.id);
		if (idx >= 0) {
			state.categories[idx] = category;
		} else {
			state.categories.push(category);
		}
	},
	selectCategory(state, { filter, isSelected }) {
		// this marks a category's "selected" property when chosen as a filter
		const category = state.categories.find(
			(category) =>
				category.value === filter.value && category.id === filter.id
		);
		category ? (category.selected = isSelected) : null;
	},
};
