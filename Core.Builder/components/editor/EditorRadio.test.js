import { mount } from "@vue/test-utils";
import EditorRadio from "./EditorRadio.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();
const radioData = [
	{
		value: "item1",
		label: "Item #1",
	},
	{
		value: "item2",
		label: "Item #2",
	},
	{
		value: "item3",
		label: "Item #3",
	},
];

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(EditorRadio, {
		propsData: {
			...propsData,
			items: radioData,
			mandatory: true,
			multiple: false,
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

describe("EditorRadio", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("Each button should have a value and label", async () => {
		wrapper.setProps({ value: "item1" });
		await wrapper.vm.$nextTick();
		expect(wrapper.emitted().input[0]).toStrictEqual(["item1"]);
	});
});
