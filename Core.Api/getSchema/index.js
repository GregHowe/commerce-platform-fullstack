const axios = require("axios");
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const getSchema = async (context, req) => {
	try {
		const token = getUserToken(req);
		const response = await axios.get(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/graphql/schema`,
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);
		context.res = {
			body: response.data || "n/a",
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = getSchema;
