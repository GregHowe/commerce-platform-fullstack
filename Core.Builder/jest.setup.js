import Vue from "vue";
import { config } from "@vue/test-utils";

Vue.config.productionTip = false;

const path = require("path");
const glob = require("glob");

config.stubs = {
	ClientOnly: {
		template: "<div><slot /></div>",
	},
	NuxtChild: { template: "<div><slot /></div>" },
	NuxtLink: { template: "<a><slot /></a>" },
	NoSsr: { template: "<span><slot /></span>" },
	CoreIcon: { template: "<span><slot /></span>" },
	VueAceEditor: { template: "<span><slot /></span>" },
};

config.mocks = {
	$gsap: { to: jest.fn() },
};

// this is required to allow us to use auto import of nuxt components
// https://github.com/nuxt/components/issues/58
glob.sync(path.join(__dirname, "./components/**/*.vue")).forEach((file) => {
	const name = file.match(/(\w*)\.vue$/)[1];
	Vue.component(name, require(file).default);
});

global.Vue = Vue;
document.body.setAttribute("data-app", true);

const storePath = `${process.env.buildDir}/store.js`;
global.store = async (extension = {}) => {
	const nuxt_store = await import(storePath);
	const store = nuxt_store.createStore();
	store.commit("site/extendSite", extension);
	return store;
};
