export const state = () => ({
	site: null,
});

export const getters = {
	getThemeByKey: (state) => (themeKey) => {
		if (state.site.themes) {
			return state.site.themes[themeKey];
		}
		return null;
	},
	getPageByHandle: (state) => (pageHandle) => {
		return state.site?.pages?.find((page) => {
			if (page.parentPageId) {
				let parentPage = state.site.pages.find(
					(p) => page.parentPageId === p.id
				);
				if (parentPage?.handle) {
					return (
						`${parentPage?.handle}/${page?.handle}` === pageHandle
					);
				} else {
					return page?.handle === pageHandle;
				}
			} else {
				return page?.handle === pageHandle;
			}
		});
	},
	getHomePage: (state) => {
		return state.site.pages.find((page) => {
			return page.id === state.site.homepageId;
		});
	},
	pageMap: (state) => state?.site?.pages.map((p) => p.handle) || [],
};

export const actions = {
	async nuxtServerInit({ commit }, ctx) {
		commit("setSite", ctx.$config.siteData);
	},
};

export const mutations = {
	setSite(state, siteData) {
		state.site = siteData;
	},
};
