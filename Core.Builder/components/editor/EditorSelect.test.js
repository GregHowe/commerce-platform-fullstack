import { mount } from "@vue/test-utils";
import EditorSelect from "./EditorSelect.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(EditorSelect, {
		propsData: {
			...propsData,
			items: [
				{
					value: "item1",
					text: "Item #1",
				},
				{
					value: "item2",
					text: "Item #2",
				},
				{
					value: "item3",
					text: "Item #3",
				},
			],
		},
		vuetify,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("EditorSelect", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("default value should be null", () => {
		expect(wrapper.vm.value).toBe(null);
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("should not allow input if disabled is true", async () => {
		wrapper.setProps({
			value: "something",
			disabled: true,
		});
		await wrapper.vm.$nextTick();
		const input = wrapper.find("input");
		expect(input.element.getAttribute("disabled")).toBe("disabled");
		// component does not emit
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("should not allow input if readonly is true", async () => {
		wrapper.setProps({
			value: "something",
			readonly: true,
		});
		await wrapper.vm.$nextTick();
		const input = wrapper.find("input");
		expect(input.element.getAttribute("readonly")).toBe("readonly");
		// component does not emit
		expect(wrapper.emitted().input).toBeFalsy();
	});

	test("sets v-model value as selected item", async () => {
		const value = "item1";
		wrapper.setProps({ value });

		await wrapper.vm.$nextTick();
		expect(wrapper.find(".v-select").props("value")).toBe(value);
		expect(wrapper.find(".v-select__selection").text()).toEqual("Item #1");
	});

	test("should not allow invalid option value", async () => {
		const value = "booboobooboo";
		wrapper.setProps({ value });

		await wrapper.vm.$nextTick();
		expect(wrapper.emitted().input).toBeFalsy();
	});
});
