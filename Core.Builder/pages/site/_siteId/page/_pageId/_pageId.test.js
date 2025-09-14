import { mount, createLocalVue } from "@vue/test-utils";
import _pageId from "./index.vue";
import ForgeBanner from "@/components/forge/ForgeBanner.vue";
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
				getters: { hasWorkingChanges: jest.fn() },
				namespaced: true,
			},
			user: {
				namespaced: true,
				getters,
			},
			auth: {
				state: {
					user: { employeeType: "Agent", ...user },
				},
				namespaced: true,
			},
		},
	});
	return mount(_pageId, {
		stubs: {
			NuxtChild: true,
			CoreBlockMainNav: true,
			CoreBlockFooter: true,
			CoreBlock: true,
			ModalSiteBuildInProgress: true,
		},
		mocks: { $route },
		components: {
			ForgeBanner,
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

describe("_pageId.vue", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("main navigation exists in the builder preview", () => {
		expect(wrapper.html()).toContain("<coreblockmainnav");
	});

	test("footer exists in the builder preview", () => {
		expect(wrapper.html()).toContain("<coreblockfooter");
	});
});
