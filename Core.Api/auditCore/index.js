const tableClient = require("../lib/tableStorage/tableClient");

module.exports = async function (context, coreEvent) {
	try {
		const auditClient = await tableClient("auditlog");
		context.log(coreEvent);
		await auditClient.createEntity({
			partitionKey: "audit",
			rowKey: "Log-" + new Date(),
			type: coreEvent.eventType || coreEvent.EventType,
			event: JSON.stringify(coreEvent),
		});

		return true;
	} catch (err) {
		console.log(err);
		return;
	}
};
