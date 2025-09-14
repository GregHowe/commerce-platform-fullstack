const axios = require("axios");
const testLeadForm = async (context, req) => {
	try {
		const payload = req.body;
		const response = await axios.post(
			`https://f92clt.ws.newyorklife.com/PRO-MS/clt-leads-pxy/api/insertLeads`,
			{
				...payload,
			}
			// {
			// 	headers: {
			// 		Authorization: `Bearer ${token}`,
			// 	},
			// }
		);

		context.res = {
			body: {
				data: response.data,
			},
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = testLeadForm;
