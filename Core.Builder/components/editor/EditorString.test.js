import EditorString from "~/components/editor/EditorString";
import userEvent from "@testing-library/user-event";
import { render } from "@testing-library/vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import { mount } from "@vue/test-utils";
import { ValidationProvider } from "vee-validate";

const renderComponent = (propsData = {}) => {
	return render(EditorString, {
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			name: "input",
			...propsData,
		},
	});
};

describe("EditorString", () => {
	it("should exist", () => {
		renderComponent({});
	});
	it("should not allow input if it is disabled", async () => {
		const user = userEvent.setup();
		const { getByRole, queryByDisplayValue } = renderComponent({
			disabled: true,
		});
		const input = getByRole("textbox");
		await user.type(input, "a random string");
		expect(queryByDisplayValue("a random string")).not.toBeInTheDocument();
	});
	it("should not allow input if it is readonly", async () => {
		const user = userEvent.setup();
		const { getByRole, queryByDisplayValue } = renderComponent({
			readonly: true,
		});
		const input = getByRole("textbox");
		await user.type(input, "a random string");
		expect(queryByDisplayValue("a random string")).not.toBeInTheDocument();
	});
	it("should emit its value as the user types", async () => {
		const user = userEvent.setup();
		const { emitted, getByRole, getByDisplayValue } = renderComponent({});
		const input = getByRole("textbox");
		await user.type(input, "123");
		expect(getByDisplayValue("123")).toBeInTheDocument();
		expect(emitted().input[0][0]).toStrictEqual("1");
		expect(emitted().input[1][0]).toStrictEqual("12");
		expect(emitted().input[2][0]).toStrictEqual("123");
	});
	it("should not emit its value as the user types if it is disabled", async () => {
		const user = userEvent.setup();
		const { emitted, getByRole, queryByDisplayValue } = renderComponent({
			disabled: true,
		});
		const input = getByRole("textbox");
		await user.type(input, "123");
		expect(queryByDisplayValue("123")).not.toBeInTheDocument();
		expect(emitted().input).toBeFalsy();
	});
	it("should not emit its value as the user types if it is readonly", async () => {
		const user = userEvent.setup();
		const { emitted, getByRole, queryByDisplayValue } = renderComponent({
			readonly: true,
		});
		const input = getByRole("textbox");
		await user.type(input, "123");
		expect(queryByDisplayValue("123")).not.toBeInTheDocument();
		expect(emitted().input).toBeFalsy();
	});
});

// unable to generate errors from validation using testing-library so for this suite we'll fire up a test-utils wrapper
describe("EditorString validation", () => {
	let wrapper;
	const wrapperFactory = (propsData = {}) => {
		return mount(EditorString, {
			propsData,
			components: { ValidationProvider },
		});
	};

	it("should display an error message if the input fails validation on max length", async () => {
		const providerName = "String Field";
		const max = "2";
		wrapper = wrapperFactory({
			rules: { max },
			providerName,
		});
		const validation = await wrapper.vm.$refs.provider.validate("1234");
		expect(validation.valid).toBe(false);
		expect(validation.errors[0]).toContain(providerName);
		expect(validation.errors[0]).toContain(max);
	});

	it("should pass max length validation", async () => {
		const providerName = "String Field";
		const max = "20";
		wrapper = wrapperFactory({
			rules: { max },
			providerName,
		});
		const validation = await wrapper.vm.$refs.provider.validate("1234");
		expect(validation.valid).toBe(true);
		expect(validation.errors).toEqual([]);
	});

	it("url validation", async () => {
		const providerName = "URL Field";
		wrapper = wrapperFactory({
			rules: { url: true },
			providerName,
		});
		let validation = await wrapper.vm.$refs.provider.validate("1234");
		expect(validation.valid).toBe(false);
		expect(validation.errors[0]).toContain(providerName);
		expect(validation.errors[0]).toContain("url");

		validation = await wrapper.vm.$refs.provider.validate(
			"https://url.com"
		);
		expect(validation.valid).toBe(true);
		expect(validation.errors).toEqual([]);
	});

	it("page slug validation", async () => {
		const providerName = "Page Slug";
		wrapper = wrapperFactory({
			rules: { alpha_dash: true, lowercase: true },
			providerName,
		});
		let validation = await wrapper.vm.$refs.provider.validate(
			"lower_with-dash_and-numbers1234"
		);
		expect(validation.valid).toBe(true);
		expect(validation.errors).toEqual([]);

		// a single uppercase letter
		validation = await wrapper.vm.$refs.provider.validate(
			"lower_with-dash_and-nuMbers1234"
		);
		expect(validation.valid).toBe(false);
		expect(validation.errors[0]).toContain(providerName);
		expect(validation.errors[0]).toContain("lowercase");

		// a single unallowed character
		validation = await wrapper.vm.$refs.provider.validate(
			"lower_with-dash_and-nu@bers1234"
		);
		expect(validation.valid).toBe(false);
		expect(validation.errors[0]).toContain(providerName);
		expect(validation.errors[0]).toContain("characters");
	});
});
