const getUserToken = function (req) {

    // just needs to pass a request with authorization headers
    // that include a Bearer token

    const authHeader = req?.headers.authorization || null;
    if (!authHeader) {
        return null;
    }

    if (authHeader.indexOf('Bearer') === -1) {
        return null;
    }

    return authHeader.split(' ')[1];

};

module.exports = getUserToken;
