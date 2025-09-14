require("dotenv").config();
const path = require("path");
process.chdir = process.env.GeneratorPath;

(async function () {
  const setEnvFromSecrets = require("./keyvault");
  await setEnvFromSecrets();
  const { receiver, sendMessage } = require("./sbclient");
  const { spawn } = require("child_process");
  const blobUpload = require("./blobupload");
  const cdnUpdate = require("./cdnupdate");
  const prodpublish = require("./prodpublish");

  const generatorPath = path.resolve(process.cwd(), `../Core.Generator`);

  const generate = async (msg) => {
    console.log("Received: ", msg.body);
    const timestamp = Date.now();
    /* PRIMARY PUBLISH */
    if (msg.body.brandId && msg.body.siteId && msg.body.action == "publish") {
      /* Store and generate first payload with / as route base */
      const child = spawn("yarn generate", {
        shell: true,
        cwd: generatorPath,
        env: {
          ...process.env,
          timestamp,
          generatorBrandId: msg.body.brandId,
          generatorSiteId: msg.body.siteId,
          NUXT_ENV_API_BASE_URL: process.env.API_BASE_URL,
          NUXT_ENV_GOOGLE_MAPS_KEY: process.env.GoogleMapsAPIKey,
        },
      });
      console.log(
        `Build starting for Brand ${msg.body.brandId}, site ${msg.body.siteId}`
      );
      child.stdout.on("data", (data) => {
        //console.log(`stdout: ${data}`);
      });

      child.stderr.on("data", (data) => {
        //console.error(`generator error/warning ${data}`);
      });

      child.on("close", async (code) => {
        if (code == 0) {
          console.log(
            `Site ${msg.body.siteId} for Brand ${msg.body.brandId} generated.`
          );
          try {
            await blobUpload(msg.body.brandId, msg.body.siteId, timestamp);
          } catch (err) {
            console.log("Blob upload failed", err);
            sendMessage([
              {
                body: {
                  action: "publish_failed",
                  brandId: msg.body.brandId,
                  siteId: msg.body.siteId,
                  details: "Upload failed",
                },
              },
            ]);
          }
          /*
          try {
            
            await cdnUpdate(
              msg.body.brandId,
              msg.body.siteId,
              timestamp,
              `f92core-${msg.body.brandId}-${msg.body.siteId}-staging`,
              `/brands/${msg.body.brandId}/websites/${msg.body.siteId}/builds/${timestamp}`
            );
            
            sendMessage([
              {
                body: {
                  action: "publish_success",
                  brandId: msg.body.brandId,
                  siteId: msg.body.siteId,
                },
              },
            ]);
          } catch (err) {
            sendMessage([
              {
                body: {
                  action: "publish_failed",
                  brandId: msg.body.brandId,
                  siteId: msg.body.siteId,
                  details: err,
                },
              },
            ]);
          }
          */
        } else {
          console.log(
            `Error generating site ${msg.body.siteId} for Brand ${msg.body.brandId}`
          );
          sendMessage([
            {
              body: {
                action: "publish_failed",
                brandId: msg.body.brandId,
                siteId: msg.body.siteId,
              },
            },
          ]);
        }
      });
      /* Build with router base defined, for use on staging */
      const stageChild = spawn("yarn generate", {
        shell: true,
        cwd: generatorPath,
        env: {
          ...process.env,
          NODE_ENV: "staging",
          timestamp: timestamp + "stage",
          generatorBrandId: msg.body.brandId,
          generatorSiteId: msg.body.siteId,
          NUXT_ENV_API_BASE_URL: process.env.API_BASE_URL,
          NUXT_ENV_GOOGLE_MAPS_KEY: process.env.GoogleMapsAPIKey,
          VUE_ROUTER_BASE: `/${msg.body.brandId}/websites/${msg.body.siteId}/latest`,
        },
      });

      stageChild.on("close", async (code) => {
        if (code == 0) {
          console.log(
            ` Staging (latest) Site ${msg.body.siteId} for Brand ${msg.body.brandId} generated.`
          );
          try {
            await blobUpload(
              msg.body.brandId,
              msg.body.siteId,
              timestamp + "stage",
              `brands/${msg.body.brandId}/websites/${msg.body.siteId}/latest/`
            );
            sendMessage([
              {
                body: {
                  action: "publish_success",
                  brandId: msg.body.brandId,
                  siteId: msg.body.siteId,
                },
              },
            ]);
          } catch (err) {
            console.log("Blob upload failed", err);
            sendMessage([
              {
                body: {
                  action: "publish_failed",
                  brandId: msg.body.brandId,
                  siteId: msg.body.siteId,
                  details: `Blob upload failure: ${err}`,
                },
              },
            ]);
          }
        } else {
          console.log(
            `Error generating staging site ${msg.body.siteId} for Brand ${msg.body.brandId}`
          );
          sendMessage([
            {
              body: {
                action: "publish_failed",
                brandId: msg.body.brandId,
                siteId: msg.body.siteId,
              },
            },
          ]);
        }
      });
    }
    /* PROD PUBLISH (really a CDN update, no publish/upload here) */
    if (
      msg.body.brandId &&
      msg.body.siteId &&
      msg.body.action == "publish-prod"
    ) {
      try {
        await prodpublish(msg.body.brandId, msg.body.siteId);
        sendMessage([
          {
            body: {
              action: "prod_publish_success",
              brandId: msg.body.brandId,
              siteId: msg.body.siteId,
            },
          },
        ]);
      } catch (err) {
        sendMessage([
          {
            body: {
              action: "prod_publish_failed",
              brandId: msg.body.brandId,
              siteId: msg.body.siteId,
              details: err,
            },
          },
        ]);
      }
    }
  };

  const errorHandler = async (error) => {
    console.log(error);
  };

  receiver.subscribe({
    processMessage: generate,
    processError: errorHandler,
  });

  console.log("Generator Daemon Online.");

  generate({
    body: {
      action: "publish",
      brandId: process.env.TEST_BRAND_ID,
      siteId: process.env.TEST_SITE_ID,
    },
  });
})();
