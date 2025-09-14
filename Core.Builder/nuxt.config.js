import { resolve } from "path";
import { readFile } from "fs/promises";
const dotnetApiBase = process.env.API_BASE_URL || "/api";
let isProductionBuild =
	process.env.NODE_ENV === "production" || process.env.NODE_ENV === "staging";

if (process.env.API_BASE_URL) {
	process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
}
export default async function () {
	const D = await readFile("env_defaults.json", "utf8");
	const defaults = JSON.parse(D);
	Object.keys(defaults).forEach(
		(k) => (process.env[k] = process.env[k] || defaults[k])
	);

	return {
		dev: !isProductionBuild,
		configureWebpack: {
			devServer: {
				port: 80,
				watchOptions: {
					poll: true,
				},
			},
		},
		static: {
			prefix: false,
		},
		// Global page headers: https://go.nuxtjs.dev/config-head
		head: {
			titleTemplate: `%s - Core Builder ${
				!isProductionBuild ? "Development Build" : ""
			}`,
			title: "NYL",
			script: [
				{
					src: "https://assets.calendly.com/assets/external/widget.js",
				},
			],
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
			],
			link: [
				{ rel: "icon", type: "image/x-icon", href: "/favicon.ico" },
				{
					rel: "stylesheet",
					href: "https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css",
				},
				{
					rel: "stylesheet",
					href: "https://fonts.googleapis.com/css?family=Material+Icons|Material+Icons+Outlined|Material+Icons+Two+Tone|Material+Icons+Round|Material+Icons+Sharp",
				},
			],
		},
		// Global CSS: https://go.nuxtjs.dev/config-css
		css: [
			"~/assets/editorstyles/_main.css",
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
			"~/../Core.Library/assets/chota/_util.scss",
			"~/static/themes/nyl-01.css", // this is always loaded as the BASE theme, choosing another will override the values here
		],

		styleResources: {
			scss: ["~/../Core.Library/assets/scss/mixins.scss"],
		},

		eslint: {
			fix: true,
		},

		// Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
		plugins: [
			"~/plugins/axios",
			"~/plugins/axios.nodefunctions",
			"~/plugins/vee-validate",
			"~/plugins/ws.client",
			"~/plugins/ace.client",
			"~/plugins/mustache",
			"~/plugins/vue-the-mask",
			"~/plugins/vuex-persistedstate.client",
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
			// https://go.nuxtjs.dev/vuetify
			"@nuxtjs/vuetify",
			[
				"@nuxtjs/eslint-module",
				{
					fix: true,
				},
			],
			//"@nuxtjs/html-validator", //too noisy
			"@nuxtjs/style-resources",
			"nuxt-gsap-module",
		],

		router: {
			middleware: [
				"loadUserFromToken",
				"auth",
				// "redirectLanding", // will be used eventually to gate access based on T&C acceptance
				"preloader",
			],
		},

		// Modules: https://go.nuxtjs.dev/config-modules
		modules: [
			"nuxt-esbuild",
			"@nuxtjs/axios",
			"@nuxtjs/auth-next",
			"nuxt-webfontloader",
			"@nuxtjs/recaptcha",
		],
		publicRuntimeConfig: {
			axios: {
				baseURL: dotnetApiBase,
			},
			API_BASE_URL: dotnetApiBase,
			API_NODE_CODE: process.env.API_NODE_CODE,
			API_NODE_FUNCTIONS: process.env.API_NODE_FUNCTIONS,
			DEV_USERNAME: process.env.DEV_USERNAME,
			DEV_PW: process.env.DEV_PW,
			buildTime: new Date(),
			AUTOSAVE_DEFAULT: process.env.AUTOSAVE_DEFAULT || false,
		},

		auth: {
			redirect: {
				login: "/auth/login",
				callback: false,
				//logout: "/auth/login",
				//home: "/",
			},
			rewriteRedirects: true,
			localStorage: false,
			strategies: {
				local: {
					scheme: "refresh",
					token: {
						property: "token",
						global: true,
						required: true,
					},
					refreshToken: {
						property: "refreshToken",
						data: "refreshToken",
						maxAge: 60 * 60 * 24 * 30,
					},
					user: {
						property: false,
						autoFetch: true,
					},
					endpoints: {
						login: {
							url: `/auth/login`,
							method: "post",
						},
						logout: {
							url: `/auth/logout`,
							method: "get",
						},
						user: {
							url: `/users/account`,
							method: "get",
						}, // lookup user data manually
						refresh: { url: "/auth/refresh", method: "post" },
					},
					scope: "api",
					fullPathRedirect: true,
				},
			},
		},

		recaptcha: {
			hideBadge: true,
			mode: "base",
			siteKey: process.env.NUXT_ENV_GOOGLE_RECAPTCHA_SITE_KEY,
			version: 3,
		},

		// Vuetify module configuration: https://go.nuxtjs.dev/config-vuetify
		vuetify: {
			customVariables: ["~/assets/scss/vuetify-variables.scss"],
			treeShake: true,
			theme: {
				options: { customProperties: true },
				themes: {
					light: {
						primary: "#000000",
						secondary: {
							base: "#BCBCBC",
							darken1: "#989898",
							darken2: "#565656",
							darken3: "#343434",
							lighten1: "#DCDCDC",
							lighten2: "#F2F2F2",
							lighten3: "#F8F8F8",
						},
						accent: "#2B9CE0",
						error: "#DE0D0D",
						info: "#2196F3",
						success: "#2E8540",
						warning: "#F17B00",
						anchor: "#2B9CE0",
					},
					dark: {
						primary: "#ffffff",
						secondary: {
							base: "#BCBCBC",
							darken1: "#989898",
							darken2: "#565656",
							darken3: "#343434",
							lighten1: "#DCDCDC",
							lighten2: "#F2F2F2",
							lighten3: "#F8F8F8",
						},
						accent: "#2B9CE0",
						error: "#DE0D0D",
						info: "#2196F3",
						success: "#2E8540",
						warning: "#F17B00",
						anchor: "#2B9CE0",
					},
				},
			},
		},
		alias: {
			"@schemas": resolve(__dirname, "../Core.Library/src/schemas"),
			"@libraryHelpers": resolve(
				__dirname,
				"../Core.Library/src/helpers"
			),
		},
		// Build Configuration: https://go.nuxtjs.dev/config-build
		build: {
			cache: !isProductionBuild,
			parallel: !isProductionBuild,
			optimizeCSS: isProductionBuild,
			html: { minify: isProductionBuild },
			transpile: ["vee-validate/dist/rules"],
			devtools: true,
		},
		ssr: false,
		target: "server",
		watchers: {
			webpack: {
				poll: 1000,
			},
		},
	};
}
