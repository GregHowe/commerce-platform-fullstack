const axios = require("axios");
const { loadFile } = require("graphql-import-files");
const repositoryId = process.env.CLOUDCMS_REPOSITORYID;
const branchId = process.env.CLOUDCMS_BRANCHID;
const getUserToken = require("../lib/getUserToken.js");

const getSite = async (context, req) => {
	const siteId = req.params.siteId;
	if (!siteId) {
		return {
			status: "Site not found!",
			error: new Error(),
		};
	}
	try {
		const token = getUserToken(req);
		const query = loadFile("./getSite/query.gql");
		const response = await axios.post(
			`https://api.cloudcms.com/repositories/${repositoryId}/branches/${branchId}/graphql`,
			{
				query,
				variables: {
					siteId,
				},
			},
			{
				headers: {
					Authorization: `Bearer ${token}`,
				},
			}
		);
		const sites = response?.data?.data?.frontend_sites || [];

		const site = sites[0];
		if (!site) {
			throw Error("Could not load site!");
		}
		site.brand_id = "nyl";
		if (!site.pages) {
			site.pages = [];
		}
		site.pages.forEach((page) => {
			if (!page.blocks) {
				page.blocks = [];
			} else {
				page.blocks = JSON.parse(page.blocks);
			}
		});

		if (!site.theme || !site.theme.indexOf("color")) {
			site.theme = {
				color: "",
				font: "",
			};
		} else {
			site.theme = JSON.parse(site.theme);
		}

		if (!site.navigation) {
			site.navigation = {
				logo: "",
				logoAltText: "",
				logoLink: "",
				name: "Name",
				jobTitle: "Title",
				buttonText: "",
				buttonLink: "",
				buttonTarget: true,
				logoStyle: "large",
				menuAlignment: "left",
			};
		} else {
			site.navigation = JSON.parse(site.navigation);
		}

		if (!site.footer) {
			site.footer = {
				logo: "",
				logoAltText: "",
				logoLink: "",
				name: "Name",
				jobTitle: "Title",
				logoStyle: "large",
				menuAlignment: "left",
				facebook: "",
				instagram: "",
				linkedIn: "",
				youTube: "",
				signUpFormDisclosure: "",
				showSignUpForm: false,
				disclosures: [],
			};
		} else {
			site.footer = JSON.parse(site.footer);
		}

		context.res = {
			body: {
				data: {
					site,
				},
			},
		};
	} catch (err) {
		console.error(err);
		context.res = {
			body: `${err}`,
			status: err?.response?.status,
		};
	}
};

module.exports = getSite;
