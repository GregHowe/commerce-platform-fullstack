import CoreIcon from "../components/icon/CoreIcon";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
import CoreIconLeftArrow from "../components/icon/CoreIconLeftArrow";
import CoreIconPhone from "../components/icon/CoreIconPhone";

const renderComponent = async (props = {}) => {
	return render(CoreIcon, {
		props,
		components: { CoreIconLeftArrow, CoreIconPhone },
	});
};
describe("CoreIcon uses svg with value of passed in prop", () => {
	test("matches snapshot for left arrow icon", async () => {
		const { getByRole } = await renderComponent({ icon: "left-arrow" });
		const img = getByRole("img");
		expect(img).toBeInTheDocument();
		expect(img).toMatchSnapshot();
	});

	test("matches snapshot for phone icon", async () => {
		const { getByRole } = await renderComponent({ icon: "phone" });
		const img = getByRole("img");
		expect(img).toBeInTheDocument();
		expect(img).toMatchSnapshot();
	});
});
