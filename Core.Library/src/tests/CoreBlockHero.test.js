import CoreBlockHero from "../components/block/CoreBlockHero";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";

const renderComponent = async (props = {}) => {
	return render(CoreBlockHero, {
		props,
		stubs: {
			NuxtLink: true,
			ForgeControlBlock: true,
		},
		mocks: {
			$route: { fullPath: "/page/hierarchy/current", name: "current" },
			$router: {
				match: () => {
					return { name: "current" };
				},
			},
		},
	});
};

describe("CoreBlockHero", () => {
	test("exists", async () => {
		await renderComponent({ settings: { id: "oi" } });
	});

	test("uses ForgeControlBlock if isEditable", async () => {
		const { container } = await renderComponent({
			settings: {
				id: "editable",
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
			isEditable: true,
		});
		expect(container).toMatchSnapshot();
	});

	test("default settings", async () => {
		const { container, getByText, queryByText } = await renderComponent({
			settings: {
				id: "default",
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		// has breadcrumbs
		getByText("page");
		getByText("hierarchy");

		// has "keep scrolling" section
		getByText("Keep Scrolling");
		expect(container).toMatchSnapshot();

		// buttons are not shown
		const hasPrimaryButton = queryByText("Primary");
		expect(hasPrimaryButton).toBeFalsy();
		const hasSecondaryButton = queryByText("Secondary");
		expect(hasSecondaryButton).toBeFalsy();
	});

	test("displays breadcrumb variant", async () => {
		const { container } = await renderComponent({
			settings: {
				id: "has-breadcrumb-variant",
				variants: { breadcrumbVariant: "compact" },
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("displays video in iframe when video embed url is supplied", async () => {
		const { container } = await renderComponent({
			settings: {
				id: "has-video",
				sideMediaSrc: "https://www.youtube.com/embed/lMh0NRwqElg",
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("displays image when url is supplied", async () => {
		const { container } = await renderComponent({
			settings: {
				id: "has-image",
				sideMediaSrc: "https://picsum.photos/200/300.jpg",
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("displays image on right", async () => {
		const { container } = await renderComponent({
			settings: {
				id: "has-image",
				sideMediaSrc: "https://picsum.photos/200/300.jpg",
				variants: { "image-align": "image-right" },
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		expect(container).toMatchSnapshot();
	});

	test("displays text when supplied", async () => {
		const overline = "overline content";
		const headline = "headliney type stuff";
		const subline = "very subliney";
		const bodyText = "when writing test content, I often sort of ramble.";
		const { getByText, container } = await renderComponent({
			settings: {
				id: "texty",
				overline,
				headline,
				subline,
				bodyText,
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		getByText(overline);
		getByText(headline);
		getByText(subline);
		getByText(bodyText);
		expect(container).toMatchSnapshot();
	});

	test("displays all three buttons when urls are supplied", async () => {
		const { getByText, container } = await renderComponent({
			settings: {
				id: "has-buttonz",
				"button-primary-url": "https://google.com",
				"button-secondary-url": "https://google.com",
				"button-tertiary-url": "https://google.com",
				showBreadcrumb: true,
				showKeepScrolling: true,
			},
		});
		getByText("Primary Button Label");
		getByText("Secondary Button Label");
		getByText("Tertiary Button Label");
		expect(container).toMatchSnapshot();
	});

	test("does not show keep scrolling if turned off", async () => {
		const { container, queryByText } = await renderComponent({
			settings: {
				id: "no-scroll",
				showKeepScrolling: false,
			},
		});

		// does not have "keep scrolling" section
		const scroller = queryByText("Keep Scrolling");
		expect(scroller).toBeFalsy();

		expect(container).toMatchSnapshot();
	});

	test("displays background image if set", async () => {
		const { container } = await renderComponent({
			settings: {
				id: "background",
				backgroundMediaSrc: "https://picsum.photos/200/300.jpg",
			},
		});

		expect(container).toMatchSnapshot();
	});
});
