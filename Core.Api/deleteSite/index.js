const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const deleteSite = async (context, req) => {
	// need to verify user is allowed to do this
	// probably also not really delete sites but mark them as having been deleted

	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}

	try {
		const token = getUserToken(req);

		// attach the new page to the site record's "pages" field, an array of page ids
		const siteResponse = await axios.patch(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${siteId}`,
			[
				{
					op: "replace",
					path: "/isDeleted",
					value: true,
				},
			],
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		if (!siteResponse.data) {
			throw Error("Failed to delete site!");
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

module.exports = deleteSite;
