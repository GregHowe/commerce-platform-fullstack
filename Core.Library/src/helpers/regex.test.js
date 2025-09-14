import {
	domain,
	email,
	phone,
	url,
	zipCode,
	lowercase,
	isAdult,
} from "./regex";

describe("Regex Helpers", () => {
	test("zipCode correctly handles zip codes", () => {
		expect(zipCode("1234567890")).toBe(false);
		expect(zipCode("53132")).toBe(true);
		expect(zipCode("53132-7896")).toBe(true);
	});
	test("domain correctly validates domains", () => {
		expect(domain("google$.co.jp")).toBe(false);
		expect(domain("-google.com")).toBe(false);
		expect(domain(".google.com")).toBe(false);
		expect(domain("google.com")).toBe(true);
		expect(domain("k12.xlsa.info")).toBe(true);
	});
	test("url correctly validates urls", () => {
		expect(url("www.website.com")).toBe(false);
		expect(url("https//website.com/")).toBe(false);
		expect(url("https:/website.com/")).toBe(false);
		expect(url("website.com:///podcasts/newspodcasturl")).toBe(false);
		expect(url("mailto://editor@company.com")).toBe(false);
		expect(url("tel:45212136")).toBe(false);
		expect(url("mail-to:editor@company.com")).toBe(false);
		expect(url("http://www.website.com")).toBe(true);
		expect(url("http://www.website.com/")).toBe(true);
		expect(url("http://website.com")).toBe(true);
		expect(url("http://website.com/")).toBe(true);
		expect(url("http://www.website.com/path/to/page/$")).toBe(true);
		expect(url("https://www.website.com")).toBe(true);
		expect(url("https://www.website.com/")).toBe(true);
		expect(url("https://website.com")).toBe(true);
		expect(url("https://website.com/")).toBe(true);
		expect(url("https://www.website.com/path/to/page")).toBe(true);
		expect(url("mailto:test@website.com")).toBe(true);
		expect(url("mailto:test@website.com")).toBe(true);
		expect(url("mailto:test@website.test.com")).toBe(true);
		expect(url("mailto:test_email.test@website.co")).toBe(true);
		expect(url("mailto:editor@website.com")).toBe(true);
		expect(url("mailto:editor@website.com")).toBe(true);
	});
	test("email correctly validates emails", () => {
		expect(email("boblob@gmail.com")).toBe(true);
		expect(email("blwoeirjw@eoirj")).toBe(false);
	});
	test("phone correctly validates phone numbers", () => {
		expect(phone("not a phone number")).toBe(false);
		expect(phone("123-456-7890")).toBe(true);
	});
	test("lowercase correctly validates strings", () => {
		expect(
			lowercase("lowercasewith-dash_underscore-and-numbers_1234")
		).toBe(true);
		expect(lowercase("UPPERCASE")).toBe(false);
		expect(
			lowercase(
				"lowercasewith-dash_underscore-and-numbers_1234_and_onEuppsercaseletter"
			)
		).toBe(false);
	});
	test("age correctly validates user is 18 years or older", async () => {
		expect(isAdult("11/11/2010")).toBe(false);
		expect(isAdult("11/11/2000")).toBe(true);
	});
});
