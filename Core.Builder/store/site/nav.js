const defaultState = {
	navItems: [
		/*
		{
			id: "",
			linkText: "",
			isInternalLink: true,
			openInNewTab: false,
			linkUrl: "",
			isFolder: false,
			parent: null,
		},
		*/
	],
};

export const state = () => ({
	...defaultState,
});

export const actions = {};

export const getters = {};

export const mutations = {
	moveNavItem(state, { order, dir }) {
		function array_move(arr, old_index, new_index) {
			if (new_index >= arr.length) {
				var k = new_index - arr.length + 1;
				while (k--) {
					arr.push(undefined);
				}
			}
			arr.splice(new_index, 0, arr.splice(old_index, 1)[0]);
			return arr; // for testing
		}
		if (dir == "up" && order < state.navItems.length - 1) {
			state.navItems = array_move([...state.navItems], order, order + 1);
		} else if (dir == "down" && order > 0) {
			state.navItems = array_move([...state.navItems], order, order - 1);
		}
	},
	deleteNavItem(state, index) {
		const arr = [...state.navItems];
		arr.splice(index, 1);
		state.navItems = arr;
	},
	insertNavItem(state, navItem) {
		let arr = [...state.navItems];
		arr.push({
			id: `${navItem.linkText}-${Math.floor(new Date().getTime() / 100)}`,
			...navItem,
		});
		state.navItems = arr;
	},
	setNavigationItem(state, { id, key, val }) {
		const links = [...state.navItems];
		const newLink = links.find((l) => l.id === id);
		newLink[key] = val;
		links.splice(
			links.findIndex((l) => l.id === id),
			1,
			newLink
		);
		state.navItems = links;
	},
	setNavItems(state, navItems) {
		state.navItems = navItems;
	},
};
