import { mount, createLocalVue } from "@vue/test-utils";
import ForgeBanner from "./ForgeBanner.vue";
import GlobalAuthWrapper from "@/components/global/AuthWrapper.vue";
import SiteSaver from "@/components/SiteSaver/index.vue";
import Vuex from "vuex";
import { getters } from "~/store/user";
import Vuetify from "vuetify";

const localVue = createLocalVue();
localVue.use(Vuex);

let wrapper;
const state = {
	workingSite: {
		pages: [],
	},
	workingPage: {
		blocks: [{ id: "qasdasd" }],
	},
	themeList: [],
	highlightedBlockIds: [],
	selectedBlockIds: [],
};

const wrapperFactory = (user = {}, $route = { params: { siteId: null } }) => {
	const store = new Vuex.Store({
		modules: {
			site: {
				state,
				getters: {
					hasWorkingChanges: jest.fn(),
					workingAzureURL: jest.fn(),
				},
				namespaced: true,
			},
			user: {
				namespaced: true,
				getters,
			},
			auth: {
				state: {
					user: {
						employeeType: "Agent",
						...user,
					},
				},
				namespaced: true,
			},
		},
	});
	return mount(ForgeBanner, {
		propsData: {
			activeView: "desktop",
		},
		stubs: {
			ModalSiteBuildInProgress: true,
		},
		mocks: { $route },
		components: {
			GlobalAuthWrapper,
			SiteSaver,
		},
		localVue,
		store,
		vuetify: new Vuetify(),
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("ForgeBanner.vue", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});
	// Compliance button no longer exists here
	/*
	test("Submit to Compliance button is shown if role is Agent", () => {
		const button = wrapper.find("button"); // right now the first button in the dom is compliance
		expect(button.exists()).toBeTruthy();
		expect(button.text()).toBe("Submit to Compliance");
		expect(button.element.disabled).toBe(true); // it's disabled as there are no changes to submit
	});

	test("Submit to Compliance button is hidden if role is not Agent", () => {
		wrapper = wrapperFactory({ employeeType: "Somethin else" });
		// if theres a button on the page it is not the compliance button
		const button = wrapper.find("button");
		expect(button.text()).not.toBe("Submit to Compliance");
		expect(wrapper.html()).not.toContain("Submit to Compliance");
	});
	*/
});
