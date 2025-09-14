import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
import userEvent from "@testing-library/user-event";
import Vuetify from "vuetify";
import ForgeChipGroup from "./ForgeChipGroup.vue";

const chips = [
	{
		value: "bomb",
		label: "Bomb",
		color: "red",
		textColor: "white",
		icon: {
			name: "mdi-bomb",
			color: "black",
		},
	},
	{
		value: "airplane",
		label: "Airplane",
		color: "green",
		textColor: "white",
		icon: {
			name: "mdi-airplane",
			color: "white",
		},
	},
	{
		value: "anchor",
		label: "Anchor",
		color: "blue",
		textColor: "white",
		icon: {
			name: "mdi-anchor",
			color: "white",
		},
	},
];

const renderComponent = async (props = {}) => {
	return render(ForgeChipGroup, {
		props,
		vuetify: new Vuetify(),
	});
};

describe("ForgeChipGroup", () => {
	test("exists", async () => {
		await renderComponent();
	});

	test("if multiple is true, multiple chips can be active", async () => {
		const user = userEvent.setup();
		const { container, getByText } = await renderComponent({
			chips,
			multiple: true,
		});

		// click a couple chips
		const firstLabel = getByText(chips[0].label);
		const secondLabel = getByText(chips[1].label);
		await user.click(firstLabel);
		await user.click(secondLabel);

		// those chips now have a class of active
		const domChips = container.querySelectorAll("div.forge-chip");
		const firstChip = domChips[0].firstChild;
		expect(firstChip.className).toContain("active");
		const secondChip = domChips[1].firstChild;
		expect(secondChip.className).toContain("active");

		//  but the others do not
		const thirdChip = domChips[2].firstChild;
		expect(thirdChip.className).not.toContain("active");
	});

	test("If multiple is false, only one chip can be active", async () => {
		const user = userEvent.setup();
		const { container, getByText } = await renderComponent({
			chips,
		});

		// click a couple chips
		const firstLabel = getByText(chips[0].label);
		const secondLabel = getByText(chips[1].label);
		await user.click(firstLabel);
		await user.click(secondLabel);

		// first chip clicked is not active
		const domChips = container.querySelectorAll("div.forge-chip");
		const firstChip = domChips[0].firstChild;
		expect(firstChip.className).not.toContain("active");

		// but the second chip clicked is active
		const secondChip = domChips[1].firstChild;
		expect(secondChip.className).toContain("active");

		// not the third
		const thirdChip = domChips[2].firstChild;
		expect(thirdChip.className).not.toContain("active");
	});

	test("If mandatory is true, first chip is active by default", async () => {
		const { container } = await renderComponent({
			chips,
			mandatory: true,
		});

		// only the first chip is active
		const domChips = container.querySelectorAll("div.forge-chip");
		const firstChip = domChips[0].firstChild;
		expect(firstChip.className).toContain("active");

		// but the rest are not
		const secondChip = domChips[1].firstChild;
		expect(secondChip.className).not.toContain("active");
		const thirdChip = domChips[2].firstChild;
		expect(thirdChip.className).not.toContain("active");
	});
});
