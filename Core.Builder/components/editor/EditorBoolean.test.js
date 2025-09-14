import { mount } from "@vue/test-utils";
import EditorBoolean from "./EditorBoolean.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(EditorBoolean, {
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

describe("EditorBoolean", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("should not toggle if disabled is true", async () => {
		wrapper.setProps({ value: true, disabled: true });
		await wrapper.vm.$nextTick();
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("should not toggle if readonly is true", async () => {
		wrapper.setProps({ value: true, readonly: true });
		await wrapper.vm.$nextTick();
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("default value should be false", () => {
		wrapper.setProps({ value: null });
		expect(wrapper.vm.value).toEqual(false);
	});

	test("should only allow boolean value", async () => {
		wrapper.setProps({ value: "666" });
		expect(wrapper.vm.value).toEqual(false);
		await wrapper.vm.$nextTick();
		const input = wrapper.find("input");
		input.trigger("click");
		expect(wrapper.emitted().input[0]).toEqual([false]);
	});

	test("should properly toggle and emit true boolean value", () => {
		expect(wrapper.vm.value).toEqual(false);
		const input = wrapper.find("input");
		input.trigger("click");
		expect(wrapper.emitted().input[0]).toEqual([true]);
	});

	test("should accept a true v-model, toggle, and emit false boolean value", async () => {
		wrapper.setProps({ value: true });
		await wrapper.vm.$nextTick();
		expect(wrapper.vm.value).toEqual(true);
		const input = wrapper.find("input");
		input.trigger("click");
		expect(wrapper.emitted().input[0]).toEqual([false]);
	});
});
