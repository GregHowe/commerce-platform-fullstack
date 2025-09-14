import { createLocalVue, mount } from "@vue/test-utils";
import EditorImage from "./EditorImage.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import { ValidationProvider } from "vee-validate";
import Vuex from "vuex";

const localVue = createLocalVue();
localVue.use(Vuex);
const vuetify = new Vuetify();

const event = {
	target: {
		files: [
			{
				name: "image.png",
				size: 5000000,
				type: "image/png",
			},
		],
	},
};

let url = "https://google.com";

const uploadFile = () => {
	return url;
};

let wrapper;
const wrapperFactory = (propsData = {}) => {
	const store = new Vuex.Store({
		modules: {
			content: {
				namespaced: true,
				actions: { uploadFile },
			},
		},
	});
	return mount(EditorImage, {
		propsData,
		vuetify,
		store,
		localVue,
		components: { ValidationProvider },
		sync: false,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("EditorImage", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("validation fails when maxSize is less than file size", async () => {
		const maxSize = 2000;
		wrapper = wrapperFactory({ rules: { maxSize } });
		// make sure validation does not pass
		const validate = await wrapper.vm.validateImage(event);
		expect(validate.valid).toBe(false);
		// whatever the error message is, make sure it has the maxSize value
		expect(validate.errors[0]).toContain(maxSize.toString());
	});

	test("validates when maxSize is set above the size of the file", async () => {
		wrapper = wrapperFactory({
			rules: { maxSize: event.target.files[0].size + 1 },
		});
		const validation = await wrapper.vm.validateImage(
			event.target.files[0]
		);
		expect(validation.valid).toBe(true);
		expect(validation.errors).toStrictEqual([]);
	});

	test("validation fails when file is an incorrect mime type", async () => {
		wrapper = wrapperFactory({
			rules: { maxSize: event.target.files[0].size + 1 },
		}); // pass maxSize validation
		event.target.files[0].type = "application/pdf"; // force an incorrect type
		// make sure validation does not pass
		const validate = await wrapper.vm.validateImage(event);
		expect(validate.valid).toBe(false);
		// whatever the error message is, make sure it has the word "image"
		expect(validate.errors[0]).toContain("Image");
	});

	test("validates with proper mime type", async () => {
		wrapper = wrapperFactory({
			rules: { maxSize: event.target.files[0].size + 1 },
		}); // pass maxSize validation
		event.target.files[0].type = "image/png"; // correct type
		const validate = await wrapper.vm.validateImage(event.target.files[0]);
		expect(validate.valid).toBe(true);
		expect(validate.errors).toStrictEqual([]);
	});

	test("does not display hint if prop is not passed", async () => {
		const hintWrapper = wrapper.find(".v-messages__message");
		expect(hintWrapper.exists()).toBe(false);
	});

	test("displays passed in hint prop", async () => {
		const hint = "pssssssst hint hint";
		wrapper = wrapperFactory({ hint });
		const hintWrapper = wrapper.find(".v-messages__message");
		expect(hintWrapper.exists()).toBe(true);
		expect(hintWrapper.text()).toBe(hint);
	});

	test("emits a valid url", async () => {
		wrapper = wrapperFactory({ maxSize: event.target.files[0].size + 1 }); // pass maxSize validation
		await wrapper.vm.updateImage(event);
		expect(wrapper.emitted().input[0]).toContain(url);
	});

	test("does not emit an invalid url", async () => {
		wrapper = wrapperFactory({ maxSize: event.target.files[0].size + 1 }); // pass maxSize validation
		url = "googlyboogly";
		await wrapper.vm.updateImage(event);
		expect(wrapper.emitted().input).toBeFalsy();
	});
});
