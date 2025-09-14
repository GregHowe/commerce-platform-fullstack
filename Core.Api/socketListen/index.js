module.exports = function (context, data, connection) {
	try {
		context.log("connection");
		context.log(
			"Request from: ",
			context.bindingData.request.connectionContext.userId
		);
		context.log("Request message data: ", data);
		context.log(
			"Request message dataType: ",
			context.bindingData.request.dataType
		);
		context.bindings.actions = {
			actionName: "sendToAll",
			data: JSON.stringify(data),
		};
	} catch (err) {
		context.log(err);
	}
	return;
};
