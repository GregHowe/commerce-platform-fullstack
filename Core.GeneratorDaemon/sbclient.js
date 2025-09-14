const { ServiceBusClient } = require("@azure/service-bus");
const sbClient = new ServiceBusClient(process.env.AzureWebJobsServiceBus);
const receiver = sbClient.createReceiver("publishing", "Generator");
const sender = sbClient.createSender("publishing");

const sendMessage = async function (messages) {
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
    await sender.sendMessages(batch);
  } catch (err) {
    console.log(err);
  }
};

module.exports = { receiver, sender, sendMessage };
