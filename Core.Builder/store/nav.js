export const state = () => ({
	leftDrawer: [
		{
			type: "site",
			title: "Dashboard",
			icon: "mdi-home-outline",
			pathName: "site-siteId",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "site",
			title: "My Website",
			icon: "mdi-monitor-cellphone-star",
			pathName: "site-siteId-page-pageId",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "site",
			title: "Assets",
			icon: "mdi-filmstrip",
			pathName: "",
			restrictedTo: [],
			position: "",
			subNav: [
				{
					title: "Images",
					icon: "",
					path: "assets/images",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Videos",
					icon: "",
					path: "assets/videos",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Documents",
					icon: "",
					path: "assets/documents",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Links",
					icon: "",
					path: "assets/links",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Disclaimers",
					icon: "",
					path: "assets/disclaimers",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Disclosures",
					icon: "",
					path: "assets/disclosures",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Text Collections",
					icon: "",
					path: "assets/texts",
					restrictedTo: [],
					position: "",
				},
				{
					title: "Blogs/Articles",
					icon: "",
					path: "assets/articles",
					restrictedTo: [],
					position: "",
				},
			],
		},
		{
			type: "site",
			title: "Theme Showroom",
			icon: "mdi-lightbulb-on-outline",
			pathName: "site-siteId-showroom",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "site",
			title: "Analytics",
			icon: "mdi-poll",
			pathName: "site-siteId-analytics",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "site",
			title: "Compliance",
			icon: "mdi-shield-star-outline",
			pathName: "site-siteId-compliance",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "site",
			title: "Settings",
			icon: "mdi-cog",
			pathName: "site-siteId-settings",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "library",
			title: "Dashboard",
			icon: "mdi-home-outline",
			pathName: "library-content-dashboard",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "library",
			title: "Add New Media",
			icon: "mdi-plus",
			pathName: "library-content-new",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "library",
			title: "Content Library",
			icon: "mdi-filmstrip",
			pathName: "library-content",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "library",
			title: "Theme Showroom",
			icon: "mdi-chart-bar",
			pathName: "library-content-themes",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "library",
			title: "Compliance",
			icon: "mdi-shield-outline",
			pathName: "library-content-compliance",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
		{
			type: "library",
			title: "Settings",
			icon: "mdi-cog",
			pathName: "library-content-settings",
			restrictedTo: [],
			position: "",
			subNav: [],
		},
	],
	topLinks: [
		{
			title: "Website Management",
			icon: "",
			pathName: "sites",
			restrictedTo: ["Agent", "FieldUser"],
			position: "left",
			subNav: [],
		},
		{
			title: "Asset Management",
			icon: "",
			pathName: "library-content-dashboard",
			restrictedTo: ["ContentLibrarian"],
			position: "left",
			subNav: [],
		},
		{
			title: "Test Harnesses",
			icon: "mdi-test-tube",
			pathName: "library-content-dashboard",
			restrictedTo: ["Admin"],
			position: "left",
			subNav: [
				{
					title: "Lead Forms",
					icon: "mdi-list-box-outline",
					pathName: "testharness-leadforms",
					restrictedTo: [],
					position: "",
				},
			],
		},
		{
			title: "Migration Manager",
			icon: "mdi-home-switch",
			pathName: "migrations",
			restrictedTo: ["Admin"],
			position: "left",
			subNav: [],
		},
	],
});

export const getters = {
	leftDrawerLinks: (state, rootGetters) => (type) => {
		if (rootGetters["user/role"] === "Admin") {
			return state.leftDrawer;
		}
		return state.leftDrawer
			.filter((link) => link.type === type)
			.filter((l) => l.restrictedTo.length == 0); // filter allowed links for user,
	},
	topLinks: (state, getters, rootState, rootGetters) => {
		if (rootGetters["user/role"] === "Admin") {
			return state.topLinks;
		}
		return state.topLinks.filter((l) =>
			l.restrictedTo.includes(rootGetters["user/role"])
		); // filter allowed links for user,
	},
};
