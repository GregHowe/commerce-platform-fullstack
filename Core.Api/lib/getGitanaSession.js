const config = require("../gitana.json");
const Gitana = require("gitana");

const getGitanaSession = function () {
	return new Gitana({
		clientKey: config.clientKey,
		clientSecret: config.clientSecret,
	});
};

module.exports = getGitanaSession;
