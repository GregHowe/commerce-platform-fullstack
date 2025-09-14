import Vue from "vue";
import { ValidationProvider, ValidationObserver, extend } from "vee-validate";
import { required, mimes, max, alpha_dash } from "vee-validate/dist/rules";
import fileHelper from "~/helpers/file";
import {
	domain,
	email,
	phone,
	url,
	zipCode,
	lowercase,
} from "~/../Core.Library/src/helpers/regex";

// custom error messages
extend("required", {
	...required,
	message: "{_field_} cannot be empty",
});

extend("alpha_dash", {
	...alpha_dash,
	message:
		"{_field_} may contain alpha-numeric characters as well as dashes and underscores",
});

extend("max", {
	...max,
	params: ["length"],
	message: "{_field_} must be less than {length} characters",
});

extend("mimes", {
	...mimes,
	message: (fieldName, values) => {
		const types = [];
		for (const key in values) {
			// only keys in the object with numeric indexes are the possible mime types
			if (!isNaN(key)) {
				// just show the available file types without "image/" or "application/"
				types.push(values[key].split("/")[1]);
			}
		}
		return `The ${fieldName} field must be ${types}`;
	},
});

extend("maxSize", {
	validate(value, args) {
		return fileHelper.toMb(value) <= args.length;
	},
	params: ["length"],
	message: "{_field_} must be less than {length} kb",
});

extend("onlyNums", {
	validate: zipCode,
	message: "{_field_} Format: 12345 or 12345-6789. Numbers only.",
});

extend("domain", {
	validate: domain,
	message: "{_field_} must be a valid domain",
});

extend("url", {
	validate: url,
	message: "{_field_} must be a valid url",
});

extend("email", {
	validate: email,
	message: "{_field_} must be a valid email",
});

extend("json", {
	validate(value) {
		try {
			JSON.parse(value);
		} catch (e) {
			return false;
		}
		return true;
	},
	message: "{_field_} is Invalid JSON",
});

extend("phone", {
	validate: phone,
	message: "{_field_} must be a valid phone number. Format: 123-456-7890",
});

extend("lowercase", {
	validate: lowercase,
	message: "{_field_} must be lowercase characters",
});

Vue.component("ValidationProvider", ValidationProvider);
Vue.component("ValidationObserver", ValidationObserver);
