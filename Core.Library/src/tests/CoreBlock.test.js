import CoreBlock from "../components/block/CoreBlock";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
import CoreBlockText from "../components/block/CoreBlockText";
import CoreBlockContentCard from "../components/block/CoreBlockContentCard";

const text = "hahaha hahahaha HAHAHAHA";

const renderComponent = async (props = {}) => {
	return render(CoreBlock, {
		props,
		components: { CoreBlockText, CoreBlockContentCard },
	});
};
describe("CoreBlock text component", () => {
	test("text component is shown if settings type is text", async () => {
		const { container } = await renderComponent({
			settings: { type: "text", text },
		});
		expect(container).toMatchSnapshot();
	});

	test("text component size variants adjust classes", async () => {
		const { container } = await renderComponent({
			settings: {
				type: "text",
				text,
				variants: {
					align: "center",
					size: "title",
					style: "italic",
					weight: "bold",
				},
			},
		});

		expect(container).toMatchSnapshot();
	});
});

describe("CoreBlock rich-text component", () => {
	test("rich text component is shown if settings type is rich-text", async () => {
		const { container } = await renderComponent({
			settings: { type: "rich-text", richText: text },
		});
		expect(container).toMatchSnapshot();
	});
});
