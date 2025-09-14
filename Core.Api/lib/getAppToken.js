const axios = require("axios");
const config = require("../gitana.json");
const token = Buffer.from(
	`${config.clientKey}:${config.clientSecret}`,
	"ascii"
).toString("base64");

const getAppToken = async () => {
	try {
		const oauth = await axios.post(
			"https://api.cloudcms.com/oauth/token",
			`username=${config.username}&password=${encodeURIComponent(
				config.password
			)}&grant_type=password&scope=api`,
			{
				headers: {
					Authorization: `Basic ${token}`,
				},
			}
		);
		return oauth.data.access_token;
	} catch (err) {
		console.error(err);
		return null;
	}
};

module.exports = getAppToken;
