const getGitanaSession = require("../lib/getGitanaSession.js");
const getUserToken = require("../lib/getUserToken.js");
const authAccount = async (context, req) => {
	const accessToken = getUserToken(req);
	if (!accessToken) {
		return {
			status: "Unauthorized!",
			error: new Error(),
		};
	}

	try {
		const session = getGitanaSession();
		await session.authenticate({
			accessToken,
		});

		const account = session?.authInfo.user || null;
		context.res = {
			body: {
				account,
			},
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = authAccount;
