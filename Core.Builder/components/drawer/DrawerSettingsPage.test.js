import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
import pageSchema from "~/schemas/page.json";

import DrawerSettingsPage from "~/components/drawer/DrawerSettingsPage";
import Vuetify from "vuetify";

const renderComponent = async (props = {}, state = {}) => {
	return render(DrawerSettingsPage, {
		props,
		store: await store(state),
		vuetify: new Vuetify(),
	});
};

describe("DrawerSettingsPage", () => {
	test("exists", async () => {
		await renderComponent();
	});

	test("has all schema fields in the dom with correct type", async () => {
		const { getByText } = await renderComponent();
		const fields = pageSchema.fields.filter(
			(field) =>
				!field.hidden && ["css", "json"].indexOf(field.type) === -1
		);

		// all labels from schema except "Blocks" should be visible in the page settings drawer
		fields.forEach((field) => {
			if (field.key !== "blocks") {
				getByText(field.label);
			}
		});
	});
});
