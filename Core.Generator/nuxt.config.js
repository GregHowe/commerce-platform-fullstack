import { resolve } from "path";
import fs from "fs";
const isProductionBuild = process.env.ENVIRONMENT != "staging";
if (!isProductionBuild) {
	//process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
}
const getAppToken = require("./queries/getAppToken.js");
const getSite = require("./queries/getSite.js");

const distDir = `dist/Brand-${process.env.generatorBrandId}-Site-${process.env.generatorSiteId}-${process.env.timestamp}`;

export default async function () {
	const appToken = await getAppToken();
	const siteData = await getSite(appToken);
	const cssDependencies = [
		"~/../Core.Library/assets/css/global.css",
		"~/../Core.Library/assets/css/variables.css",
		"~/../Core.Library/assets/chota/_base.scss",
		"~/../Core.Library/assets/chota/_grid.scss",
		"~/../Core.Library/assets/chota/_form.scss",
		"~/../Core.Library/assets/chota/_nav.scss",
		"~/../Core.Library/assets/chota/_tab.scss",
		"~/../Core.Library/assets/chota/_tag.scss",
		"~/../Core.Library/assets/chota/_dropdown.scss",
		"~/../Core.Library/assets/chota/_util.scss",
		"~/../Core.Builder/static/themes/nyl-01.css", // ALWAYS load this one first to set the defaults
	];
	const selectedThemeBase = siteData?.themes?.base || null;
	if (selectedThemeBase) {
		cssDependencies.push(
			`~/../Core.Builder/static/themes/${selectedThemeBase}.css`
		);
	}

	return {
		router: {
			base: process.env.VUE_ROUTER_BASE || "/",
		},
		dev: !isProductionBuild,

		// Global page headers: https://go.nuxtjs.dev/config-head
		head: {
			// __dangerouslyDisableSanitizers: ['script'],
			titleTemplate: `%s - ${siteData.title} ${
				!isProductionBuild ? "Development Build" : ""
			}`,
			title: "NYL",
			script: [
				{
					src: "https://assets.calendly.com/assets/external/widget.js",
				}
				// ,{
				// 	type:'text/javascript',   charset: 'utf-8',  innerHTML: `<!-- Google Tag Manager -->
				// 	(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
				// 	new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
				// 	j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
				// 	'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
				// 	})(window,document,'script','dataLayer','GTM-XXXXXX');
				// 	<!-- End Google Tag Manager -->`
				//   }
			],
			// body:[
			// 	  {
			// 		charset: 'utf-8',  innerHTML: `<!-- Google Tag Manager (noscript) -->
			// 		<iframe src="https://www.googletagmanager.com/ns.html?id=GTM-XXXXXX"
			// 		height="0" width="0" style="display:none;visibility:hidden"></iframe>
			// 		<!-- End Google Tag Manager (noscript) -->`
			// 	  }
			// ],
			htmlAttrs: {
				lang: "en",
			},
			meta: [
				{ charset: "utf-8" },
				{
					name: "viewport",
					content: "width=device-width, initial-scale=1",
				},
				{ hid: "description", name: "description", content: "" },
				{ name: "format-detection", content: "telephone=no" },
				{
					name: "f92-core-timestamp",
					content: new Date().toGMTString(),
				},
			],
			link: [
				{ rel: "icon", type: "image/x-icon", href: "/favicon.ico" },
				{
					rel: "stylesheet",
					href: "https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css",
				},
				{
					rel: "stylesheet",
					href: "https://calendly.com/assets/external/widget.css",
				},
			],
		},
		// Global CSS: https://go.nuxtjs.dev/config-css
		css: cssDependencies,

		styleResources: {
			scss: ["~/../Core.Library/assets/scss/mixins.scss"],
		},

		eslint: {
			fix: true,
		},

		// Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
		plugins: [
			"~/plugins/util",
			"~/plugins/axios",
			"~/plugins/vee-validate",
			"~/plugins/mustache",
			"~/plugins/vue-the-mask",
		],

		// Auto import components: https://go.nuxtjs.dev/config-components
		components: [
			{
				path: resolve(__dirname, "../Core.Library/src/components"),
				pathPrefix: false,
				watch: true,
			},
			{
				path: "~/components",
			},
		],

		// Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
		buildModules: [
			[
				"@nuxtjs/eslint-module",
				{
					fix: true,
				},
			],
			//"@nuxtjs/html-validator", // too noisey for now
			"@nuxtjs/style-resources",
		],

		// Modules: https://go.nuxtjs.dev/config-modules
		modules: [
			"nuxt-esbuild",
			"@nuxtjs/axios",
			"nuxt-webfontloader",
			"@nuxtjs/recaptcha",
			'@nuxtjs/gtm'
		],
		gtm: {
			enabled: true,
			id: 'GTM-54ZKWF8',
			// autoInit: true
		  },
		recaptcha: {
			hideBadge: true,
			mode: "base",
			siteKey: process.env.NUXT_ENV_GOOGLE_RECAPTCHA_SITE_KEY,
			version: 3,
		},

		axios: {
			baseURL:
				process.env.NUXT_ENV_API_BASE_URL ||
				"https://localhost:7283/api",
		},

		publicRuntimeConfig: {
			appToken,
			siteData,
		},

		generate: {
			dir: distDir,
			routes: async () => {
				// since the site is static, we need to specify what pages (routes) to
				// generate in advance, in addition to the normal directory/file setup
				// required for dynamic pages within nuxt.
				try {
					const handles = siteData.pages.reduce((handles, page) => {
						if (page.id !== siteData.homepageId) {
							if (page.parentPageId) {
								let parentPage = siteData.pages.find(
									(p) => page.parentPageId === p.id
								);
								if (parentPage.handle) {
									handles.push(
										`/${parentPage.handle}/${page.handle}`
									);
								} else {
									handles.push(`/${page.handle}`);
								}
							} else {
								handles.push(`/${page.handle}`);
							}
						} else {
							page.handle = "";
							handles.push("/");
						}
						return handles;
					}, []);
					return handles;
				} catch (err) {
					throw new Error(
						"Couldnt resolve routes, likely cannot load site data"
					);
				}
			},
		},

		// Build Configuration: https://go.nuxtjs.dev/config-build
		build: {
			cache: !isProductionBuild,
			parallel: !isProductionBuild,
			optimizeCSS: isProductionBuild,
			html: { minify: isProductionBuild },
			transpile: ["vee-validate/dist/rules"],
			// eslint-disable-next-line no-empty-pattern
			extend(config, {}) {
				config.node = {
					fs: "empty",
				};
			},
		},
		target: "static",
		alias: {
			"@builder": resolve(__dirname, "../Core.Builder"),
			"@themes": resolve(__dirname, "../Core.Library/assets/themes"),
			"@libraryHelpers": resolve(
				__dirname,
				"../Core.Library/src/helpers"
			),
		},
		hooks: {
			generate: {
				done: (builder) => {
					fs.writeFileSync(
						`${distDir}/sitedata.json`,
						JSON.stringify(siteData)
					);
				},
			},
		},
	};
}
