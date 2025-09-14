import { mount } from "@vue/test-utils";
import EditorSlider from "./EditorSlider.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

const prependClick = jest.fn();
const appendClick = jest.fn();

let wrapper;

const wrapperFactory = (propsData = {}) => {
	return mount(EditorSlider, {
		propsData: {
			...propsData,
			prependClick,
			appendClick,
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

describe("EditorSlider", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();

		const slider = wrapper.find(".slider");
		expect(slider.exists()).toBe(true);
	});

	test("should always initialize to the min value", async () => {
		wrapper.setProps({ value: 0, min: 5, max: 10 });
		await wrapper.vm.$nextTick();
		expect(wrapper.emitted().input[0]).toStrictEqual([5]);
	});

	test("when prepend icon is set we have the icon and the correct method is called when clicked", async () => {
		wrapper.setProps({ "prepend-icon": "test" });
		await wrapper.vm.$nextTick();
		const prependIconButton = wrapper.find(
			".v-input__icon--prepend > button"
		);
		expect(prependIconButton.exists()).toBe(true);

		prependIconButton.trigger("click");
		await wrapper.vm.$nextTick();
		expect(prependClick).toHaveBeenCalled();
		expect(appendClick).not.toHaveBeenCalled();
	});

	test("when append icon is set we have the icon and the correct method is called when clicked", async () => {
		wrapper.setProps({ "append-icon": "test" });
		await wrapper.vm.$nextTick();
		const appendIconButton = wrapper.find(
			".v-input__icon--append > button"
		);
		expect(appendIconButton.exists()).toBe(true);

		appendIconButton.trigger("click");
		await wrapper.vm.$nextTick();
		expect(appendClick).toHaveBeenCalled();
	});
});
