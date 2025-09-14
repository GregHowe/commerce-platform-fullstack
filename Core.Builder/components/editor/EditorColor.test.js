import { mount } from "@vue/test-utils";
import EditorColor from "./EditorColor.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(EditorColor, {
		propsData,
		vuetify,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("EditorColor", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("default value should be null", () => {
		expect(wrapper.vm.value).toBe(null);
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("should not allow input if readonly is true", async () => {
		wrapper.setProps({ value: "#ffffff", readonly: true });

		await wrapper.vm.$nextTick();
		const input = wrapper.find(".v-color-picker");
		expect(input.element.getAttribute("readonly")).toBe("readonly");
		// component does not emit
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("swatches attribute should override default swatches", async () => {
		wrapper.setProps({ swatches: [["#12345678"]] });
		await wrapper.vm.$nextTick();
		const result = wrapper.vm.attrsOverride.swatches[0][0] === "#12345678";
		expect(result).toBeTruthy();
	});

	test("should only allow valid color value", () => {
		wrapper.setProps({ value: "20" });
		expect(wrapper.emitted().input).toBeFalsy();
	});
});
