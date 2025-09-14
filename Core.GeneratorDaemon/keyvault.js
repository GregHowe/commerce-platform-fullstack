const { SecretClient } = require("@azure/keyvault-secrets");
const { ClientSecretCredential } = require("@azure/identity");

const credential = new ClientSecretCredential(
  process.env.AZURE_TENANT_ID,
  process.env.AZURE_CLIENT_ID,
  process.env.AZURE_CLIENT_SECRET,
  { additionallyAllowedTenants: "*" }
);

const keyVaultName = process.env["KEY_VAULT_NAME"];
const url = `https://${keyVaultName}.vault.azure.net`;

const client = new SecretClient(url, credential);

const secrets = [
  "AzureWebJobsServiceBus",
  "AZURE-STORAGE-KEY",
  "GeneratorUsername",
  "GeneratorPassword",
  "GoogleMapsAPIKey",
];

async function setEnvFromSecrets() {
  for (const secretName of secrets) {
    let secret = await client.getSecret(secretName);
    process.env[secretName] = secret.value;
  }
}

module.exports = setEnvFromSecrets;
