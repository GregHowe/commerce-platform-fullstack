import { mount } from "@vue/test-utils";
import ForgeButtonGroup from "./ForgeButtonGroup.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(ForgeButtonGroup, {
		propsData: {
			...propsData,
			buttons: [
				{ id: 1, value: "1", icon: "mdi-plus" },
				{ id: 2, value: "2", icon: "mdi-plus" },
				{ id: 3, value: "3", icon: "mdi-minus" },
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

describe("ForgeButtonGroup", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("If mandatory attribute is on, then the button group's v-model must always have a value", async () => {
		await wrapper.setProps({ mandatory: true, value: "" });
		await wrapper.vm.$nextTick();

		expect(wrapper.emitted().input[0]).toEqual(["1"]);
	});

	test("If multiple attribute is enabled, then the emitted value must be in an array.", async () => {
		await wrapper.setProps({ multiple: true });
		await wrapper.vm.$nextTick();

		const button2 = wrapper.find("[data-test='button2']");
		await button2.trigger("click");
		await button2.vm.$nextTick();

		const button1 = wrapper.find("[data-test='button1']");
		await button1.trigger("click");
		await button1.vm.$nextTick();

		const emittedVals = wrapper.emitted().input[1];

		const dataType = Array.isArray(emittedVals);
		expect(dataType).toBe(true);
	});
});
