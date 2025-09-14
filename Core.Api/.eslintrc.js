module.exports = {
	root: true,
	env: {
		browser: true,
		node: true,
	},
	plugins: ["prettier"],
	extends: [
		"plugin:vue/recommended",
		"eslint:recommended",
		"prettier",
		"plugin:prettier/recommended",
	],
	rules: {
		indent: ["error", "tab", { SwitchCase: 1 }],
		"no-console": process.env.NODE_ENV === "production" ? 2 : 1,
		"no-debugger": process.env.NODE_ENV === "production" ? 2 : 1,
		"no-unused-vars": process.env.NODE_ENV === "production" ? 2 : 1,
		"vue/no-v-html": "off",
		"vue/multi-word-component-names": 0,
		"vue/component-name-in-template-casing": ["error", "PascalCase"],
		"prettier/prettier": ["error"], //https://github.com/prettier/eslint-plugin-prettier
	},
	globals: {
		$nuxt: true,
	},
};
