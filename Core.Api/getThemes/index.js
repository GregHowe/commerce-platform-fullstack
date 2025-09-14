const axios = require("axios");
const { loadFile } = require("graphql-import-files");
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const getThemes = async (context, req) => {
	try {
		const token = getUserToken(req);
		const query = loadFile("./getThemes/query.gql");
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

		const themes = response.data.data.preset_themes || [];

		context.res = {
			body: {
				data: {
					themes,
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

module.exports = getThemes;
