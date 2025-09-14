/* eslint-disable no-console */
/*

router middleware watches the siteId params in the url
and if the new value does not match the last value
then the site is either loaded or cleared

*/

export default async function ({ store, route }) {
	try {
		if (!store.state.brand.handle) {
			await store.dispatch("brand/loadBrand", 3); // this should be determined by passing the domain the app is loaded on
		}
		if (!store.state.auth.user) {
			store.commit("site/clearEverything");
			return;
		}
		// handle data that must be loaded everywhere
		const globalPreloads = [];
		if (store.state.site.siteList === null) {
			globalPreloads.push(store.dispatch("site/loadSiteList"));
		}
		if (store.state.site.themeList === null) {
			globalPreloads.push(store.dispatch("site/loadThemeList"));
		}
		await Promise.all(globalPreloads);
		// handle route based data requirements
		const isLibrary = route.path.indexOf("library") >= 0;
		if (isLibrary) {
			if (!store.state.library.content) {
				globalPreloads.push(store.dispatch("library/getContent"));
			}
			if (!store.state.library.categories) {
				globalPreloads.push(store.dispatch("library/getCategories"));
			}
		}
		const lastSiteId = store.state.site.selectedSiteId;
		const nextSiteId = parseInt(route.params.siteId, 10) || null; // remove parseInt if ids stop being ints
		const lastPageId = store.state.site.selectedPageId;
		const nextPageId = parseInt(route.params.pageId, 10) || null; // remove parseInt if ids stop being ints
		if (
			nextSiteId !== lastSiteId ||
			(!store.state.site.storedSite && nextSiteId !== null)
		) {
			store.commit("site/setSelectedSiteId", nextSiteId);
			if (nextSiteId) {
				await store.dispatch("site/loadSite");
			}
		}
		if (nextSiteId && lastPageId !== nextPageId) {
			store.commit("site/setSelectedPageId", nextPageId); // set this after the site has been loaded
		}
	} catch (err) {
		console.log("Could not phone home.", err);
	}
}
