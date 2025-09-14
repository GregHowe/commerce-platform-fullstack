const axios = require("axios");
const personalAccessToken = process.env.MS_PAT;

const pipelineQueue = async (context, req) => {
	try {
		const auth = {
			username: "New-York-Life",
			password: personalAccessToken,
		};

		const pipelineResponse = await axios.get(
			`https://dev.azure.com/New-York-Life/NYL%20Core/_apis/pipelines/26/runs?api-version=6.0-preview.1`,
			{ auth }
		);
		const inProgressBuilds = pipelineResponse.data.value.filter(
			(v) => v.state == "inProgress"
		);
		context.res = {
			body: {
				queue: inProgressBuilds.reverse(),
			},
		};
	} catch (err) {
		console.error(err);
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = pipelineQueue;
