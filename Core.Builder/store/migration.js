import { pick as _pick } from "lodash";

// These mappings need to be expanded
// [Core Property, NYL Property]

const nylMappings = [
	["marketerId", "employeeId"],
	["email", "mail"],
	["address1", "streetAddress"],
	["zip", "postalCode"],
];

const defaultAgent = {
	marketerId: "",
	caLicense: "",
	arLicense: "",
	displayName: "",
	firstName: "",
	preferredName: "",
	middleName: "",
	lastName: "",
	nameSuffix: "",
	email: "",
	title: "",
	address1: "",
	address2: "",
	address3: "",
	city: "",
	state: "",
	zip: "",
	businessPhone: "",
	cellPhone: "",
	fax: "",
	additionalPhoneNumber1: "",
	additionalPhoneNumber2: "",
	facebook: "",
	linkedIn: "",
	twitter: "",
	calendly: "",
};

export const state = () => ({
	currentStep: 1,
	agentSearchEmail: "",
	agentSearchmarketerId: "",
	currentAgent: defaultAgent,
	templateType: "",
	isDBA: true,
	dbaDisplayName: "",
	prebuiltPages: [],
	calculators: [],
	error: "",
});

export const actions = {
	SEARCH_AGENT: async function ({ state, commit }, userEmail) {
		try {
			commit("setError", "");
			commit("resetAgent");
			const agent = await this.$axios.get(
				`/users/showuser?userEmailAddress=${userEmail}`
			);
			commit("setAgent", agent.data);
		} catch (err) {
			commit("setError", "User Not Found");
		}
	},
};

export const getters = {
	sortedAgentFields(state) {
		return [
			{
				title: "Identification",
				fields: [
					{
						key: "marketerId",
						label: "Marketer ID",
						cols: 3,
						rules: [],
					},
					{
						key: "caLicense",
						label: "CA License #",
						cols: 3,
						rules: [],
					},
					{
						key: "arLicense",
						label: "AR License #",
						cols: 3,
						rules: [],
					},
					{
						key: "firstName",
						label: "First Name",
						cols: 4,
						rules: [],
					},
					{
						key: "preferredName",
						label: "Preferred Name",
						cols: 4,
						rules: [],
					},
					{
						key: "middleName",
						label: "Middle Name",
						cols: 4,
						rules: [],
					},
					{
						key: "lastName",
						label: "Last Name",
						cols: 4,
						rules: [],
					},
					{
						key: "nameSuffix",
						label: "Name Suffix",
						cols: 4,
						rules: [],
					},
					{
						key: "email",
						label: "Email",
						cols: 4,
						rules: [],
					},
					{
						key: "title",
						label: "Title",
						cols: 4,
						rules: [],
					},
				],
			},
			{
				title: "Address",
				fields: [
					{
						key: "address1",
						label: "address1",
						cols: 12,
						rules: [],
					},
					{
						key: "address2",
						label: "address2",
						cols: 12,
						rules: [],
					},
					{
						key: "address3",
						label: "address3",
						cols: 12,
						rules: [],
					},
					{
						key: "city",
						label: "City",
						cols: 5,
						rules: [],
					},
					{
						key: "state",
						label: "State",
						cols: 3,
						rules: [],
					},
					{
						key: "zip",
						label: "zip",
						cols: 4,
						rules: [],
					},
				],
			},
			{
				title: "Phone",
				fields: [
					{
						key: "businessPhone",
						label: "Business Phone",
						icon: "mdi-phone",
						cols: 6,
						rules: [],
					},
					{
						key: "cellPhone",
						label: "Cell Phone",
						icon: "mdi-phone",
						cols: 6,
						rules: [],
					},
					{
						key: "fax",
						label: "Business Phone",
						icon: "mdi-fax",
						cols: 6,
						rules: [],
					},
					{
						key: "additionalPhoneNumber1",
						label: "Additional 1",
						icon: "mdi-phone",
						cols: 6,
						rules: [],
					},
					{
						key: "additionalPhoneNumber2",
						label: "Additional 2",
						icon: "mdi-phone",
						cols: 6,
						rules: [],
					},
				],
			},
			{
				title: "Social",
				fields: [
					{
						key: "facebook",
						label: "Facebook",

						cols: 6,
						rules: [],
					},
					{
						key: "linkedIn",
						label: "LinkedIn",

						cols: 6,
						rules: [],
					},
					{
						key: "twitter",
						label: "Twitter",

						cols: 6,
						rules: [],
					},
					{
						key: "calendley",
						label: "Calendly",
						cols: 6,
						rules: [],
					},
				],
			},
		];
	},
};

export const mutations = {
	resetAgent(state) {
		state.currentAgent = defaultAgent;
	},
	setAgent(state, agent) {
		const cA = {
			...state.currentAgent,
		};
		Object.keys(agent).forEach((k) => {
			let mapping = nylMappings.find((m) => m[1] === k);
			if (cA[k] !== undefined) {
				cA[k] = agent[k];
			} else if (mapping) {
				cA[mapping[0]] = agent[k];
			}
		});
		state.currentAgent = cA;
	},
	setAgentProperty(state, change) {
		const cA = {
			...state.currentAgent,
		};
		cA[change.key] = change.value;
	},
	setError(state, error) {
		state.error = error;
	},
	setStep(state, step) {
		state.currentStep = step;
	},
};
