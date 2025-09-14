import "@testing-library/jest-dom";
import userEvent from "@testing-library/user-event";
import EditorEmail from "~/components/editor/EditorEmail";
import Vuetify from "vuetify";

import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	const root = document.createElement("div");
	root.setAttribute("data-app", "true");
	return render(EditorEmail, {
		container: document.body.appendChild(root),
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			...propsData,
		},
	});
};

describe("EditorEmail", () => {
	it("exists", () => {
		renderComponent({});
	});
	it("should allow the user to input an email", async () => {
		const user = userEvent.setup();
		const { getByRole, getByDisplayValue } = renderComponent({});
		const emailInput = getByRole("textbox");
		expect(emailInput).toBeInTheDocument();
		await user.type(emailInput, "google.com");
		expect(getByDisplayValue("google.com")).toBeInTheDocument();
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
