import "@testing-library/jest-dom";
import userEvent from "@testing-library/user-event";
import EditorAddress from "~/components/editor/EditorAddress";
import Vuetify from "vuetify";

import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	const root = document.createElement("div");
	root.setAttribute("data-app", "true");
	return render(EditorAddress, {
		container: document.body.appendChild(root),
		vuetify: new Vuetify(),
		propsData: {
			value: {
				line1: "",
				line2: "",
				city: "",
				state: "",
				zip: "",
			},
			...propsData,
		},
	});
};

describe("EditorAddress", () => {
	it("should render", () => {
		renderComponent({});
	});
	it("should allow the user to enter a street address", async () => {
		const user = userEvent.setup();
		const { getByLabelText, getByDisplayValue } = renderComponent({});
		const streetAddressInput = getByLabelText("Street Address");
		expect(streetAddressInput).toBeInTheDocument();
		await user.type(streetAddressInput, "1002 N Street");
		expect(getByDisplayValue("1002 N Street")).toBeInTheDocument();
	});
	it("should allow the user to enter an apartment/suite number", async () => {
		const user = userEvent.setup();
		const { getByLabelText, getByDisplayValue } = renderComponent({});
		const apartmentInput = getByLabelText("Apartment / Suite Number");
		expect(apartmentInput).toBeInTheDocument();
		await user.type(apartmentInput, "Apt 14");
		expect(getByDisplayValue("Apt 14")).toBeInTheDocument();
	});
	it("should allow the user to enter a city", async () => {
		const user = userEvent.setup();
		const { getByLabelText, getByDisplayValue } = renderComponent({});
		const cityInput = getByLabelText("City");
		expect(cityInput).toBeInTheDocument();
		await user.type(cityInput, "Ogdenville");
		expect(getByDisplayValue("Ogdenville")).toBeInTheDocument();
	});
	it("should allow the user to enter a zip code", async () => {
		const user = userEvent.setup();
		const { getByLabelText, getByDisplayValue } = renderComponent({});
		const zipCodeInput = getByLabelText("Zip Code");
		expect(zipCodeInput).toBeInTheDocument();
		await user.type(zipCodeInput, "12345");
		expect(getByDisplayValue("12345")).toBeInTheDocument();
	});
	it("should emit the entire address as the user inputs the street address", async () => {
		const user = userEvent.setup();
		const { emitted, getByLabelText, getByDisplayValue } = renderComponent(
			{}
		);
		const streetAddressInput = getByLabelText("Street Address");
		expect(streetAddressInput).toBeInTheDocument();
		await user.type(streetAddressInput, "1");
		expect(getByDisplayValue("1")).toBeInTheDocument();
		expect(emitted().input[0][0]).toEqual({
			line1: "1",
			line2: "",
			city: "",
			state: "",
			zip: "",
		});
	});
	it("should emit the entire address as the user inputs the apartment / suite number", async () => {
		const user = userEvent.setup();
		const { emitted, getByLabelText, getByDisplayValue } = renderComponent(
			{}
		);
		const apartmentInput = getByLabelText("Apartment / Suite Number");
		expect(apartmentInput).toBeInTheDocument();
		await user.type(apartmentInput, "1");
		expect(getByDisplayValue("1")).toBeInTheDocument();
		expect(emitted().input[0][0]).toEqual({
			line1: "",
			line2: "1",
			city: "",
			state: "",
			zip: "",
		});
	});
	it("should emit the entire address as the user inputs the city", async () => {
		const user = userEvent.setup();
		const { emitted, getByLabelText, getByDisplayValue } = renderComponent(
			{}
		);
		const cityInput = getByLabelText("City");
		expect(cityInput).toBeInTheDocument();
		await user.type(cityInput, "1");
		expect(getByDisplayValue("1")).toBeInTheDocument();
		expect(emitted().input[0][0]).toEqual({
			line1: "",
			line2: "",
			city: "1",
			state: "",
			zip: "",
		});
	});
	it.todo("should emit the entire address as the user selects a state");
	it("should emit the entire address as the user inputs the zip code", async () => {
		const user = userEvent.setup();
		const { emitted, getByLabelText, getByDisplayValue } = renderComponent(
			{}
		);
		const zipCodeInput = getByLabelText("Zip Code");
		expect(zipCodeInput).toBeInTheDocument();
		await user.type(zipCodeInput, "1");
		expect(getByDisplayValue("1")).toBeInTheDocument();
		expect(emitted().input[0][0]).toEqual({
			line1: "",
			line2: "",
			city: "",
			state: "",
			zip: "1",
		});
	});
	it.todo(
		"should display an error message if the zip code fails validation, after the user clicks outside the input"
	);
});
