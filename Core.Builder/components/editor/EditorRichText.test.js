import { shallowMount } from "@vue/test-utils";
import EditorRichText from "./EditorRichText.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";

const vuetify = new Vuetify();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return shallowMount(EditorRichText, {
		propsData: { ...propsData },
		vuetify,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("EditorRichText", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("should output HTML markup", () => {
		wrapper.vm.$data.editor.chain().insertContent("something").run();
		expect(wrapper.emitted().input[0]).toEqual(["<p>something</p>"]);
	});

	test("if a label is passed, the editor should have a label", async () => {
		wrapper.setProps({ label: "Label" });
		await wrapper.vm.$nextTick();
		const label = wrapper.find("label");
		expect(label.exists()).toBeTruthy();
		expect(label.text()).toBe("Label");
	});
});
