import CoreBlockFooter from "../components/block/footer/CoreBlockFooter.vue";
import CoreBlockFooterDisclosures from "../components/block/footer/CoreBlockFooterDisclosures.vue";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";
const homepageId = "basdfasdflnasdfo";
const pages = [
	{ handle: "page-link", title: "Test" },
	{ handle: "second-page-link", title: "Test 2" },
	{ handle: "third-page-link", title: "Test 3", id: homepageId },
];
const banner = {
	bodyCopy:
		"For more information about NYLIFE Securities LLC and its investment professionals.",
	buttonText: "Click here",
	buttonLink: "https://brokercheck.finra.org/",
};

const renderComponent = async (props = {}) => {
	return render(CoreBlockFooter, {
		components: { CoreBlockFooterDisclosures },
		props: {
			site: {
				pages,
			},
			...props,
		},
	});
};

describe("CoreBlockFooter", () => {
	it("exists", async () => {
		await renderComponent();
	});

	it("logo has src and alt text", async () => {
		const logoAltText = "picture of picture";
		const { container, getByAltText } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					logoStyle: "large",
					logo: "https://logo.com",
					logoAltText,
					showDisclosures: true,
					showBanner: true,
					banner,
				},
				pages,
			},
		});
		getByAltText(logoAltText);
		expect(container).toMatchSnapshot();
	});

	it("name and job title appear in the footer when passed in", async () => {
		const { container } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					logoStyle: "both",
					name: "Johnnyyyy",
					jobTitle: "in the way",
					showDisclosures: true,
					showBanner: true,
					banner,
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});

	it("has a sign up form", async () => {
		const { container } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					showSignUpForm: true,
					signUpFormDisclosure:
						"I beg you please do not sign up for this",
					showDisclosures: true,
					showBanner: true,
					banner,
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});

	it("has links to social media", async () => {
		const { container } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					facebook: "https://youtube.com",
					linkedin: "https://youtube.com",
					instagram: "https://youtube.com",
					twitter: "https://youtube.com",
					youtube: "https://youtube.com",
					showDisclosures: true,
					showBanner: true,
					banner,
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});

	it("disclosures appear in the footer when passed in", async () => {
		const { container } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					disclosures: [
						"first disclosure in the array",
						"second disclosurre but disclosure is spelled wrong",
					],
					showDisclosures: true,
					showBanner: true,
					banner,
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});

	it("disclosures do not appear in the footer when passed in but showDisclosures is false", async () => {
		const { container } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					disclosures: [
						"first disclosure in the array",
						"second disclosurre but disclosure is spelled wrong",
					],
					showDisclosures: false,
					showBanner: true,
					banner,
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});

	it("banner does not appear in the footer when passed in but showBanner is false", async () => {
		const { container } = await renderComponent({
			site: {
				footer: {
					showFooter: true,
					showDisclosures: false,
					showBanner: false,
					banner,
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});
});
