const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const deletePage = async (context, req) => {
	// first of all we should verify that the page is actually on the site
	// also need to verify user is allowed to do this
	// probably also not really delete anything but mark things as having been deleted

	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}

	const pageId = req.params.pageId;
	if (!pageId) {
		return {
			status: "Page not found!",
			error: new Error(),
		};
	}

	try {
		const token = getUserToken(req);

		const site = await axios.get(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteId}`,
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		const startingPageCount = site.data.pages.length;
		site.data.pages.forEach((page, pageIdx) => {
			if (page.id === pageId) {
				site.data.pages.splice(pageIdx, 1);
				return true;
			}
		});
		if (startingPageCount !== site.data.pages.length) {
			// remvoe the page from the site record's "pages" field, an array of page ids
			await axios.patch(
				`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteId}`,
				[
					{
						op: "replace",
						path: "/pages",
						value: [...site.data.pages],
					},
				],
				{
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			);

			// delete the page itself
			const pageResponse = await axios.delete(
				`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${pageId}`,
				{
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			);

			if (!pageResponse.data) {
				throw Error("Failed to delete page!");
			}
		}

		context.res = {
			body: {
				success: true,
			},
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = deletePage;
