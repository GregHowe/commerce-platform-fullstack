/* eslint-disable no-console */
import blockSchema from "~/schemas/block.json";
import pageSchema from "~/schemas/page.json";
import siteSchema from "~/schemas/site.json";
import blockHelper from "~/helpers/blocks.js";

import {
	get as _get,
	isEqual as _isEqual,
	cloneDeep as _cloneDeep,
	set as _set,
	extend as _extend,
	has as _has,
	remove as _remove,
} from "lodash";

import Vue from "vue";

const defaultState = {
	siteList: null,
	themeList: null,
	// site state
	selectedSiteId: null,
	storedSite: null,
	workingSite: {
		id: "",
		brandId: "",
		pages: [],
	},
	// page state
	selectedPageId: null,
	workingPage: null, // reference to a page in the array
	// block state
	workingBlock: null, // reference to a block in the array
	highlightedBlockIds: [],
	selectedBlockIds: [], // this is an array of block ids that are breadcrumbs to the selected block (nesting)
	selectedBlockId: null, // this is explicitly the block that is selected
	copiedBlock: null,
};

export const state = () => ({
	...defaultState,
});

export const getters = {
	workingSiteAzureURL: (state) => ({
		live: `https://f92core-${state.workingSite.brandId}-${state.workingSite.id}.azureedge.net`,
		staging: `https://f92core-nylwebsites.azureedge.net/${state.workingSite.brandId}/websites/${state.workingSite.id}/latest`,
	}),
	getWorkingBlockSetting: (state) => (key) => {
		if (!state.workingBlock) {
			return null;
		}
		return _get(state.workingBlock, key);
	},
	getWorkingPageSetting: (state) => (key) => {
		if (!state.workingPage) {
			return null;
		}
		return _get(state.workingPage, key);
	},
	getWorkingSiteSetting: (state) => (key) => {
		return _get(state.workingSite, key);
	},
	hasWorkingChanges: (state) => {
		if (!_isEqual(state.storedSite, state.workingSite)) {
			return true;
		}
		return false;
	},
	navItems: (state) => state.workingSite.navigation?.links || [],
	pageTree: (state) => {
		return (
			state?.workingSite?.navigation?.links
				?.filter((l) => !l.parent)
				.map((l) => {
					return {
						...l,
						subPages: state.workingSite.navigation?.links.filter(
							(sl) => sl?.parent == l?.linkUrl
						),
						target: l.openInNewTab ? "_blank" : "_self",
					};
				}) || []
		);
	},
	siteTree: (state) => {
		const pages = state.workingSite.pages;
		function recurseBlocks(blocks, pageId) {
			if (blocks && blocks != null && blocks.length) {
				try {
					return blocks.map((b) => ({
						id: b.id,
						pageId,
						name: b.type,
						type: "block",
						children: recurseBlocks(b.blocks, pageId),
					}));
				} catch (err) {
					return [];
				}
			}
			return [];
		}
		function recursePages(pgs) {
			return pgs.map((p) => ({
				name: p.title,
				type: "page",
				pageId: p.id,
				children: [
					...pages
						.filter((sp) => sp.parentPageId == p.id)
						.map((sp) => ({
							pageId: sp.id,
							name: sp.title,
							type: "page",
							children: recurseBlocks(sp.blocks, sp.id),
						})),
					...recurseBlocks(p.blocks, p.id),
				],
			}));
		}

		return recursePages(pages.filter((p) => !p.parentPageId));
	},
};

