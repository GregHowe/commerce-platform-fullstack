const processSAMLResponse = async function (body) {
	console.debug(`processing SAML response: ${body}`);
	const encodedSAML = body.replace("SAMLResponse=", "");
	if (!encodedSAML) {
		return null;
	}
	try {
		const uriDecoded = decodeURIComponent(encodedSAML);
		const b64decoded = new Buffer.from(uriDecoded, "base64");

		const q = new RegExp(
			`<saml:NameID Format=\"urn:oasis:names:tc:SAML:1.1:nameid-format:unspecified\">([a-z0-9]+)</saml:NameID>`
		);
		const matches = b64decoded.toString().match(q);
		return matches ? matches[1] : null;
	} catch (err) {
		console.error(err);
		return null;
	}
};
module.exports = processSAMLResponse;
