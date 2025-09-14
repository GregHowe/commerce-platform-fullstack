const axios = require("axios");
const baseURL = process.env.NUXT_ENV_API_BASE_URL || "https://localhost:7283/api";

const username = process.env.GeneratorUsername;
const password = process.env.GeneratorPassword;

const getAppToken = async () => {
	try {
		const response = await axios.post(`${baseURL}/auth/login`, {
			username,
			password,
		});
		const oauth = response.data;
		if (oauth.success) {
			return oauth.token;
		}
		return null;
	} catch (err) {
		console.error(err);
		return null;
	}
};

module.exports = getAppToken;
