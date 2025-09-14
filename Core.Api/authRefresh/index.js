const getGitanaSession = require("../lib/getGitanaSession.js");
const authRefresh = async (context, req) => {
	try {
		const session = getGitanaSession();
		const refreshToken = (await req.body?.refresh_token) || null;
		const timeout = (prom, time) =>
			Promise.race([
				prom,
				new Promise((_r, rej) => setTimeout(rej, time)),
			]);

		await timeout(
			session.authenticate({
				refreshToken,
			}),
			3000
		);
		const accessToken = session?.authInfo.accessToken || null;
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

module.exports = authRefresh;
