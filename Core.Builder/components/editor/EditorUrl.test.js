import "@testing-library/jest-dom";
import userEvent from "@testing-library/user-event";
import EditorUrl from "~/components/editor/EditorUrl";
import Vuetify from "vuetify";
import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	return render(EditorUrl, {
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			name: "input",
			...propsData,
		},
	});
};

describe("EditorUrl", () => {
	it("exists", () => {
		renderComponent({});
	});
	it("should allow the user to enter a url", async () => {
		const user = userEvent.setup();
		const { getByRole, getByDisplayValue } = renderComponent({});
		const URLInput = getByRole("textbox");
		expect(URLInput).toBeInTheDocument();
		await user.type(URLInput, "https://werwoeirjwoeirjwo.com");
		expect(
			getByDisplayValue("https://werwoeirjwoeirjwo.com")
		).toBeInTheDocument();
	});
	it("should emit its value after each input event", async () => {
		const user = userEvent.setup();
		const { emitted, getByRole, getByDisplayValue } = renderComponent({});
		const input = getByRole("textbox");
		await user.type(input, "123");
		expect(getByDisplayValue("123")).toBeInTheDocument();
		expect(emitted().input[0][0]).toStrictEqual("1");
		expect(emitted().input[1][0]).toStrictEqual("12");
		expect(emitted().input[2][0]).toStrictEqual("123");
	});
	it("should display the hint that is passed in", () => {
		const { getByText } = renderComponent({ hint: "website.com" });
		expect(getByText("website.com"));
	});
});
