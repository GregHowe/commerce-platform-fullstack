const zipCode = (value) => {
	return /(^(?!0{5})(\d{5})(?!-?0{4})(|-\d{4})?$)/.test(value);
};

const domain = (value) => {
	return /^((?!-)[\da-z-]+\.[a-z.]{2,6})*?$/.test(value);
};

const url = (value) => {
	return (
		/^(?:(?:(?:https?|ftp):)?\/\/)(?:\S+(?::\S*)?@)?(?:(?!(?:10|127)(?:\.\d{1,3}){3})(?!(?:169\.254|192\.168)(?:\.\d{1,3}){2})(?!172\.(?:1[6-9]|2\d|3[0-1])(?:\.\d{1,3}){2})(?:[1-9]\d?|1\d\d|2[01]\d|22[0-3])(?:\.(?:1?\d{1,2}|2[0-4]\d|25[0-5])){2}(?:\.(?:[1-9]\d?|1\d\d|2[0-4]\d|25[0-4]))|(?:(?:[a-z0-9\u00a1-\uffff][a-z0-9\u00a1-\uffff_-]{0,62})?[a-z0-9\u00a1-\uffff]\.)+(?:[a-z\u00a1-\uffff]{2,}\.?))(?::\d{2,5})?(?:[/?#]\S*)?$/.test(
			value
		) || /(?:mailto?:([\w\-.+]+@[\w\-.]+\.[A-Za-z]{2,}))/.test(value)
	);
};

const email = (value) => {
	// eslint-disable-next-line no-control-regex
	return /^(?:[A-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9]{2,}(?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])$/.test(
		value
	);
};

const phone = (value) => {
	return /^\d{3}-\d{3}-\d{4}$/.test(value);
};

const lowercase = (value) => {
	return /^[a-z0-9_-]+$/.test(value);
};

const isAdult = (value) => {
	const currentYear = new Date().getFullYear();
	const birthYear = new Date(value).getFullYear();
	const age = currentYear - birthYear;
	return age >= 18 ? true : false;
};

export { zipCode, domain, url, email, phone, lowercase, isAdult };
