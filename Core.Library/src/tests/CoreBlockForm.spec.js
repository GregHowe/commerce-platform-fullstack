import CoreBlockForm from "../components/block/CoreBlockForm";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { TheMask } from "vue-the-mask";
import "@testing-library/jest-dom";
import { mount, createLocalVue } from "@vue/test-utils";

const localVue = createLocalVue();

let wrapper;
const wrapperFactory = (propsData = {}) => {
	return mount(CoreBlockForm, {
		propsData: {
			dataSite: {
				id: 12,
			},
			...propsData,
		},
		components: { ValidationProvider, ValidationObserver, TheMask },
		mocks: {
			$recaptcha: {
				init: jest.fn(),
			},
		},
		localVue,
	});
};

describe("CoreBlockForm", () => {
	test("if form type is goRecruiter, date of birth field is not present", async () => {
		wrapper = wrapperFactory({
			settings: { formType: "goRecruiter" },
		});

		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if newsletter value is set, only name fields and email field are present", async () => {
		wrapper = wrapperFactory({
			settings: { formType: "agentNewsletter" },
		});
		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if form type is agentCustom, only first name, last name, phone, and email should be present", async () => {
		wrapper = wrapperFactory({
			settings: { formType: "agentCustom" },
		});
		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if language value is set, language preference field is present", async () => {
		wrapper = wrapperFactory({
			settings: { languageFieldset: true },
		});

		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if interests value is set, interests checkbox group is present", async () => {
		wrapper = wrapperFactory({
			settings: { interestesFieldset: true },
		});

		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if linkedIn value is set, LinkedIn field is present", async () => {
		wrapper = wrapperFactory({
			settings: { linkedinFieldset: true },
		});
		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if best time to call value is set, best time to call dropdown is present", async () => {
		wrapper = wrapperFactory({
			settings: { bestTimeToCallFieldset: true },
		});
		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if form submission fails, fail div is present", async () => {
		wrapper = wrapperFactory({
			settings: {},
		});
		await wrapper.setData({ status: "fail" });

		expect(wrapper.html()).toMatchSnapshot();
	});

	test("if form submission succeeds, success div is present", async () => {
		wrapper = wrapperFactory({
			settings: {},
		});
		await wrapper.setData({ status: "success" });

		expect(wrapper.html()).toMatchSnapshot();
	});
});
