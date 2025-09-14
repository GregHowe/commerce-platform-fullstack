import "@testing-library/jest-dom";
import userEvent from "@testing-library/user-event";
import EditorTime from "~/components/editor/EditorTime";
import Vuetify from "vuetify";
import { render } from "@testing-library/vue";

const renderComponent = (propsData = {}) => {
	return render(EditorTime, {
		vuetify: new Vuetify(),
		propsData: {
			value: "",
			...propsData,
		},
	});
};

describe("EditorTime", () => {
	it("exists", () => {
		renderComponent({});
	});
	it("time should be in hh:mm:ss format", async () => {
		const { getByRole } = renderComponent({});
		const input = getByRole("time");
		await userEvent.type(input, "0102");
		expect(input.value).toBe("01:02");
	});
});
