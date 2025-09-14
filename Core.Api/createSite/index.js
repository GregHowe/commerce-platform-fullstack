const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");
const generateSlug = require("../lib/generateSlug.js");

const createSite = async (context, req) => {
	try {
		const token = getUserToken(req);

		const payload = req.body;
		const userId = payload.userId || null;
		if (!userId) {
			throw Error("Must include userId!");
		}

		// create page first
		const pageResponse = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes`,
			{
				_type: "frontend:page",
				slug: generateSlug(), // this needs to go
				title: "Untitled Page",
				theme: JSON.stringify({
					color: "",
					font: "",
				}),
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
		const newPagesList = [
			{
				ref: `node://${projectId}/${repositoryId}/${branchId}/${pageId}`,
			},
		];

		const siteResponse = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes`,
			{
				_type: "frontend:site",
				title: "Untitled Site",
				user_id: userId,
				pages: newPagesList,
				brand_id: "nyl",
				theme: "",
				isHome: pageId,
			},
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		if (!siteResponse.data) {
			throw Error("Create site failed!");
		}
		console.debug("site created", siteResponse.data);
		context.res = {
			body: siteResponse.data._doc,
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = createSite;
