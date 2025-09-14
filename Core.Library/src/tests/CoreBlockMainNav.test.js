import CoreBlockMainNav from "../components/block/CoreBlockMainNav";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";

// Mock Nuxt components

const homepageId = "basdfasdflnasdfo";
const pages = [
	{ handle: "bio", title: "Bio" },
	{ handle: "second-page-link", title: "Test 2" },
	{ handle: "third-page-link", title: "Home", id: homepageId },
];

const renderComponent = async (props = {}) => {
	return render(CoreBlockMainNav, {
		props: {
			site: {
				user: { givenName: "bill" },
				navigation: {
					name: "",
					links: [
						{
							id: "Bio 1",
							linkText: "Bio",
							isInternalLink: true,
							openInNewTab: false,
							linkUrl: "/bio",
							isFolder: false,
						},
					],
				},
				pages,
			},
			...props,
		},
		stubs: {
			NuxtLink: "<a><slot /></a>",
		},
	});
};

describe("CoreBlockMainNav", () => {
	it("has accessible roles", async () => {
		// "main menu" text is visually hidden, but has labelled 2
		// of the elements in the navbar
		const { getAllByLabelText, getByRole, getAllByRole, getByText } =
			await renderComponent();
		const ariaLabels = getAllByLabelText("Main Menu");
		// navbar is aria-labelled
		expect(ariaLabels[0].classList.toString()).toBe("navbar");
		// nav ul is aria-labelled and has a role of "menubar"
		const navbarList = getByRole("menubar");
		expect(ariaLabels[1].classList.toString()).toContain("navbar_nav");
		expect(navbarList.classList.toString()).toContain("navbar_nav");
		// clickable links have a role of "menuitem"
		const links = getAllByRole("menuitem");
		expect(links.length).toBe(1);
	});
	// No longer a requirement
	/*
	it("has home page first", async () => {
		const { getAllByRole } = await renderComponent({
			site: {
				homepageId,
				pages,
				user: { givenName: "bill" },
				navigation: {
					name: "",
				},
			},
		});
		// clickable links have a role of "menuitem"
		const links = getAllByRole("menuitem");
		// item set as isHome is the first in the list
		expect(links[0].toString()).toContain(pages[2].handle);
		expect(links[1].toString()).toContain(pages[0].handle);
	});
	*/
	it("logo has src and alt text", async () => {
		const logoAltText = "picture of picture";
		const { container, getByAltText } = await renderComponent({
			site: {
				navigation: {
					menuAlignment: "left",
					logoStyle: "large",
					logo: "https://logo.com",
					logoAltText,
				},
				pages,
			},
		});
		getByAltText(logoAltText);
		expect(container).toMatchSnapshot();
	});

	it("name and job title appear in the nav when passed in", async () => {
		const { container } = await renderComponent({
			site: {
				navigation: {
					menuAlignment: "left",
					logoStyle: "large",
					name: "Johnnyyyy",
					jobTitle: "in the way",
				},
				pages,
			},
		});
		expect(container).toMatchSnapshot();
	});

	it("call to action button appears when values passed in", async () => {
		const buttonText = "call to nature";
		const { container, getByText } = await renderComponent({
			site: {
				navigation: {
					menuAlignment: "left",
					logoStyle: "large",
					button: true,
					buttonLink: "https://linkolinko.link",
					buttonText,
					logoAltText: "imagine a picture in your head",
				},
				pages,
				user: { givenName: "bill" },
			},
		});
		getByText(buttonText);
		expect(container).toMatchSnapshot();
	});

	it("has mobile navigation", async () => {
		const { container } = await renderComponent({
			site: {
				navigation: {
					menuAlignment: "left",
					logoStyle: "medium",
					logoAltText: "imagine a picture in your head",
				},
				pages,
				user: { givenName: "bill" },
			},
			isBuilderMobile: true,
		});
		expect(container).toMatchSnapshot();
	});
});
