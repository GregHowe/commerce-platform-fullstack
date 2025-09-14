module.exports = {
	globalSetup: "<rootDir>/jest.nuxtsetup.js",
	moduleNameMapper: {
		"^@/(.*)$": "<rootDir>/$1",
		"^~/(.*)$": "<rootDir>/$1",
		"^vue$": "vue/dist/vue.common.js",
		"^vuetify/lib$": "vuetify/es5/entry-lib",
		"^vuetify/lib/(.*)": "vuetify/es5/$1",
		"\\.(css|less|scss|sass)$": "identity-obj-proxy",
	},
	moduleFileExtensions: ["js", "vue", "json"],
	transform: {
		"^.+\\.js$": "babel-jest",
		".*\\.(vue)$": "vue-jest",
	},
	collectCoverage: true,
	collectCoverageFrom: [
		"<rootDir>/components/**/*.vue",
		"<rootDir>/pages/**/*.vue",
	],
	testEnvironment: "jsdom",
	setupFiles: ["<rootDir>/jest.setup.js"],
};
