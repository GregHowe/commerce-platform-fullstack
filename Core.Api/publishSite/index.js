const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const personalAccessToken = process.env.MS_PAT;
const getUserToken = require("../lib/getUserToken.js");
const sendMessages = require("../lib/servicebus/servicebusclient");

const publishSite = async (context, req) => {
	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}
	const token = getUserToken(req);
	const payload = req.body;
	const siteData = { ...payload.site };
	const pageData = siteData?.pages ? [...siteData.pages] : [];

	await sendMessages("core", [
		{
			contentType: "application/json",
			subject: "Compliance-InReview",
			body: {
				EventType: "compliance",
				BrandCode: siteData.brand_id,
				ReviewerId: "1234",
				SiteId: siteData._doc,
				Status: "IN REVIEW",
				Notes: `Your website ${siteData._doc} is being reviewed.`,
			},
		},
	]);

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
	try {
		const timestamp = `Build-${Date.now()}`;
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
			TIMESTAMP: timestamp,
		};
		await axios.post(
			`https://dev.azure.com/New-York-Life/NYL%20Core/_apis/pipelines/26/runs?api-version=6.0-preview.1`,
			{
				templateParameters: params,
			},
			{ auth }
		);
		console.log("sending service bus message");

		await sendMessages("core", [
			{
				contentType: "application/json",
				subject: "Compliance-SiteChanges",
				body: {
					EventType: "compliance",
					BrandCode: siteData.brand_id,
					ReviewerId: "1234",
					SiteId: siteData._doc,
					Status: "APPROVED",
					Notes: "Site looks great!",
				},
			},
		]);
		console.log("message sent");

		context.res = {
			body: {
				message: "Success",
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

module.exports = publishSite;
