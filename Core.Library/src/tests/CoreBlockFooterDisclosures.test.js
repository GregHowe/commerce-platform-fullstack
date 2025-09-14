import CoreBlockFooterDisclosures from "../components/block/footer/CoreBlockFooterDisclosures.vue";
import "@testing-library/jest-dom";
import { render } from "@testing-library/vue";

const renderComponent = async (user = {}, site = {}) => {
	return render(CoreBlockFooterDisclosures, {
		props: {
			site: {
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [],
				},
				...site,
				user,
			},
		},
	});
};

describe("CoreBlockFooterDisclosures", () => {
	test("exists", async () => {
		await renderComponent();
	});

	test("disclosure shows eagle financial states and text", async () => {
		const { container, getByText } = await renderComponent(
			{
				ddcUserData: {
					ddcLicenseData: [
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							stateCountyCode: "AR",
							licenseIdNumber: "1234",
							eagleData: "Y",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							stateCountyCode: "AL",
							licenseIdNumber: "1234",
							eagleData: "Y",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							stateCountyCode: "WA",
							licenseIdNumber: "1234",
							eagleData: "N",
						},
					],
				},
			},
			{
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [
						"<p>test mustache {{ statesEagleFinancial }} {{ eagleFinancialStateText }} stuff even works for eagle financial</p>",
					],
				},
			}
		);
		getByText(
			"test mustache AL, and AR (AR Insurance License #1234) states stuff even works for eagle financial"
		);
		expect(container).toMatchSnapshot();
	});

	test("multiple selling licenses show comma seperated states in alphabetical order, with CA and AR including license number", async () => {
		const { container, getByText } = await renderComponent(
			{
				ddcUserData: {
					regRepInd: "Y",
					ddcLicenseData: [
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							stateCountyCode: "AR",
							licenseIdNumber: "1234",
							eagleData: "Y",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							stateCountyCode: "AL",
							licenseIdNumber: "1234",
							eagleData: "Y",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "CA",
							licenseIdNumber: "CA1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "WA",
							licenseIdNumber: "WA1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "FL",
							licenseIdNumber: "FL1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "ND",
							licenseIdNumber: "ND1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "AR",
							licenseIdNumber: "AR1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "30",
							stateCountyCode: "ND",
							licenseIdNumber: "ND1234",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "30",
							stateCountyCode: "AR",
							licenseIdNumber: "AR1234",
						},
					],
				},
			},
			{
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [
						"<p>test mustache {{ statesSellingInsurance }} {{ sellingInsuranceStateText }} stuff even works for selling insurance</p>",
						"<p>test mustache {{ statesRegisteredAgent }} {{ registeredAgentStateText }} stuff even works for registered agents</p>",
						"<p>test mustache {{ statesEagleFinancial }} {{ eagleFinancialStateText }} stuff even works for eagle financial</p>",
					],
				},
			}
		);
		getByText(
			"test mustache AR (AR Insurance License #AR1234), CA (CA Insurance License #CA1234), FL, ND, and WA states stuff even works for selling insurance"
		);
		getByText(
			"test mustache AR (AR Insurance License #AR1234), and ND states stuff even works for registered agents"
		);
		getByText(
			"test mustache AL, and AR (AR Insurance License #1234) states stuff even works for eagle financial"
		);
		expect(container).toMatchSnapshot();
	});

	test("only licenses within timeframe are analyzed", async () => {
		const { container, getByText } = await renderComponent(
			{
				ddcUserData: {
					regRepInd: "Y",
					ddcLicenseData: [
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "CA",
							licenseIdNumber: "CA1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "WA",
							licenseIdNumber: "WA1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "FL",
							licenseIdNumber: "FL1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "ND",
							licenseIdNumber: "ND1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "1975-12-31",
							licenseIssueDt: "2000-12-31",
							licenseLobCode: "80",
							stateCountyCode: "AR",
							licenseIdNumber: "AR1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "30",
							stateCountyCode: "ND",
							licenseIdNumber: "ND1234",
						},
						{
							licenseExpiryDt: "1975-12-31",
							licenseIssueDt: "2000-12-31",
							licenseLobCode: "30",
							stateCountyCode: "AR",
							licenseIdNumber: "AR1234",
						},
					],
				},
			},
			{
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [
						"<p>test mustache {{ statesSellingInsurance }} {{ sellingInsuranceStateText }} stuff even works for selling insurance</p>",
						"<p>test mustache {{ statesRegisteredAgent }} {{ registeredAgentStateText }} stuff even works for registered agents</p>",
					],
				},
			}
		);
		getByText(
			"test mustache CA (CA Insurance License #CA1234), FL, ND, and WA states stuff even works for selling insurance"
		);
		getByText(
			"test mustache North Dakota state stuff even works for registered agents"
		);
		expect(container).toMatchSnapshot();
	});

	test("shows a single licensed state of CA with the full name of the state", async () => {
		const { container, getByText } = await renderComponent(
			{
				ddcUserData: {
					ddcLicenseData: [
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "CA",
							licenseIdNumber: "CA1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
					],
				},
			},
			{
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [
						"<p>test mustache {{ statesSellingInsurance }} {{ sellingInsuranceStateText }} stuff even works for selling insurance</p>",
					],
				},
			}
		);
		getByText(
			"test mustache California (California Insurance License #CA1234) state stuff even works for selling insurance"
		);
		expect(container).toMatchSnapshot();
	});

	test("a numeric state returns as Florida for registered representative and selling insurance", async () => {
		const { container, getByText } = await renderComponent(
			{
				ddcUserData: {
					regRepInd: "Y",
					ddcLicenseData: [
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "30",
							stateCountyCode: "123",
							licenseIdNumber: "1234",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "123",
							licenseIdNumber: "1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
					],
				},
			},
			{
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [
						"<p>test mustache {{ statesSellingInsurance }} {{ sellingInsuranceStateText }} stuff even works for selling insurance</p>",
						"<p>test mustache {{ statesRegisteredAgent }} {{ registeredAgentStateText }} stuff even works for registered agents</p>",
					],
				},
			}
		);
		getByText(
			"test mustache Florida state stuff even works for selling insurance"
		);
		getByText(
			"test mustache Florida state stuff even works for registered agents"
		);
		expect(container).toMatchSnapshot();
	});

	test("state text is singular if one registered license", async () => {
		const { container, getByText } = await renderComponent(
			{
				ddcUserData: {
					ddcLicenseData: [
						{
							licenseExpiryDt: "2085-12-31",
							licenseIssueDt: "2091-12-31",
							licenseLobCode: "80",
							stateCountyCode: "AR",
							licenseIdNumber: "1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
						{
							licenseExpiryDt: "2025-12-31",
							licenseIssueDt: "2021-12-31",
							licenseLobCode: "80",
							stateCountyCode: "AL",
							licenseIdNumber: "1234",
							busLicenseTpCode: "I",
							busEntityCode: "001",
						},
					],
				},
			},
			{
				footer: {
					showFooter: true,
					showDisclosures: true,
					disclosures: [
						"<p>test mustache {{ statesSellingInsurance }} {{ sellingInsuranceStateText }} stuff even works for selling insurance</p>",
					],
				},
			}
		);
		getByText(
			"test mustache Alabama state stuff even works for selling insurance"
		);
		expect(container).toMatchSnapshot();
	});
});
