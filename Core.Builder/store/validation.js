export const state = () => ({
	validation: {},
});

export const getters = {
	validationFailedItem: (state) => (key) => {
		return !!failedValidation(state.validation).find(
			(item) => item === key
		);
	},
	validationFailedAny: (state) => {
		return !!failedValidation(state.validation).length;
	},
	validationFailedMessages: (state) => {
		let messages = [];
		failedValidation(state.validation).map(function (item) {
			const rules = Object.values(state.validation[item].failedRules);
			messages.push(rules);
		});
		return messages.flat();
	},
};

export const mutations = {
	setValidation(state, val) {
		state.validation = val;
	},
};

const failedValidation = (obj) => {
	// object of all fields which have failed validation
	return Object.keys(obj).filter((field) => obj[field].failed);
};
