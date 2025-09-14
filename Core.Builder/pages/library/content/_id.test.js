import { mount, createLocalVue } from "@vue/test-utils";
import _id from "./_id.vue";
import ContentLibraryForm from "~/components/contentLibrary/ContentLibraryForm";
import ContentLibraryHeader from "~/components/contentLibrary/ContentLibraryHeader";
import schema from "~/schemas/library.json";
import Vuex from "vuex";
import Vuetify from "vuetify";
import { getters } from "~/store/library";

const localVue = createLocalVue();
localVue.use(Vuex);

const state = {
	content: [
		{ id: 1, type: "video" },
		{ id: 2, type: "image" },
	],
	categories: [
		{ id: 1, value: "category1", label: "Category One" },
		{ id: 2, value: "category2", label: "Category Two" },
	],
};

let wrapper;

const wrapperFactory = ($route = { params: { id: 1 } }) => {
	const store = new Vuex.Store({
		modules: {
			library: {
				state,
				getters,
				namespaced: true,
			},
		},
	});
	return mount(_id, {
		mocks: { $route },
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

describe("Library Content By Id", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("page includes header component", () => {
		expect(wrapper.findComponent(ContentLibraryHeader).exists()).toBe(true);
	});

	test("page includes form component", () => {
		expect(wrapper.findComponent(ContentLibraryForm).exists()).toBe(true);
	});

	test("has an asset type matching the type saved to the content item", async () => {
		expect(wrapper.vm.assetType).toBe("video");
		wrapper = wrapperFactory({ params: { id: 2 } });
		expect(wrapper.vm.assetType).toBe("image");
	});

	test("form is prefilled with content based on schema", () => {
		const fields = schema.types[wrapper.vm.assetType].fields;
		fields.forEach((field) => {
			expect(wrapper.html()).toContain(field.label);
		});
	});
});
