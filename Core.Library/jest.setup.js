import Vue from "vue";
import { config } from "@vue/test-utils";

Vue.config.productionTip = false;

const path = require("path");
const glob = require("glob");

// this is required to allow us to use auto import of nuxt components
// https://github.com/nuxt/components/issues/58
glob.sync(path.join(__dirname, "./src/components/**/*.vue")).forEach((file) => {
	const name = file.match(/(\w*)\.vue$/)[1];
	Vue.component(name, require(file).default);
});

global.Vue = Vue;
global.Mustache = require("mustache");

config.stubs = {
	CoreIcon: { template: "<span><slot /></span>" },
	NuxtLink: { template: "<a test-id='nuxtlink-stub'><slot /></a>" },
};
