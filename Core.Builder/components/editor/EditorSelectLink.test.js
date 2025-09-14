import { mount, createLocalVue } from "@vue/test-utils";
import EditorSelectLink from "./EditorSelectLink.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";

const localVue = createLocalVue();
localVue.use(Vuex);

const pages = [
	{ id: 1, handle: "ceramic" },
	{ id: 2, handle: "wood" },
	{ id: 3, handle: "parent" },
	{ id: 4, handle: "child", parentPageId: 3 },
];

let wrapper;
const wrapperFactory = (propsData = {}) => {
	const store = new Vuex.Store({
		modules: {
			site: {
				namespaced: true,
				state: {
					workingSite: {
						pages,
					},
				},
			},
		},
	});
	return mount(EditorSelectLink, {
		propsData: {
			...propsData,
		},
		vuetify: new Vuetify(),
		store,
		localVue,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("EditorSelectLink", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("relative link has stripped slashes, and link type is set to Page", async () => {
		await wrapper.setProps({ value: `/${pages[0].handle}` });
		// no slashy
		expect(wrapper.vm.localizedValue).not.toBe(`/${pages[0].handle}`);
		expect(wrapper.vm.localizedValue).toBe(pages[0].handle);
		// link type should automatically be set to page since this is a relative link
		expect(wrapper.vm.linkType).toBe("Page");
		expect(wrapper.html()).toContain("Page");
		expect(wrapper.html()).not.toContain("External URL");
	});

	test("logic for child page with its parent in the url structure", async () => {
		const childPage = pages.find((page) => page.parentPageId);
		const parentPage = pages.find(
			(page) => page.id === childPage.parentPageId
		);
		await wrapper.setProps({
			value: `/${parentPage.handle}/${childPage.handle}`,
		});
		// child page value with no slashy
		expect(wrapper.vm.localizedValue).not.toBe(`/${childPage.handle}`);
		expect(wrapper.vm.localizedValue).toBe(childPage.handle);
		// not the parent page handle value
		expect(wrapper.vm.localizedValue).not.toBe(parentPage.handle);
		// link type should automatically be set to page since this is a relative link
		expect(wrapper.vm.linkType).toBe("Page");
		expect(wrapper.html()).toContain("Page");
		expect(wrapper.html()).not.toContain("External URL");
	});

	test("external link does not have stripped slashes, and link type is set to External URL", async () => {
		const value = "https://www.blah.blah";
		await wrapper.setProps({ value });
		expect(wrapper.vm.localizedValue).toBe(value);
		// link type should automatically be set to external url
		expect(wrapper.vm.linkType).toBe("External URL");
		expect(wrapper.html()).not.toContain("Page");
		expect(wrapper.html()).toContain("External URL");
	});
});
