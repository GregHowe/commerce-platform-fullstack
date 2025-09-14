const { TableClient, AzureNamedKeyCredential } = require("@azure/data-tables");
const tableServiceClient = require("./tableServiceClient");
const storageAccount = "agentdeployments";
const accountKey = process.env.AzureWebJobsStorageKey;
const credential = new AzureNamedKeyCredential(storageAccount, accountKey);

// Create a new client table and prefix it with the proper environment we're on (so we can support table sets per env)

module.exports = async (tablename, prefix) => {
	try {
		await new Promise((resolve) => {
			tableServiceClient.createTable(tablename).then(() => resolve());
		});
		return new TableClient(
			`https://${storageAccount}.table.core.windows.net`,
			tablename,
			credential
		);
	} catch (err) {
		return {};
	}
};
