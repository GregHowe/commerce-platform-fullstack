const { CdnManagementClient } = require("@azure/arm-cdn");
const { DefaultAzureCredential } = require("@azure/identity");

const subscriptionId = "3420c08a-ba08-48d8-93f5-f139de540eec";
const client = new CdnManagementClient(
  new DefaultAzureCredential(),
  subscriptionId
);

const prodPublish = async function (brandId, siteId) {
  try {
    const endpointName = `f92core-${brandId}-${siteId}`;
    const staging = await client.endpoints.get(
      "NewYorkLife",
      "core-basic",
      `${endpointName}-staging`
    );

    await client.endpoints.beginCreateAndWait(
      "NewYorkLife",
      "core-basic",
      endpointName,
      {
        location: "centralus",
        originPath: staging.originPath,
        isCompressionEnabled: true,
        contentFilePaths: {
          contentPaths: [],
        },
        contentTypesToCompress: [
          "text/plain",
          "text/html",
          "text/css",
          "text/javascript",
          "application/x-javascript",
          "application/javascript",
          "application/json",
          "application/xml",
          "image/png",
          "image/jpg",
          "image/gif",
          "video/mp4",
        ],
        originHostHeader: "agentdeployments.z19.web.core.windows.net",
        origins: [
          {
            name: "origin-0",
            hostName: "agentdeployments.z19.web.core.windows.net",
          },
        ],
      }
    );
    console.log("Prod CDN created/updated", endpointName);
    console.log("Prod CDN purge commencing...", endpointName);
    await client.endpoints.beginPurgeContentAndWait(
      "NewYorkLife",
      "core-basic",
      endpointName,
      { contentPaths: ["/*"] }
    );
    console.log("Prod CDN purged", endpointName);
  } catch (err) {
    console.log(err);
    throw new Error(err);
  }
};

module.exports = prodPublish;
