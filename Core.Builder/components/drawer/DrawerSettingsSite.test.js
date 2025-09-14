import siteSchema from "~/schemas/site.json";
import { createLocalVue, mount } from "@vue/test-utils";
import DrawerSettingsSite from "~/components/drawer/DrawerSettingsSite";
import EditorSelectHomePage from "~/components/editor/EditorSelectHomePage.vue";
import EditorMainNavigation from "~/components/editor/mainNavigation/EditorMainNavigation.vue";
import EditorFooter from "~/components/editor/footer/EditorFooter.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";
import { getters as siteGetters } from "~/store/site/index.js";

const localVue = createLocalVue();
localVue.use(Vuex);
localVue.use(Vuetify);

let wrapper;
const wrapperFactory = (propsData = {}) => {
	const store = new Vuex.Store({
		modules: {
			interface: {
				namespaced: true,
			},
			library: {
				namespaced: true,
				state: {
					content: [],
				},
			},
			site: {
				namespaced: true,
				getters: siteGetters,
				state: {
					workingSite: {
						footer: {
							disclosures: [],
						},
						pages: [],
					},
				},
			},
		},
	});
	return mount(DrawerSettingsSite, {
		propsData,
		vuetify: new Vuetify(),
		store,
		localVue,
		components: {
			EditorMainNavigation,
			EditorSelectHomePage,
			EditorFooter,
		},
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("DrawerSettingsSite", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});
	test("has all schema fields in the dom", async () => {
		const fields = siteSchema.fields.filter(
			(field) =>
				!field.hidden && ["css", "json"].indexOf(field.type) === -1
		);

		const html = wrapper.html();
		// all labels from schema except should be visible in the site settings drawer
		fields.forEach((field) => {
			expect(html).toContain(field.label);
		});
	});
});
