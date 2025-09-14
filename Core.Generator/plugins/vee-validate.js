import Vue from "vue";
import { ValidationProvider, ValidationObserver, extend } from "vee-validate";
import {
	required,
	numeric,
	max,
	min,
	email,
	alpha_num,
	alpha_dash,
} from "vee-validate/dist/rules";
import { isAdult } from "@libraryHelpers/regex";
// custom error messages
extend("required", {
	...required,
	message: "This field is required",
});

extend("max", {
	...max,
	params: ["length"],
	message: "{_field_} must be less than {length} characters",
});

extend("min", {
	...min,
	params: ["length"],
	message: "{_field_} must be more than {length} characters",
});

extend("numeric", {
	...numeric,
	message: "{_field_} must be a number",
});

extend("alpha_num", {
	...alpha_num,
	message: "{_field_} must contain only letters and numbers",
});

extend("alpha_dash", {
	...alpha_dash,
	message: "{_field_} must contain only letters, numbers, or dashes",
});

extend("email", {
	...email,
	message: "Please enter an email address in the name@domain.com format.",
});

extend("date_format", {
	validate(value) {
		let dateRegex = /^\d{2}\/\d{2}\/\d{4}$/;
		return dateRegex.test(value);
	},
	message: "Please enter a date of birth in the MM/DD/YYYY format.",
});

extend("zip", {
	validate(value) {
		return value.length === 5 || value.length === 9;
	},
	message: "This field must be 5 or 9 digits, 00000 or 00000-0000.",
});

extend("linkedin", {
	validate(value) {
		let dateRegex =
			/^(http(s)?:\/\/)?([\w]+\.)?linkedin\.com\/(pub|in|profile)\/([-a-zA-Z0-9]+)\/*/;
		return dateRegex.test(value);
	},

	message: "Must be a valid LinkedIn url",
});

extend("phone", {
	validate: (value) => {
		return value.match(/^\d{3}-\d{3}-\d{4}$/);
	},
	message: "{_field_} must be a valid phone number. Format: 123-456-7890",
});

extend("isAdult", {
	validate: isAdult,
	message: "You must be 18 or older to submit this form.",
});

Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);
