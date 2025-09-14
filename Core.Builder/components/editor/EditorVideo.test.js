import { createLocalVue, shallowMount } from "@vue/test-utils";
import EditorVideo from "./EditorVideo.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";
import { getters } from "~/store/validation";

const localVue = createLocalVue();
localVue.use(Vuex);

const state = {
	validation: {},
};

let wrapper;
const wrapperFactory = (propsData = {}) => {
	const store = new Vuex.Store({
		modules: {
			validation: {
				namespaced: true,
				state,
				getters,
			},
		},
	});
	return shallowMount(EditorVideo, {
		propsData,
		vuetify: new Vuetify(),
		store,
		localVue,
	});
};

beforeEach(() => {
	wrapper = wrapperFactory();
});

afterEach(() => {
	wrapper.destroy();
});

describe("EditorVideo", () => {
	test("exists", () => {
		expect(wrapper.exists()).toBeTruthy();
	});

	test("if youtube video is supplied, button contains youtube icon", async () => {
		wrapper.setProps({ value: "https://www.youtube.com/embed/asdfkjh" });
		await localVue.nextTick();
		const stub = wrapper.find("v-icon-stub");
		expect(stub.text()).toBe("mdi-youtube");
	});

	test("if vimeo video is supplied, button contains vimeo icon", async () => {
		wrapper.setProps({ value: "https://www.player.vimeo.com/asdfkjh" });
		await localVue.nextTick();
		const stub = wrapper.find("v-icon-stub");
		expect(stub.text()).toBe("mdi-vimeo");
	});

	test("if brightcove video is supplied, button contains brightcove icon", async () => {
		wrapper.setProps({
			value: "https://www.players.brightcove.com/asdfkjh",
		});
		await localVue.nextTick();
		const stub = wrapper.find("v-icon-stub");
		expect(stub.text()).toBe("mdi-video");
	});

	test("if unmatched video is supplied, button does not contain icon", async () => {
		wrapper.setProps({ value: "https://www.other-video.com/asdfkjh" });
		await localVue.nextTick();
		const stub = wrapper.find("v-icon-stub");
		expect(stub.exists()).toBeFalsy();
	});
});
