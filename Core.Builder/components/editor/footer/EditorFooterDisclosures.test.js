import { mount, createLocalVue } from "@vue/test-utils";
import EditorFooterDisclosures from "./EditorFooterDisclosures.vue";
import "@testing-library/jest-dom";
import Vuetify from "vuetify";
import Vuex from "vuex";
import { getters as siteGetters } from "~/store/site/index.js";

const localVue = createLocalVue();
localVue.use(Vuex);

const content = [
	{
		type: "disclosure",
		title: "something license to sell insurance else",
		presetSettings: {
			text: "license to sell insurance has {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "something MDRT Member else",
		presetSettings: {
			text: "MDRT Member disclosure has {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "General Office disclosure",
		presetSettings: {
			text: "General Office disclosure {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "something Nautilus Member hello",
		presetSettings: {
			text: "test Nautilus Member {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "something Registered Representative Blahhhhh",
		presetSettings: {
			text: "test Registered Representative {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "something Ownership something",
		presetSettings: {
			text: "test Ownership disclosure for {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "something Account Advise Statement something",
		presetSettings: {
			text: "test Account Advise Statement disclosure for {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
	{
		type: "disclosure",
		title: "something Eagle Financial Advisor hello",
		presetSettings: {
			text: "test Eagle Financial Advisor {{ user.fistName }} mustache values but won't render in the the editor",
		},
	},
];

let wrapper;

const wrapperFactory = (user = {}) => {
	const store = new Vuex.Store({
		modules: {
			library: {
				namespaced: true,
				state: {
					content,
				},
				actions: { getContentByType: jest.fn() },
			},
			site: {
				namespaced: true,
				state: {
					workingSite: {
						footer: {
							disclosures: [],
						},
					},
					storedSite: {
						user,
					},
				},
				getters: siteGetters,
				mutations: { setWorkingSiteSetting: jest.fn() },
			},
		},
	});
	return mount(EditorFooterDisclosures, {
		vuetify: new Vuetify(),
		store,
		localVue,
	});
};

afterEach(() => {
	wrapper.destroy();
});

describe("EditorFooterDisclosures", () => {
	test("exists", () => {
		wrapper = wrapperFactory();
		expect(wrapper.exists()).toBeTruthy();
	});

	test("has general office disclosure only if employeeType === GO", async () => {
		wrapper = wrapperFactory({ employeeType: "GO" });
		await localVue.nextTick();
		expect(wrapper.vm.isGO).toBe(true);
		const testGODisclosure = content.filter((d) => {
			return d.title.toLowerCase().includes("general office");
		});
		// only one disclosure available for GO, and it's in a p tag
		const disclosures = wrapper.vm.disclosures;
		expect(disclosures).toHaveLength(1);
		const GODisclosure = wrapper.vm.GODisclosure;
		expect(GODisclosure).toEqual(
			`<p>${testGODisclosure[0].presetSettings.text}</p>`
		);
	});

	test("all footer disclosure are in the expected order if not general office", async () => {
		wrapper = await wrapperFactory({
			ddcUserData: {
				dba1Nm: "test",
				nautilusInd: "Y",
				eagleInd: "Y",
				mdrtLastYear: new Date().getFullYear(),
				regRepInd: "Y",
				ddcLicenseData: [
					{
						licenseExpiryDt: "2025-12-31",
						licenseIssueDt: "2022-12-31",
						licenceLobCode: "30", // registered rep
					},
					{
						licenseExpiryDt: "2025-12-31",
						licenseIssueDt: "2022-12-31",
						licenceLobCode: "80", // license to sell
					},
				],
			},
		});
		await localVue.nextTick();
		const testAgentDisclosures = content.filter((d) => {
			return !d.title?.toLowerCase().includes("general office");
		});
		expect(wrapper.vm.isGO).toBe(false);
		expect(wrapper.vm.isLicensedToSellInsurance).toBe(true);
		expect(wrapper.vm.isRegisteredRep).toBe(true);
		expect(wrapper.vm.isEagleAdvisor).toBe(true);
		expect(wrapper.vm.isNautilusMember).toBe(true);
		expect(wrapper.vm.isDBA).toBe(true);
		const disclosures = wrapper.vm.disclosures;
		// disclosures available for agent sites should not include GO
		const GODisclosures = disclosures.filter((d) => {
			return d.title?.toLowerCase().includes("general office");
		});
		expect(GODisclosures.length).toBeFalsy();
		expect(disclosures.length).toEqual(testAgentDisclosures.length);
		expect(disclosures[0].item.toLowerCase()).toContain("license"); // license to sell first in the array
		expect(disclosures[1].item.toLowerCase()).toContain("registered"); // registered rep second
		expect(disclosures[2].item.toLowerCase()).toContain("eagle"); // eagle ind third
		expect(disclosures[3].item.toLowerCase()).toContain("nautilus"); // nautilus fourth
		expect(disclosures[4].item.toLowerCase()).toContain("mdrt"); // mdrt fifth
		expect(disclosures[5].item.toLowerCase()).toContain("ownership"); // ownership sixth
		expect(disclosures[6].item.toLowerCase()).toContain("advise"); // account advise statement seventh
	});
});
