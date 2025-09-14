import CoreBlockSocialLinks from "../components/block/CoreBlockSocialLinks";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";

const renderComponent = async (propsData = {}) => {
	return render(CoreBlockSocialLinks, {
		propsData: {
			dataSite: {
				id: 12,
			},
			...propsData,
		},
	});
};

describe("CoreBlockSocialLinks", () => {
	test("if facebook value is set, make sure facebook icon appears and link matches value", async () => {
		const { container } = await renderComponent({
			settings: {
				facebook: "http://facebook.com",
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("if youtube value is set, make sure youtube icon appears and link matches value", async () => {
		const { container } = await renderComponent({
			settings: {
				youtube: "http://youtube.com",
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("if headline is set, make sure headline appears", async () => {
		const { container } = await renderComponent({
			settings: { headline: "text" },
		});
		expect(container).toMatchSnapshot();
	});

	test("if username is set, make sure username appears", async () => {
		const { container } = await renderComponent({
			settings: { username: "text" },
		});
		expect(container).toMatchSnapshot();
	});
});
