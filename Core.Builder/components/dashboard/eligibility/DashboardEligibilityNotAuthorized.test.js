import { createLocalVue, mount } from "@vue/test-utils";
import DashboardEligibilityNotAuthorized from "./DashboardEligibilityNotAuthorized.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";
import { getters } from "~/store/user.js";

const localVue = createLocalVue();
localVue.use(Vuex);
const vuetify = new Vuetify();

let wrapper;

const displayName = "Johnny John";
const wrapperFactory = (state = {}, propsData = {}) => {
	const store = new Vuex.Store({
		modules: {
			auth: {
				namespaced: true,
				state,
			},
			user: {
				namespaced: true,
				getters,
			},
		},
	});
	return mount(DashboardEligibilityNotAuthorized, {
		propsData,
		vuetify,
		store,
		localVue,
		sync: false,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("DashboardEligibilityNotAuthorized", () => {
	test("exists", async () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("shows not authorized screen without createsite permission", async () => {
		wrapper = wrapperFactory({
			user: {
				displayName,
				employeeType: "Agent",
				permissions: [],
			},
		});
		expect(wrapper.html()).toContain("Not authorized");
		expect(wrapper.html()).toContain(
			"You are not yet eligible to sign in."
		);
		expect(wrapper.html()).not.toContain("Welcome");
		expect(wrapper.html()).not.toContain(
			"You're only a couple steps from your new website."
		);
	});

	test("shows welcome screen with createsite permission", async () => {
		wrapper = wrapperFactory({
			user: {
				displayName,
				employeeType: "Agent",
				permissions: ["createsite"],
			},
		});
		expect(wrapper.html()).not.toContain("Not authorized");
		expect(wrapper.html()).not.toContain(
			"You are not yet eligible to sign in."
		);
		expect(wrapper.html()).toContain("Welcome");
		expect(wrapper.html()).toContain(
			"You're only a couple steps from your new website."
		);
	});
});
