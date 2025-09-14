const axios = require("axios");
const { loadFile } = require("graphql-import-files");
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const getSites = async (context, req) => {
	const payload = req.body;
	const userId = payload.userId || null;
	if (!userId) {
		throw Error("Must include userId!");
	}
	try {
		const token = await getUserToken(req);
		if (!token) {
			return {
				status: "Unauthorized!",
				error: new Error(),
			};
		}
		const query = loadFile("./getSites/query.gql");
		const response = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/graphql`,
			{
				query,
			},
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);
		const sites =
			response.data.data.frontend_sites.filter((site) => {
				if (site.isDeleted === true) {
					return false;
				}
				if (site.user_id !== userId) {
					return false;
				}
				return true;
			}) || [];
		context.res = {
			body: {
				data: {
					sites,
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

module.exports = getSites;
