const healthCheck = async (context) => {
	context.res = {
		body: "Hello world",
	};
};

module.exports = healthCheck;
