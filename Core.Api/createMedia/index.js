// const axios = require("axios");
const projectId = process.env.CLOUDCMS_PROJECTID;
// const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
// const branchId = process.env.CLOUDCMS_BRANCHID;
// const getUserToken = require("../lib/getUserToken.js");
const storageConnectionString = process.env.AzureWebJobsStorage;
const { BlobServiceClient } = require("@azure/storage-blob");

const parseMultipartFormData =
	require("@anzp/azure-function-multipart").default;

/*gobu


pass upload object
upload: {
	name: xyz,
	type: mime/type,
	data: base64
}


*/

const createMedia = async (context, req) => {
	// content librarians need the ability to upload media to their organization rather than individual sites
	// but that would be handled at a different endpoint that does not have the siteId involved
	// possibly consolidate a lot of this logic into a shared file

	const siteId = req.params.siteId;
	if (!siteId) {
		throw Error("Site not found!");
	}
	if (!storageConnectionString) {
		throw Error("Connection String not found!");
	}
	try {
		const { fields, files } = await parseMultipartFormData(req);
		const blobServiceClient = BlobServiceClient.fromConnectionString(
			storageConnectionString
		);
		const targetPath = `$web/projects/${projectId}/websites/${siteId}/uploads`;
		const containerClient =
			blobServiceClient.getContainerClient(targetPath);
		if (!containerClient) {
			throw Error("Container Client not found!");
		}
		const file = files[0];
		if (!file) {
			throw Error("File not found!");
		}
		const filesize = parseInt(fields[1].value, 10);
		const blobClient = containerClient.getBlockBlobClient(file.filename);
		if (!blobClient) {
			throw Error("Blob Client not found!");
		}
		const response = await blobClient.upload(file.bufferFile, filesize, {
			blobHTTPHeaders: {
				blobContentType: file.mimeType,
			},
		});
		if (response._response.status !== 201) {
			throw Error(
				`Error uploading ${blobClient.name} to container ${blobClient.containerName}`
			);
		}

		// // for argument sake lets assume that worked
		// // second, update the cloudcms database
		// const token = getUserToken(req);

		// const mediaResponse = await axios.post(
		// 	`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes`,
		// 	{
		// 		_type: "media:asset",
		// 		title: "Untitled Media Asset",
		// 		url: response.
		// 	},
		// 	{
		// 		headers: {
		// 			Authorization: `Bearer ${token}`,
		// 		},
		// 	}
		// );

		// if (!pageResponse.data) {
		// 	throw Error("Media Asset create failed!");
		// }

		// const mediaId = mediaResponse.data._doc;

		// // pull down page data to send back
		// const pageData = await axios.get(
		// 	`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/nodes/${pageId}`,
		// 	{
		// 		headers: {
		// 			Authorization: `Bearer ${token}`,
		// 		},
		// 	}
		// );

		context.res = {
			body: {
				url: blobClient.url,
			},
		};
	} catch (err) {
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = createMedia;
