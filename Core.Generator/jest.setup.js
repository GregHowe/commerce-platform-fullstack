import Vue from "vue";
import Vuetify from "vuetify";
import { config, createLocalVue } from "@vue/test-utils";

Vue.use(Vuetify);
Vue.config.productionTip = false;

const localVue = createLocalVue();

const vuetify = new Vuetify({});
const path = require("path");
const glob = require("glob");

config.stubs = {
	ClientOnly: {
		template: "<div><slot /></div>",
	},
};

localVue.use(vuetify);
// this is required to allow us to use auto import of nuxt components
// https://github.com/nuxt/components/issues/58
glob.sync(path.join(__dirname, "./components/**/*.vue")).forEach((file) => {
	const name = file.match(/(\w*)\.vue$/)[1];
	Vue.component(name, require(file).default);
});

global.Vue = Vue;

export default {
	localVue,
	vuetify,
};
