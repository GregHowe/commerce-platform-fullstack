import { uniq as _uniq } from "lodash";
export const state = () => ({});

export const getters = {
	emailAddress: (state, getters, rootState) =>
		rootState.auth?.user?.mail || "",

	firstName: (state, getters, rootState) =>
		rootState.auth?.user?.firstName || "",
	lastName: (state, getters, rootState) =>
		rootState.auth?.user?.lastName || "",

	fullName: (state, getters, rootState) =>
		rootState.auth?.user?.displayName || "",

	// If no sites are set up, they are a first timer. Plan to add logic to this
	firstTimeUser: (state, getters, rootState) =>
		!rootState?.site?.siteList?.length,

	permissions: (state, getters, rootState) => {
		const base = _uniq(rootState.auth?.user?.permissions) || [];
		return [...base, "constructblocks"];
	},

	role: (state, getters, rootState) => {
		if (rootState.auth?.user?.isOBOSession) {
			return "OBO";
		}
		return rootState.auth?.user?.employeeType;
	},

	oboUser: (state, getters, rootState) => {
		if (rootState.auth?.user?.obonylUser) {
			// once it is located top level, this will return the object
			return rootState.auth.user.obonylUser;
		}
		return null;
	},

	hasPermission: (state, getters) => (permission) => {
		return (
			getters.permissions?.includes(permission) ||
			getters.role === "Admin"
		);
	},

	isEligible: (state, getters, rootState) => {
		if (getters.role !== "Agent") {
			return true; // only agents can be ineligible, so non-agents are automatically eligible
		}
		if (rootState?.site?.siteList?.length > 0) {
			return true; // this agent has a site in our system already, and should be able to manage it
		}
		if (rootState.auth?.user?.hasWebsiteAgent) {
			return true; // this agent has a site according to NYL data feed
		}
		if (rootState.auth?.user?.hasWebsiteRecruiter) {
			return true; // this agent has a site according to NYL data feed
		}
		return false; // only agents with no existing sites (either core or NYL data feed)
	},
	hasBeenWelcomed: (state, getters, rootState) => {
		return rootState.auth?.user?.welcomePagePresented;
	},
	hasAcceptedTerms: (state, getters, rootState) => {
		return rootState.auth?.user?.acceptedTerms;
	},
};
