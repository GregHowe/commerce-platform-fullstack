const getUserToken = require("../lib/getUserToken.js")
const getGitanaSession = require("../lib/getGitanaSession.js")
const authLogout = async (context, req) => {

    const accessToken = getUserToken(req)
    if (!accessToken) {
        return {
            status: 'Unauthorized!',
            error: new Error(),
        }
    }

    try {

        const session = getGitanaSession();
        await session.authenticate({
            accessToken,
        });

        session.logout();
        context.res = {
            body: true
        };

    }

    catch (err) {
        context.res = {
            body: `${err}`,
            status: err?.response?.status,
        };
    }

}

module.exports = authLogout;
