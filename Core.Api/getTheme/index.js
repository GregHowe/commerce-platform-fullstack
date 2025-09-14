const axios = require("axios");
const { loadFile } = require("graphql-import-files");
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const getSite = async (context, req) => {
	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}
	try {
		const token = getUserToken(req);
		const query = loadFile("./getSite/query.gql");
		const response = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/graphql`,
			{
				query,
				variables: {
					siteId,
				},
			},
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		const sites = response.data.data.frontend_sites || [];
		const site = sites[0];
		if (!site.pages) {
			site.pages = [];
		} else if (site.pages.length) {
			site.pages.forEach((page) => {
				if (page.blocks !== null) {
					page.blocks = JSON.parse(page.blocks);
				}
			});
		}
		context.res = {
			body: {
				data: {
					site,
				},
			},
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = getSite;
