import "@testing-library/jest-dom";
import userEvent from "@testing-library/user-event";
import EditorPhone from "~/components/editor/EditorPhone";
import Vuetify from "vuetify";

import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	const root = document.createElement("div");
	root.setAttribute("data-app", "true");
	return render(EditorPhone, {
		container: document.body.appendChild(root),
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			name: "input",
			...propsData,
		},
	});
};

describe("EditorPhone", () => {
	it("exists", () => {
		renderComponent({});
	});
	it("should allow the user to enter a phone number", async () => {
		const user = userEvent.setup();
		const { getByRole, getByDisplayValue } = renderComponent({});
		const phoneInput = getByRole("textbox");
		expect(phoneInput).toBeInTheDocument();
		await user.type(phoneInput, "123-456-7890");
		expect(getByDisplayValue("123-456-7890")).toBeInTheDocument();
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
});
