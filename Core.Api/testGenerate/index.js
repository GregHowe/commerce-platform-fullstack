const testGenerate = async (context, req) => {
	try {
		const siteId = req.query.siteId;
		const brandId = req.query.brandId;

		context.bindings.sendPublishMessages = {
			action: "publish",
			brandId,
			siteId,
		};
		context.res = {
			body: {
				message: "Success",
			},
		};
	} catch (err) {
		context.log(err);
	}
};

module.exports = testGenerate;
