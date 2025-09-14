module.exports = {
	moduleNameMapper: {
		"^@/(.*)$": "<rootDir>/$1",
		"^~/(.*)$": "<rootDir>/$1",
		"@libraryHelpers/(.*)": "<rootDir>/src/helpers/$1",
		"^vue$": "vue/dist/vue.common.js",
		"\\.(css|less|scss|sass)$": "identity-obj-proxy",
	},
	moduleFileExtensions: ["js", "vue", "json"],
	transform: {
		"^.+\\.js$": "babel-jest",
		".*\\.(vue)$": "vue-jest",
	},
	transformIgnorePatterns: [],
	collectCoverage: true,
	collectCoverageFrom: ["<rootDir>/components/**/*.vue"],
	testEnvironment: "jsdom",
	setupFiles: ["<rootDir>/jest.setup.js"],
};
