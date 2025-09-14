const getGitanaSession = require("../lib/getGitanaSession.js");

const authLogin = async (context, req) => {
	const username = req.body?.username || null;

	if (!username) {
		return {
			status: "Username required.",
			error: new Error(),
		};
	}

	const password = req.body?.password || null;
	if (!password) {
		return {
			status: "Password required.",
			error: new Error(),
		};
	}

	try {
		const session = getGitanaSession();
		const timeout = (prom, time) =>
			Promise.race([
				prom,
				new Promise((_r, rej) => setTimeout(rej, time)),
			]);
		await timeout(
			session.authenticate({
				username,
				password,
			}),
			3000
		);
		const accessToken = session?.authInfo.accessToken || null;
		const refreshToken = session.http.refreshToken();
		context.res = {
			body: {
				accessToken,
				refreshToken,
			},
		};
	} catch (err) {
		console.debug("ERROR", err);
		context.res = {
			body: "UNAUTHORIZED",
			status: 401,
		};
	}
};

module.exports = authLogin;
