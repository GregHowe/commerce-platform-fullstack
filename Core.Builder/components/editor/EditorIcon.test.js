import { mount } from "@vue/test-utils";
import EditorIcon from "./EditorIcon.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(EditorIcon, {
		propsData: {
			...propsData,
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

describe("EditorIcon", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("Active icon should match passed value", async () => {
		await wrapper.setProps({ value: "down-arrow" });
		await wrapper.vm.$nextTick();
		const iconBtn = wrapper.find(".down-arrow");
		expect(iconBtn.classes()).toContain("down-arrow");
	});

	test("If removed button is clicked, emitted value should be null", async () => {
		await wrapper.setProps({ value: "down-arrow" });
		await wrapper.vm.$nextTick();
		const rmvBtn = wrapper.find(".remove-btn");
		await rmvBtn.trigger("click");
		expect(wrapper.emitted().input[0][0]).toBeNull();
	});

	test("Active icon should be highlighted", async () => {
		await wrapper.setProps({ value: "down-arrow" });
		await wrapper.vm.$nextTick();
		const iconBtn = wrapper.find(".down-arrow");
		await iconBtn.trigger("click");
		expect(iconBtn.classes()).toContain("v-item--active");
	});

	test("Clicked icon should emit it's associated value", async () => {
		await wrapper.setProps({ value: "" });
		await wrapper.vm.$nextTick();
		const iconBtn = wrapper.find(".down-arrow");
		await iconBtn.trigger("click");
		expect(wrapper.emitted().input[0][0]).toBe("down-arrow");
	});

	test("Remove button should not be rendered if mandatory prop is true", async () => {
		await wrapper.setProps({ mandatory: true });
		await wrapper.vm.$nextTick();
		const rmvBtn = wrapper.find(".remove-btn");
		expect(rmvBtn.exists()).toBe(false);
	});
});
