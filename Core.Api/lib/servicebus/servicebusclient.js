const { ServiceBusClient } = require("@azure/service-bus");
const connectionString = process.env.AzureWebJobsServiceBus;

const sendMessagesToTopic = async (topicName, messages) => {
	const sbClient = new ServiceBusClient(connectionString);
	const sender = sbClient.createSender(topicName);
	try {
		let batch = await sender.createMessageBatch();
		for (let i = 0; i < messages.length; i++) {
			if (!batch.tryAddMessage(messages[i])) {
				await sender.sendMessages(batch);
				batch = await sender.createMessageBatch();
				if (!batch.tryAddMessage(messages[i])) {
					throw new Error("Message too big to fit in a batch");
				}
			}
		}
		await sender.sendMessages(messages);
		console.log(`Sent a batch of messages to the topic: ${topicName}`);
		await sender.close();
	} finally {
		await sbClient.close();
		return Promise.resolve(true);
	}
};

module.exports = sendMessagesToTopic;
