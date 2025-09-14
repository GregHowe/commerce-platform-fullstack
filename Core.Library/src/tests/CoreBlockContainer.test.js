import CoreBlockContainer from "../components/block/container/CoreBlockContainer.vue";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
import userEvent from "@testing-library/user-event";

const emptyBlockText = "[ Choose Block Type ]";

const renderComponent = async (settings = {}) => {
	return render(CoreBlockContainer, {
		props: {
			settings: {
				...settings,
			},
		},
	});
};

describe("CoreBlockContainer", () => {
	test("exists", async () => {
		await renderComponent();
	});

	test("displays accordion when hasAccordion is true", async () => {
		const { container } = await renderComponent({
			type: "accordion",
			hasAccordion: true,
		});
		expect(container).toMatchSnapshot();
	});

	test("displays entered text in accordion header", async () => {
		const accordionText = "text over troubled accordions";
		const { container, getByText } = await renderComponent({
			accordionText,
			type: "accordion",
			hasAccordion: true,
		});
		getByText(accordionText);
		expect(container).toMatchSnapshot();
	});

	test("clicking the accordion toggle header opens the accordion body", async () => {
		const user = userEvent.setup();
		const { container, getByText } = await renderComponent({
			type: "accordion",
			hasAccordion: true,
			blocks: [{ id: "1w1w1w", type: null }],
		});
		const trigger = getByText("Accordion");
		await user.click(trigger);

		// since we havent assigned children, we should be shown the empty block to choose one
		getByText(emptyBlockText);
		expect(container).toMatchSnapshot();
	});

	test("displays toggle container classes when layout variant is set to toggle", async () => {
		const { container } = await renderComponent({
			type: "toggle",
			variants: { layout: "toggle" },
		});
		expect(container).toMatchSnapshot();
	});

	test("displays empty horizontal blocks for each toggle button", async () => {
		const { container, findAllByText } = await renderComponent({
			type: "toggle",
			blocks: [
				{ id: "1", type: null },
				{ id: "2", type: null },
			],
			variants: { layout: "toggle" },
		});

		// based on the blocks inserted there are 2 toggle buttons with default text and 2 empty blocks
		const toggleButtons = await findAllByText("Toggle");
		expect(toggleButtons).toHaveLength(2);
		const emptyBlocks = await findAllByText(emptyBlockText);
		expect(emptyBlocks).toHaveLength(2);

		expect(container).toMatchSnapshot();
	});

	test("displays 2 different toggle labels dynamically", async () => {
		const firstToggleText = "toggle one";
		const secondToggleText = "toggle tow";
		const { container, getByText } = await renderComponent({
			type: "toggle",
			variants: { layout: "toggle" },
			"label-1": firstToggleText,
			"label-2": secondToggleText,
			blocks: [
				{ id: "1", type: null },
				{ id: "2", type: null },
			],
		});
		getByText(firstToggleText);
		getByText(secondToggleText);
		expect(container).toMatchSnapshot();
	});
});
