import "@testing-library/jest-dom";
import userEvent from "@testing-library/user-event";
import EditorDomain from "~/components/editor/EditorDomain";
import Vuetify from "vuetify";

import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	const root = document.createElement("div");
	root.setAttribute("data-app", "true");
	return render(EditorDomain, {
		container: document.body.appendChild(root),
		vuetify: new Vuetify(),
		propsData: {
			value: "placeholder",
			...propsData,
		},
	});
};

describe("EditorDomain", () => {
	it("should exist", () => {
		renderComponent({});
	});
	it("should allow the user to enter a domain", async () => {
		const user = userEvent.setup();
		const { getByRole, getByDisplayValue } = renderComponent({});
		const domainInput = getByRole("textbox");
		expect(domainInput).toBeInTheDocument();
		await user.type(domainInput, "google.com");
		expect(getByDisplayValue("placeholdergoogle.com")).toBeInTheDocument();
	});
	it("should not allow the user to type if it is disabled", async () => {
		const user = userEvent.setup();
		const { getByRole, getByDisplayValue } = renderComponent({
			disabled: true,
		});
		const domainInput = getByRole("textbox");
		await user.type(domainInput, "some input");
		expect(getByDisplayValue("placeholder")).toBeInTheDocument();
	});
	it("should not allow the user to type if it is readonly", async () => {
		const user = userEvent.setup();
		const { getByRole, getByDisplayValue } = renderComponent({
			disabled: true,
		});
		const domainInput = getByRole("textbox");
		await user.type(domainInput, "some input");
		expect(getByDisplayValue("placeholder")).toBeInTheDocument();
	});
	it("should emit its value as the user inputs a domain", async () => {
		const user = userEvent.setup();
		const { emitted, getByRole, getByDisplayValue } = renderComponent({});
		const domainInput = getByRole("textbox");
		await user.type(domainInput, "some input");
		expect(getByDisplayValue("placeholdersome input")).toBeInTheDocument();
		expect(emitted().input[9][0]).toStrictEqual("placeholdersome input");
	});
});
