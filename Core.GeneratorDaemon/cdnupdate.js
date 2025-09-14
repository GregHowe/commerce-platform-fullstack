const { CdnManagementClient } = require("@azure/arm-cdn");
const { DefaultAzureCredential } = require("@azure/identity");

const subscriptionId = "3420c08a-ba08-48d8-93f5-f139de540eec";
const client = new CdnManagementClient(
  new DefaultAzureCredential(),
  subscriptionId
);

const updateCDN = async function (brandId, siteId, timestamp, endpointName) {
  try {
    await client.endpoints.beginCreateAndWait(
      "NewYorkLife",
      "core-basic",
      endpointName,
      {
        location: "centralus",
        originPath: `/brands/${brandId}/websites/${siteId}/builds/${timestamp}`,
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
    console.log("CDN created/updated", endpointName);
    console.log("CDN purge commencing...", endpointName);
    await client.endpoints.beginPurgeContentAndWait(
      "NewYorkLife",
      "core-basic",
      endpointName,
      { contentPaths: ["/*"] }
    );
    console.log("CDN purged", endpointName);
  } catch (err) {
    console.log(err);
    throw new Error(err);
  }
};

module.exports = updateCDN;
