import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
import userEvent from "@testing-library/user-event";
import Vuetify from "vuetify";
import ForgeTooltip from "./ForgeTooltip.vue";

const triggerText = "click here";
const tooltipText = "this should show as tooltip text";

const renderComponent = async (props = {}, slots = {}) => {
	return render(ForgeTooltip, {
		props,
		slots,
		vuetify: new Vuetify(),
	});
};

describe("ForgeTooltip", () => {
	test("exists", async () => {
		await renderComponent();
	});

	test("tooltip defaults", async () => {
		const { queryByText, getByText } = await renderComponent(
			{},
			{
				trigger: `<div>${triggerText}</div>`,
			}
		);

		// trigger text is in dom
		getByText(triggerText);
		// but the tooltip itself is hidden
		const isTooltipShowing = queryByText(tooltipText);
		expect(isTooltipShowing).toBeFalsy();
	});

	test("tooltip content shows on hover", async () => {
		// delay to account for any animation on hover
		const user = userEvent.setup({ delay: 400 });
		const { getByText, queryByText } = await renderComponent(
			{},
			{
				trigger: `<div>${triggerText}</div>`,
				default: tooltipText,
			}
		);

		// tooltip content is not in dom
		expect(queryByText(tooltipText)).toBeFalsy();

		// hover trigger
		const trigger = getByText(triggerText);
		await user.hover(trigger);

		// tooltip content is in dom after hover
		expect(queryByText(tooltipText)).toBeTruthy();
	});
});
