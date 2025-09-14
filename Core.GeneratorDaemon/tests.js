const generate = require("./index");

(async function () {
  const setEnvFromSecrets = require("./keyvault");
  await setEnvFromSecrets();
  const { sender } = require("./sbclient");
  ////////////////TEST BELOW///////////////////
  // CALL GENERATE MANUALLY WITH A MOCK PAYLOAD
  /*
generate({
  body: {
    action: "publish",
    brandId: process.env.TEST_BRAND_ID,
    siteId: process.env.TEST_SITE_ID,
  },
});
*/
  // SEND TEST MESSAGES

  const messages = [
    // Should Succeed
    {
      body: {
        action: "publish",
        brandId: process.env.TEST_BRAND_ID,
        siteId: process.env.TEST_SITE_ID,
      },
    },
    // Should Fail
    /*
    {
      body: {
        action: "publish",
        brandId: "asdf",
        siteId: "asdf",
      },
    },
	*/
    // Shouldnt process (no action)
    /*
    {
      body: {
        brandId: process.env.TEST_BRAND_ID,
        siteId: process.env.TEST_SITE_ID,
      },
    },
	*/
    // Prod Publish
    /*
    {
      body: {
        action: "publish-prod",
        brandId: process.env.TEST_BRAND_ID,
        siteId: process.env.TEST_SITE_ID,
      },
    },
    */
  ];

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
})();
