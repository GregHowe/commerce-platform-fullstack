const { SecretClient } = require("@azure/keyvault-secrets");
const { DefaultAzureCredential } = require("@azure/identity");

/* 
  DefaultAzureCredential works by detecting an authenticated session in the Azure CLI.
  Please install the Azure CLI (https://learn.microsoft.com/en-us/cli/azure/install-azure-cli), 
	then run 'az login'. If you have been granted access to the key vault, you will receive 
	secrets. Please set an environment variable 'KEY_VAULT_NAME' to specify a key vault.
*/

async function setEnvVariablesFromKeyvault(keyvault) {
	/*
		Sets process.env variables based on all secrets from specified keyvault
	*/

	const credential = new DefaultAzureCredential();
	const keyVaultName = keyvault || process.env["KEY_VAULT_NAME"];
	const url = `https://${keyVaultName}.vault.azure.net`;
	let client = new SecretClient(url, credential);
	for await (const secretProperties of client.listPropertiesOfSecrets()) {
		const secret = await client.getSecret(secretProperties.name);
		process.env[secret.name] = secret.value;
	}
}

async function getSecret(keyvault, secretName) {
	/* 
  	Gets a single secret by name
  */

	const credential = new DefaultAzureCredential();
	const keyVaultName = keyvault || process.env["KEY_VAULT_NAME"];
	const url = `https://${keyVaultName}.vault.azure.net`;
	const client = new SecretClient(url, credential);
	const secret = await client.getSecret(secretName);
	return secret.value;
}

module.exports = { setEnvVariablesFromKeyvault, getSecret };
