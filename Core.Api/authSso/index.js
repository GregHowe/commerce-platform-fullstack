const processSAMLResponse = require("../lib/processSAMLResponse.js");
const fetchUserData = require("../lib/fetchUserData.js");
const fn = async (context, req) => {
	const userId = await processSAMLResponse(req.body);
	let userData = null;
	if (!!userId) {
		userData = await fetchUserData(userId);
	}
	context.res = {
		body: {
			message: userId && userData ? "Success" : "Fail",
			userId: userId || null,
			userData: userData || null,
		},
	};
};
module.exports = fn;
