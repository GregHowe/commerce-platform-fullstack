const axios = require("axios");
const baseURL =
	process.env.NUXT_ENV_API_BASE_URL || "https://localhost:7283/api";

const getSite = async (appToken) => {
	const siteId = process.env.generatorSiteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}
	try {
		const response = await axios.get(`${baseURL}/sites/${siteId}`, {
			headers: {
				Authorization: `Bearer ${appToken}`,
			},
		});
		const site = response.data;
		if (site) {
			if (site.themes.color) {
				const themeColor = await axios.get(
					`${baseURL}/library/themes/${site.themes.color}`,
					{
						headers: {
							Authorization: `Bearer ${appToken}`,
						},
					}
				);
				site.themes.color =
					themeColor.data?.style || "/* no color theme selected */";
			}

			if (site.themes.font) {
				const themeFont = await axios.get(
					`${baseURL}/library/themes/${site.themes.font}`,
					{
						headers: {
							Authorization: `Bearer ${appToken}`,
						},
					}
				);
				site.themes.font =
					themeFont.data?.style || "/* no font theme selected */";
			}
			site.navigation = site.navigation
				? JSON.parse(site.navigation)
				: null;
			site.footer = site.footer ? JSON.parse(site.footer) : null;

			// We don't want to couple navigation with pages,
			// so stripping this out in favor of what gets
			// put in site.navigation.links

			site.pages.forEach((p) => (p.parentPageId = null));
		}
		return site;
	} catch (err) {
		console.log(err);
		throw new Error("Couldnt load site");
	}
};

module.exports = getSite;
