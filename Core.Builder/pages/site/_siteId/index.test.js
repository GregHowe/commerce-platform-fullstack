import { mount, createLocalVue } from "@vue/test-utils";
import _siteId from "./index.vue";
import Vuex from "vuex";
// import '@testing-library/jest-dom'

const localVue = createLocalVue();
localVue.use(Vuex);

let wrapper;
const actions = {
	selectSite: jest.fn(),
	loadThemeList: jest.fn(),
};

const wrapperFactory = ($route = { params: { siteId: null } }) => {
	const store = new Vuex.Store({
		modules: {
			site: {
				actions,
				namespaced: true,
			},
		},
	});
	return mount(_siteId, {
		stubs: {
			NuxtChild: true,
		},
		mocks: { $route },
		localVue,
		store,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("site page", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	// tests removed because middleware now handles site and page loading
	// test("site is not fetched if siteId not present", () => {
	// 	expect(actions.selectSite).not.toHaveBeenCalled();
	// });

	// test("site is fetched on mount with siteId", () => {
	// 	wrapper = wrapperFactory({ params: { siteId: "test" } });
	// 	expect(actions.selectSite).toHaveBeenCalled();
	// });
});
