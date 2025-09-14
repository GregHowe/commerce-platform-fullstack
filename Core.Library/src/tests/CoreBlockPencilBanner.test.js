import CoreBlockPencilBanner from "../components/block/CoreBlockPencilBanner";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";

const renderComponent = async (props = {}) => {
	return render(CoreBlockPencilBanner, {
		props: {
			...props,
			dataSite: {
				id: 666,
			},
		},
	});
};

describe("CoreBlockPencilBanner", () => {
	test("exists", async () => {
		await renderComponent({ settings: {} });
	});

	test("displays entered text", async () => {
		const { container } = await renderComponent({
			settings: { bodyCopy: "text across a banner" },
		});
		expect(container).toMatchSnapshot();
	});

	test("shows button if button text is entered", async () => {
		const { container } = await renderComponent({
			settings: { buttonText: "text in a button" },
		});
		expect(container).toMatchSnapshot();
	});

	test("shows close button if setting is true", async () => {
		const { container } = await renderComponent({
			settings: { closeButton: true },
		});
		expect(container).toMatchSnapshot();
	});
});