export const actions = {
	async createSite({ dispatch }) {
		await this.$axios.$post("/sites/create");
		return dispatch("loadSiteList");
	},
	async removeSite({ dispatch }, siteId) {
		await this.$axios.$delete(`/sites/${siteId}`);
		return dispatch("loadSiteList");
	},
	async createPage({ state, commit, dispatch }) {
		try {
			await this.$axios.$put(`/sites/${state.selectedSiteId}`, {
				...state.workingSite,
			});
			commit("setStoredSite", state.workingSite);
		} catch (err) {
			return false;
		}
		const response = await this.$axios.$post(
			`/sites/${state.selectedSiteId}/pages/create`
		);
		await dispatch("loadSite");
		commit("setSelectedPageId", response?.id);
	},
	insertBlock({ state, commit, dispatch }, payload) {
		const { settings = {}, position = [0] } = payload;
		_assignBlockDefaults([settings]); // assures that whenever we insert blocks they all have unique identifiers
		const workingBlocks = state.workingPage?.blocks || [];
		const blocksCloned = _cloneDeep(workingBlocks);
		const update = blockHelper.insert(blocksCloned, position);
		_extend(update.newBlock, {
			...settings,
		});
		commit("setWorkingPageBlocks", update.blocks);
		dispatch("selectBlockIds", [update.newBlock.id]);
	},
	removeBlock({ state, commit, dispatch }) {
		const blocksCloned = _cloneDeep(state.workingPage.blocks);
		const blocksUpdated = blockHelper.remove(
			blocksCloned,
			state.workingBlock.id
		);
		_remove(_assignedIds, (item) => item === state.workingBlock.id);
		dispatch("selectBlockIds", []);
		commit("setWorkingPageBlocks", blocksUpdated);
	},
	selectBlockIds({ commit }, payload = []) {
		commit("setSelectedBlockIds", payload);
		commit(
			"interface/setActiveSidebarKey",
			payload.length ? "block" : null,
			{ root: true }
		);
	},
	async removePage({ state, dispatch }, pageId) {
		await this.$axios.$delete(
			`/sites/${state.selectedSiteId}/pages/${pageId}`
		);
		return await dispatch("loadSite");
	},
	async loadSite({ state, commit, dispatch, getters }) {
		// clear selected blocks
		let response = {};
		dispatch("selectBlockIds");
		if (!state.selectedSiteId) {
			commit("setStoredSite", null);
			dispatch("resetWorkingSite");
			return false;
		} else {
			// query api to retrieve site and array of page objects

			response = await this.$axios.$get(`/sites/${state.selectedSiteId}`);

			// apply fetched data to state
			commit("setStoredSite", response);
			dispatch("resetWorkingSite");

			// instantiate default values from the site schema for footer and navigation
			siteSchema.fields.forEach((field) => {
				if (["navigation", "footer"].includes(field.type))
					field.sections.forEach((section) => {
						section.fields.forEach((schemaField) => {
							if (
								!getters.getWorkingSiteSetting(
									`${field.type}.${schemaField.key}`
								) // not already set
							) {
								commit("setWorkingSiteSetting", {
									key: `${field.type}.${schemaField.key}`,
									value: schemaField.default,
								});
							}
						});
					});
			});
			return true;
		}
	},
	async loadSiteList({ commit }) {
		const response = await this.$axios.$get(`/sites`);
		commit("setSiteList", response);
	},
	async loadThemeList({ commit }) {
		// query api to retrieve array of theme objects
		const response = await this.$axios.$get(`/library/themes`);
		commit("setThemeList", null);
	},
	async saveWorkingSite({ commit, dispatch, state }) {
		console.log(state.workingSite);
		console.log(state.selectedSiteId);
		try {
			await this.$axios.$put(`/sites/${state.selectedSiteId}`, {
				...state.workingSite,
			});
			commit("setStoredSite", state.workingSite);
			return await dispatch("loadSite");
		} catch (err) {
			return false;
		}
	},
	async saveAndPublishWorkingSite({ commit, dispatch, state }) {
		commit("sanitizeWorkingSite");
		await this.$axios.$put(`/sites/${state.selectedSiteId}/publish`, {
			...state.workingSite,
		});
		await dispatch("buildQueue/GET_PIPELINE_STATUS");
		commit("buildQueue/setJustPublished", true, { root: true });
		return await dispatch("loadSite");
	},
	resetWorkingSite({ commit }) {
		commit("cloneStoredSite");
		commit("resetWorkingPageAndBlock");
	},
};

