import { createLocalVue, shallowMount } from "@vue/test-utils";
import homePage from "./index.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";
import { getters } from "~/store/user.js";
import ModalFirstTimeUser from "~/components/modal/FirstTimeUser.vue";
import SamplesUserFields from "~/components/samples/UserFields.vue";
import DashboardLayoutFieldUser from "~/components/dashboard/layout/FieldUser.vue";

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
	return shallowMount(homePage, {
		propsData,
		vuetify,
		store,
		localVue,
		sync: false,
		components: {
			ModalFirstTimeUser,
			SamplesUserFields,
			DashboardLayoutFieldUser,
		},
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("Home page", () => {
	test("exists", async () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("user who is not agent skips eligibility prompts", async () => {
		wrapper = wrapperFactory({
			user: {
				displayName,
				employeeType: "ContentLibrarian",
				permissions: [],
			},
		});
		expect(wrapper.html()).not.toContain(
			"<dashboardeligibilitynotauthorized-stub>"
		);
		expect(wrapper.html()).toContain(displayName);
	});

	test("user who is agent and has a site skips eligibility prompts", async () => {
		wrapper = wrapperFactory({
			user: {
				displayName,
				employeeType: "Agent",
				hasWebsiteAgent: true,
				hasWebsiteRecruiter: false,
				permissions: [],
			},
		});
		expect(wrapper.html()).not.toContain(
			"<dashboardeligibilitynotauthorized-stub>"
		);
		expect(wrapper.html()).toContain(displayName);
	});

	test("user who is agent with no sites who CANNOT create a site sees the eligibility prompt", async () => {
		wrapper = wrapperFactory({
			user: {
				displayName,
				employeeType: "Agent",
				hasWebsiteAgent: false,
				hasWebsiteRecruiter: false,
				permissions: [],
			},
		});
		expect(wrapper.html()).toContain(
			"<dashboardeligibilitynotauthorized-stub>"
		);
		expect(wrapper.html()).not.toContain(displayName);
	});

	test("user who is agent with no sites who CAN create a site sees the eligibility prompt", async () => {
		wrapper = wrapperFactory({
			user: {
				displayName,
				employeeType: "Agent",
				hasWebsiteAgent: false,
				hasWebsiteRecruiter: false,
				permissions: ["createsite"],
			},
		});
		expect(wrapper.html()).toContain(
			"<dashboardeligibilitynotauthorized-stub>"
		);
		expect(wrapper.html()).not.toContain(displayName);
	});
});
