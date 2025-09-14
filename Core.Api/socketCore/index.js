module.exports = async function (context, coreEvent) {
	try {
		context.bindings.actions = {
			actionName: "sendToAll",
			data: JSON.stringify(coreEvent),
		};
		// UserEventResponse directly return to caller
		var response = {
			data: "[SYSTEM] ack.",
			dataType: "text",
		};
		return response;
	} catch (err) {
		console.log(err);
		return;
	}
};
