import { mount } from "@vue/test-utils";
import EditorInteger from "./EditorInteger.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(EditorInteger, {
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

describe("EditorInteger", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("should not allow input if disabled is true", async () => {
		wrapper.setProps({ value: 666, disabled: true });

		await wrapper.vm.$nextTick();
		const input = wrapper.find("input");
		expect(input.element.getAttribute("disabled")).toBe("disabled");

		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("should not allow input if readonly is true", async () => {
		wrapper.setProps({ value: 666, readonly: true });

		await wrapper.vm.$nextTick();
		const input = wrapper.find("input");
		expect(input.element.getAttribute("readonly")).toBe("readonly");

		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("default value should be 0", () => {
		wrapper.setProps({ value: null });
		expect(wrapper.vm.value).toEqual(0);
	});

	test("should emit an integer if string is passed in", async () => {
		wrapper.setProps({ value: "666" });

		await wrapper.vm.$nextTick();
		const input = wrapper.find("input");
		input.trigger("input");
		expect(wrapper.emitted().input[0]).toEqual([666]);
	});
});
