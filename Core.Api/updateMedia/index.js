const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const personalAccessToken = process.env.MS_PAT;
const getUserToken = require("../lib/getUserToken.js");

const updateMedia = async (context, req) => {
	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}
	const mediaId = req.params.mediaId;
	if (!mediaId) {
		return {
			status: "Media not found!",
			error: new Error(),
		};
	}
	try {
		const payload = req.body;

		const mediaData = {
			...payload.media,
		};

		// could not find a way to update just the title for example,
		// and it was wiping out pages array so site had no pages
		// todo: only update specific fields if possible
		// todo: update pages as well
		// todo: would like to post diffs and go through to patch the records

		const token = getUserToken(req);

		payload.pages.forEach((pageData) => {
			// this maps associated pages to site.pages field using node refs
			siteData.pages.push({
				ref: `node://${projectId}/${repositoryId}/${branchId}/${pageData._doc}`,
			});
		});

		const siteResponse = await axios.put(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteId}`,
			siteData,
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		// save each page
		payload.pages.forEach(async (pageData) => {
			let blocksAsString = "";
			if (pageData.blocks && pageData.blocks.length) {
				blocksAsString = JSON.stringify(pageData.blocks);
			}
			await axios.put(
				`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${pageData._doc}`,
				{
					...pageData,
					blocks: blocksAsString,
				},
				{
					headers: {
						Authorization: `Bearer ${token}`,
					},
				}
			);
		});

		const timestamp = Date.now();
		const auth = {
			username: "New-York-Life",
			password: personalAccessToken,
		};
		const params = {
			CLOUDCMS_BRANDID: siteData.brand_id,
			CLOUDCMS_SITEID: siteData._doc,
			CLOUDCMS_PROJECTID: projectId,
			CLOUDCMS_REPOSITORYID: repositoryId,
			CLOUDCMS_BRANCHID: branchId,
			TIMESTAMP: `Build-${timestamp}`,
		};

		const pipelineResponse = await axios.post(
			`https://dev.azure.com/New-York-Life/NYL%20Core/_apis/pipelines/26/runs?api-version=6.0-preview.1`,
			{
				templateParameters: params,
			},
			{ auth }
		);

		context.res = {
			body: pipelineResponse.data,
		};
	} catch (err) {
		console.error(err);
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = updateMedia;
