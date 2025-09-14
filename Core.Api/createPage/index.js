const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");
const generateSlug = require("../lib/generateSlug.js");

const createPage = async (context, req) => {
	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}

	try {
		const token = getUserToken(req);

		const payload = req.body;

		const pageResponse = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes`,
			{
				_type: "frontend:page",
				slug: generateSlug(), // this needs to go
				title: "Untitled Page",
				blocks: payload?.blocks ? JSON.stringify(payload.blocks) : "[]",
				titleNav: "",
			},
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		if (!pageResponse.data) {
			throw Error("Page create failed!");
		}

		const pageId = pageResponse.data._doc;
		const site = await axios.get(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteId}`,
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		const newPagesOperation = !site.data.pages ? "add" : "replace";
		const newPagesList = !site.data.pages ? [] : [...site.data.pages];
		newPagesList.push({
			ref: `node://${projectId}/${repositoryId}/${branchId}/${pageId}`,
		});

		// attach the new page to the site record's "pages" field, an array of page ids
		await axios.patch(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteId}`,
			[
				{
					op: newPagesOperation,
					path: "/pages",
					value: newPagesList,
				},
			],
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		// pull down page data to send back
		const pageData = await axios.get(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${pageId}`,
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		context.res = {
			body: {
				page: pageData.data,
			},
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = createPage;