export const mutations = {
	setCopiedBlockAsWorkingBlock(state) {
		const clonedCopy = _cloneDeep(state.copiedBlock);
		_assignBlockDefaults([clonedCopy]);
		for (const key in clonedCopy) {
			state.workingBlock[key] = clonedCopy[key];
		}
	},
	extendSite(state, value) {
		// used for jest testing
		state = { ...state, ...value };
	},

	clearBlockSelections(state) {
		state.highlightedBlockIds = [];
		state.selectedBlockId = null;
		state.selectedBlockIds = [];
		state.workingBlock = null;
	},

	clearEverything(state) {
		Object.keys(defaultState).forEach((k) => (state[k] = defaultState[k]));
	},

	cloneStoredSite(state) {
		state.workingSite = state.storedSite
			? _cloneDeep(state.storedSite)
			: null;
	},

	resetWorkingPageAndBlock(state) {
		// if there is an active workingPage or workingBlock, reset them as well
		if (state.selectedPageId) {
			state.workingPage = state.workingSite.pages.find(
				(page) => page.id === state.selectedPageId
			);
		}
		if (state.selectedBlockId) {
			state.workingBlock = blockHelper.find(
				state.workingPage.blocks,
				state.selectedBlockId
			);
		}
	},
	setCopiedBlock(state, value) {
		state.copiedBlock = value;
	},
	setSiteList(state, value) {
		state.siteList = value ? _cloneDeep(value) : null;
	},

	setThemeList(state, value) {
		state.themeList = value ? _cloneDeep(value) : null;
	},

	setSelectedSiteId(state, value = null) {
		state.selectedSiteId = value;
	},

	setStoredSite(state, site) {
		state.storedSite = site ? _cloneDeep(site) : null;
	},

	// careful this looks like it could break page references
	extendWorkingSite(state, siteData) {
		if (state.workingSite) {
			Object.keys(siteData).forEach((key) => {
				// uses $set so new properties will be reactive
				// when extending the working page
				Vue.set(state.workingSite, key, siteData[key]);
			});
			return true;
		}
		return false;
	},

	setWorkingSiteSetting(state, { key, value }) {
		if (state.workingSite) {
			// if the key is a nested array index we need to explicitly $set the array item
			const arrayIndex = key.slice(
				key.indexOf("[") + 1,
				key.lastIndexOf("]")
			);
			if (!isNaN(arrayIndex)) {
				const splitKey = key.split("."); // ["footer", "disclosures[0]"]
				Vue.set(
					state.workingSite[splitKey[0]][splitKey[1].split("[")[0]],
					arrayIndex,
					value
				);
				return true;
			}
			_set(state.workingSite, key, value);
			return true;
		}
		return false;
	},

	setWorkingPageBlocks(state, value) {
		if (state.workingPage) {
			state.workingPage.blocks = value;
			if (state.selectedBlockId) {
				/// added to renew the reference to workingBlock
				state.workingBlock = blockHelper.find(
					state.workingPage.blocks,
					state.selectedBlockId
				);
			}
			return true;
		}
		return false;
	},

	extendWorkingPage(state, pageData) {
		if (state.workingPage) {
			Object.keys(pageData).forEach((key) => {
				// uses $set so new properties will be reactive
				// when extending the working page
				Vue.set(state.workingPage, key, pageData[key]);
			});
			return true;
		}
		return false;
	},

	extendWorkingBlock(state, blockData) {
		if (state.workingBlock) {
			Object.keys(blockData).forEach((key) => {
				// uses $set so new properties will be reactive
				// when extending the working block
				Vue.set(state.workingBlock, key, blockData[key]);
			});
			_assignBlockDefaults([state.workingBlock]); // assures that any blocks within have ids
			return true;
		}
		return false;
	},

	setWorkingPageSetting(state, { key, value }) {
		if (state.workingPage) {
			_set(state.workingPage, key, value);
			return true;
		}
		return false;
	},

	setWorkingBlockSetting(state, { key, value }) {
		if (state.workingBlock) {
			_set(state.workingBlock, key, value);
			return true;
		}
		return false;
	},

	setSelectedPageId(state, value = null) {
		state.workingPage = state.workingSite.pages.find(
			(page) => page.id === value
		);
		state.selectedPageId = state.workingPage ? value : null;
	},

	setHighlightedBlockIds(state, params = { blockId: null, target: null }) {
		let path = [];
		if (params.target?.path) {
			params.target.path.forEach((el) => {
				// oh this is gnarly
				if (el.classList && el.classList.contains("block")) {
					path = [el.id, ...path];
				}
			});
		}
		state.highlightedBlockIds = path;
	},
	clearSelectedBlockIds(state) {
		state.selectedBlockId = null;
		state.workingBlock = null;
	},
	setNavigationItems(state, navigationItems) {
		let site = { ...state.workingSite };
		site.navigation.links = navigationItems;
		state.workingSite = site;
	},
	setSelectedBlockIds(state, blockIds = []) {
		state.selectedBlockIds = blockIds;
		if (blockIds.length) {
			state.selectedBlockId = blockIds[blockIds.length - 1];
			state.workingBlock = blockHelper.find(
				state.workingPage.blocks,
				state.selectedBlockId
			);
		} else {
			state.selectedBlockId = null;
			state.workingBlock = null;
		}
	},
	setSelectedBlockId(state, blockId) {
		state.selectedBlockId = blockId;
		state.selectedBlockIds = [blockId];
		state.workingBlock = blockHelper.find(
			state.workingPage.blocks,
			state.selectedBlockId
		);
	},
	// ===
	// cloudcms has this annoying habit of sending null values for empty but required string properties
	// and then throwing a 500 error when you send it back w/ a null value because it is not a string
	// so these sanitize functions are to replace string field null values with an empty string
	sanitizeWorkingSite(state) {
		return true;
		// siteSchema.fields.forEach((fieldSchema) => {
		// 	if (fieldSchema.type === "string") {
		// 		if (!_get(state.workingSite, fieldSchema.key, null)) {
		// 			_set(state.workingSite, fieldSchema.key, "");
		// 		}
		// 	}
		// });
		// state.workingSite.pages.forEach((workingPage) => {
		// 	pageSchema.fields.forEach((fieldSchema) => {
		// 		if (fieldSchema.type === "string") {
		// 			if (!_get(workingPage, fieldSchema.key, null)) {
		// 				_set(workingPage, fieldSchema.key, "");
		// 			}
		// 		}
		// 	});
		// });
	},
};

