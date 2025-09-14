const {
  BlobServiceClient,
  StorageSharedKeyCredential,
} = require("@azure/storage-blob");
const fs = require("fs");

require("dotenv").config();

const accountName = "agentdeployments";
const accountKey = process.env["AZURE-STORAGE-KEY"];
const sharedKeyCredential = new StorageSharedKeyCredential(
  accountName,
  accountKey
);

const blobServiceClient = new BlobServiceClient(
  `https://${accountName}.blob.core.windows.net`,
  sharedKeyCredential
);

async function main(brandId, siteId, timestamp, bN) {
  const dir = `../Core.Generator/dist/Brand-${brandId}-Site-${siteId}-${timestamp}`;
  if (!fs.existsSync(`${dir}/index.html`)) {
    throw new Error("Site generation failed: no index.html");
  }

  const containerName = "$web";
  const blobName =
    bN || `brands/${brandId}/websites/${siteId}/builds/${timestamp}/`;
  const containerClient = await blobServiceClient.getContainerClient(
    containerName
  );

  // upload file to blob storage

  const arr = getAllFiles(dir);
  const p = `/Brand-${brandId}-Site-${siteId}-${timestamp}`;
  for (const file of arr) {
    let blockBlobClient = await containerClient.getBlockBlobClient(
      blobName + file.slice(file.lastIndexOf(p) + p.length + 1)
    );
    let options = {};
    if (file.lastIndexOf(".html") > -1) {
      options = { blobHTTPHeaders: { blobContentType: "text/html" } };
    }
    await blockBlobClient.uploadFile(file, options);
  }
  console.log(`Site payload uploaded: ${timestamp}`);
  fs.rmSync(dir, { recursive: true, force: true });
  return;
}

const getAllFiles = function (dirPath, arrayOfFiles) {
  files = fs.readdirSync(dirPath);

  arrayOfFiles = arrayOfFiles || [];

  files.forEach(function (file) {
    if (fs.statSync(dirPath + "/" + file).isDirectory()) {
      arrayOfFiles = getAllFiles(dirPath + "/" + file, arrayOfFiles);
    } else {
      arrayOfFiles.push(dirPath + "/" + file);
    }
  });

  return arrayOfFiles;
};

module.exports = main;
