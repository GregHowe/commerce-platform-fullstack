import CoreBlockButton from "../components/block/CoreBlockButton";
import CoreIcon from "../components/icon/CoreIcon";
import CoreIconLeftArrow from "../components/icon/CoreIconLeftArrow";
import CoreIconDownArrow from "../components/icon/CoreIconDownArrow";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";

const renderComponent = async (props = {}) => {
	return render(CoreBlockButton, {
		props,
		components: { CoreIcon, CoreIconLeftArrow, CoreIconDownArrow },
	});
};
describe("CoreBlockButton", () => {
	test("left arrow icon is shown is shown if passed into prop", async () => {
		const { container } = await renderComponent({
			settings: {
				icon: "left-arrow",
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("if url is set, make sure button href matches", async () => {
		const { container } = await renderComponent({
			settings: { url: "https://www.google.com" },
		});
		expect(container).toMatchSnapshot();
	});

	test("if newTab is set, make sure button target matches", async () => {
		const { container } = await renderComponent({
			settings: { newTab: "_blank" },
		});
		expect(container).toMatchSnapshot();
	});

	test("if url is an internal link, nuxt link is used for routing", async () => {
		const { container } = await renderComponent({
			settings: { url: "/internal-link" },
		});
		expect(container).toMatchSnapshot();
	});
});
