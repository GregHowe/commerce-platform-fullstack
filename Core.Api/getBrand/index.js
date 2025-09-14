const axios = require("axios");
const { loadFile } = require("graphql-import-files");
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getAppToken = require("../lib/getAppToken.js");

const getBrand = async (context, req) => {
	const brandId = req.params.brandId;
	if (!brandId) {
		return {
			status: "Brand not found!",
			error: new Error(),
		};
	}
	try {
		const token = await getAppToken();
		const query = loadFile("./getBrand/query.gql");
		const response = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/graphql`,
			{
				query,
				variables: {
					brandId,
				},
			},
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);

		const brands = response.data.data.core_brands || [];
		const brand = brands[0];
		context.res = {
			body: {
				data: {
					...brand,
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

module.exports = getBrand;
