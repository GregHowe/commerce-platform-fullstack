import { createLocalVue, mount } from "@vue/test-utils";
import SiteSaver from "./SiteSaver/index.vue";
import GlobalAuthWrapper from "@/components/global/AuthWrapper.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";
import { getters as validationGetters } from "~/store/validation.js";
import { getters as userGetters } from "~/store/user.js";
import {
	mutations as interfaceMutations,
	getters as interfaceGetters,
} from "~/store/interface.js";
import {
	mutations as contentMutations,
	getters as contentGetters,
} from "~/store/site/index.js";

const localVue = createLocalVue();
localVue.use(Vuex);
const vuetify = new Vuetify();

const saveWorkingSite = jest.fn();

const interfaceState = {
	busy: {
		loading: [],
		saving: [],
	},
	validation: {},
};

const validationState = {
	validation: {},
};

const contentState = {
	workingSite: {},
};

let wrapper;
const wrapperFactory = (propsData = {}) => {
	const store = new Vuex.Store({
		modules: {
			auth: {
				namespaced: true,
				state: {
					user: {
						employeeType: "Agent",
					},
				},
			},
			user: {
				namespaced: true,
				getters: userGetters,
			},
			site: {
				namespaced: true,
				state: contentState,
				getters: contentGetters,
				mutations: contentMutations,
				actions: {
					saveWorkingSite,
				},
			},
			interface: {
				namespaced: true,
				state: interfaceState,
				getters: interfaceGetters,
				mutations: interfaceMutations,
			},
			validation: {
				namespaced: true,
				state: validationState,
				getters: validationGetters,
			},
		},
	});
	return mount(SiteSaver, {
		propsData,
		vuetify,
		store,
		localVue,
		sync: false,
		components: { GlobalAuthWrapper },
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
	jest.clearAllMocks();
});

describe("SiteSaver", () => {
	test("exists", async () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("workingSite is saved", async () => {
		wrapper.vm.save();
		expect(saveWorkingSite).toBeCalledTimes(1);
	});

	test("workingSite is not saved if there are validation errors", async () => {
		const failedSEOMessage = "SEO Description cannot be empty";
		const failedTitleMessage = "Title cannot be empty";
		validationState.validation = {
			"SEO Description": {
				failed: true,
				failedRules: {
					required: failedSEOMessage,
				},
			},
			Title: {
				failed: true,
				failedRules: {
					required: failedTitleMessage,
				},
			},
		};
		wrapper.vm.save();
		expect(saveWorkingSite).not.toHaveBeenCalled();
		expect(wrapper.vm.validationFailedMessages.toString()).toBe(
			`${failedSEOMessage},${failedTitleMessage}`
		);
	});
});
