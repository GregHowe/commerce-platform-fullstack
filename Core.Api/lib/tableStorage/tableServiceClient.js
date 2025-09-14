const {
	TableServiceClient,
	AzureNamedKeyCredential,
} = require("@azure/data-tables");
const storageAccount = "agentdeployments";
const accountKey = process.env.AzureWebJobsStorageKey;
const credential = new AzureNamedKeyCredential(storageAccount, accountKey);

const client = new TableServiceClient(
	`https://${storageAccount}.table.core.windows.net`,
	credential
);
module.exports = client;