// cloudcms currently is storing page blocks as JSON
// so each block is not assured to have an id assigned
// this fn makes sure each block has a unique id for the frontend
// there are a few places this needs to run
// a) when a block is inserted
// b) when a block is extended (since it might have blocks inside without ids)
// c) any others?

const _assignedIds = [];
const _assignBlockDefaults = (blocks) => {
	if (blocks.length) {
		blocks.forEach((block) => {
			let isUniqueId = block.id
				? _assignedIds.indexOf(block.id) === -1
				: false;
			if (!isUniqueId) {
				do {
					block.id = blockHelper.generateId();
					isUniqueId = _assignedIds.indexOf(block.id) === -1;
				} while (!isUniqueId);
			}
			_assignedIds.push(block.id);
			// find block schema based on type
			const blockType = block?.type || null;
			if (blockType) {
				const blockTypeSchema = blockSchema.types[blockType];
				const blockTypeFields = blockTypeSchema?.fields || [];
				// fill in default values for any fields that are missing and have defaults
				if (blockTypeFields) {
					blockTypeFields.forEach((field) => {
						if (field.default && !_has(block, [field.key])) {
							_set(block, [field.key], field.default);
						}
					});
				}
				// if this block requires a child but is missing one
				// this will force an empty block inside
				if (blockTypeSchema?.requireChild && !_has(block, "blocks")) {
					_set(block, "blocks", [blockHelper.getBlank()]);
				}
				if (!block?.variants) {
					block.variants = {};
				}
			}
			if (block.blocks) {
				// the block has its own blocks!
				_assignBlockDefaults(block.blocks);
			}
		});
		return blocks;
	}
};
