module.exports = function (length = 6) {
	const chars = "bcdfghkmnpqrstvwxyz1234567890";
	const rand = Array.from(
		{ length },
		(v, k) => chars[Math.floor(Math.random() * chars.length)]
	);
	return rand.join("");
};
