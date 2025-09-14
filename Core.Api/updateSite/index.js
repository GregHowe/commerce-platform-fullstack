const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const personalAccessToken = process.env.MS_PAT;
const getUserToken = require("../lib/getUserToken.js");

const updateSite = async (context, req) => {
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
		const siteData = { ...payload.site };
		const pageData = siteData?.pages ? [...siteData.pages] : [];
		console.log("pageData", pageData);

		const pagesResponse = await Promise.all(
			pageData.map(async (page) => {
				page.parentPageId = page?.parentPageId || "";
				const blocksAsString = page?.blocks
					? JSON.stringify(page.blocks)
					: "[]";
				page.blocks = blocksAsString;
				return await axios
					.put(
						`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${page._doc}`,
						page,
						{
							headers: {
								Authorization: `Bearer ${token}`,
							},
						}
					)
					.then((result) => {
						console.log("page save result", page._doc);
					});
			})
		).then((results) => {
			console.log("completely all page saves");
			return results;
		});

		// could not find a way to update just the title for example,
		// and it was wiping out pages array so site had no pages
		// todo: only update specific fields if possible
		// todo: update pages as well
		// todo: would like to post diffs and go through to patch the records

		// convert pages to refs for cloudcms
		siteData.pages = [];
		pageData.forEach((pageData) => {
			siteData.pages.push({
				ref: `node://${projectId}/${repositoryId}/${branchId}/${pageData._doc}`,
			});
		});
		siteData.isDeleted = siteData?.isDeleted || false;
		siteData.seo = {
			title: siteData?.seo?.title || "",
			isHidden: siteData?.seo?.isHidden || false,
			description: siteData?.seo?.description || "",
		};
		siteData.pages = siteData?.pages || [];
		console.debug(
			"Theme info",
			siteData.theme,
			JSON.stringify(siteData.theme)
		);
		siteData.theme = JSON.stringify(siteData.theme);
		siteData.navigation = JSON.stringify(siteData?.navigation || []);
		siteData.footer = JSON.stringify(siteData?.footer || []);

		const siteResponse = await axios
			.put(
				`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteData._doc}`,
				siteData,
				{
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			)
			.then((result) => {
				console.log("completely all site saves", siteData._doc);
				return result;
			});
		context.res = {
			body: {
				message: "Success",
				siteResponse: Boolean(siteResponse),
				pagesResponse: Boolean(pagesResponse),
			},
		};
	} catch (err) {
		context.log(err);
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = updateSite;
