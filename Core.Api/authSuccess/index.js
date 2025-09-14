const processSAMLResponse = require("../lib/processSAMLResponse.js");
const fetchUserData = require("../lib/fetchUserData.js");
const fn = async (context, req) => {
	const userId = await processSAMLResponse(req.body);
	let userData = null;
	if (!!userId) {
		userData = await fetchUserData(userId);
	}

	// would appreciate it
	// if everyone kept their
	// opinion to themselves
	// about what is to follow

	let html = ``;
	if (userId && userData) {
		html += `<html>
	<head>
		<title>Core2.0 SSO Test: Success</title>
		<style>
			body { font-family: sans-serif; padding: 2rem; }
			dl { margin: 0 1rem; width: 25% }
			dd { margin: 0 0 1rem; }
			hr {
				margin: 1rem 0 1.5rem;
			}
			.button {
				display: inline-flex;
				padding: 0 1rem;
				border-radius: .4rem;
				text-transform: uppercase;
				font-weight: 600;
				align-items: center;
				background: white;
				border: 1px solid rgb(25, 118, 210);
				color: rgb(25, 118, 210);
				font-size: 14px;
				height: 44px;
				cursor: pointer;
				text-decoration: none;
			}
		</style>
	</head>
	<body>
		<h1>SSO Test: Auth Succeeded</h1>
		<p>NYLID: <strong>${userId}</strong></p>
		<hr>
		<div style="display:flex">
			<dl>
				<dt><h2>User File</h2></dt>
				<dd>Preferred First Name: <br><strong>${userData.user.PreferredFirstName_User}</strong></dd>
				<dd>Preferred Last Name: <br><strong>${userData.user.PreferredLastName_User}</strong></dd>
				<dd>Fusion92 Role: <br><strong>${userData.user.Fusion92Role}</strong></dd>
				<dd>Role: <br><strong>${userData.user.Role}</strong></dd>
			</dl>`;

		if (userData.agent) {
			html += `
			<dl>
				<dt><h2>Agent Data</h2></dt>
				<dd>Preferred First Name: <br><strong>${userData.agent.PreferredFirstName_Agent}</strong></dd>
				<dd>Preferred Last Name: <br><strong>${userData.agent.PreferredLastName_Agent}</strong></dd>
				<dd>Legal First_Name: <br><strong>${userData.agent.Legal_First_Name}</strong></dd>
				<dd>Legal Last_Name: <br><strong>${userData.agent.Legal_Last_Name}</strong></dd>
				<dd>Office Phone_Number: <br><strong>${userData.agent.Office_Phone_Number}</strong></dd>
				<dd>Email Address: <br><strong>${userData.agent.Email_Address}</strong></dd>
				<dd>Business Address_Line_1: <br><strong>${userData.agent.Business_Address_Line_1}</strong></dd>
				<dd>Business Address_City: <br><strong>${userData.agent.Business_Address_City}</strong></dd>
				<dd>Business Address_State: <br><strong>${userData.agent.Business_Address_State}</strong></dd>
				<dd>Business Address_Zip_Code: <br><strong>${userData.agent.Business_Address_Zip_Code}</strong></dd>
				<dd>Agent Title_Description: <br><strong>${userData.agent.Agent_Title_Description}</strong></dd>
				<dd>NYLID: <br><strong>${userData.agent.NYLID}</strong></dd>
				<dd>Marketer ID: <br><strong>${userData.agent.Marketer_ID}</strong></dd>
				<dd>Eagle Strategies_Indicator: <br><strong>${userData.agent.Eagle_Strategies_Indicator}</strong></dd>
				<dd>Nautilus Indicator: <br><strong>${userData.agent.Nautilus_Indicator}</strong></dd>
				<dd>Org Unit Code: <br><strong>${userData.agent.Org_Unit_Code}</strong></dd>
				<dd>Marketer URL: <br><strong>${userData.agent.Marketer_URL}</strong></dd>
				<dd>Agent Designation_1_Code: <br><strong>${userData.agent.Agent_Designation_1_Code}</strong></dd>
				<dd>Agent Designation_1_Year: <br><strong>${userData.agent.Agent_Designation_1_Year}</strong></dd>
				<dd>Languages Spoken: <br><strong>${userData.agent.Languages_Spoken}</strong></dd>
				<dd>Agent Profile_Website_URL: <br><strong>${userData.agent.Agent_Profile_Website_URL}</strong></dd>
				<dd>Registered Representative_Indicator: <br><strong>${userData.agent.Registered_Representative_Indicator}</strong></dd>
				<dd>Detached Indicator: <br><strong>${userData.agent.Detached_Indicator}</strong></dd>
				<dd>New Organization_Indicator: <br><strong>${userData.agent.New_Organization_Indicator}</strong></dd>
				<dd>Global Identification_Number: <br><strong>${userData.agent.Global_Identification_Number}</strong></dd>
				<dd>Business and_Residential_addresses_match_indicator: <br><strong>${userData.agent.Business_and_Residential_addresses_match_indicator}</strong></dd>
				<dd>Contact Status: <br><strong>${userData.agent.Contact_Status}</strong></dd>
			</dl>
			<dl>
				<dt><h2>Certification Data</h2></dt>
				<dd>Certification Code: <br><strong>${userData.agent.CertificationCode}</strong></dd>
				<dd>Certification Description: <br><strong>${userData.agent.CertificationDescription}</strong></dd>
				<dd>Certification Eff Dt: <br><strong>${userData.agent.CertificationEffDt}</strong></dd>
				<dd>Certification Expiry Dt: <br><strong>${userData.agent.CertificationExpiryDt}</strong></dd>
			</dl>
			<dl>
				<dt><h2>License Data</h2></dt>
				<dd>State County Code: <br><strong>${userData.agent.StateCountyCode}</strong></dd>
				<dd>Bus License Tp Code: <br><strong>${userData.agent.BusLicenseTpCode}</strong></dd>
				<dd>Bus License Tp Desc: <br><strong>${userData.agent.BusLicenseTpDesc}</strong></dd>
				<dd>License Tp Code: <br><strong>${userData.agent.LicenseTpCode}</strong></dd>
				<dd>License Tp Description: <br><strong>${userData.agent.LicenseTpDescription}</strong></dd>
				<dd>Multiple License Cntrl Code: <br><strong>${userData.agent.MultipleLicenseCntrlCode}</strong></dd>
				<dd>License Expiry Dt: <br><strong>${userData.agent.LicenseExpiryDt}</strong></dd>
				<dd>License Issue Dt: <br><strong>${userData.agent.LicenseIssueDt}</strong></dd>
				<dd>License Renwl Dt: <br><strong>${userData.agent.LicenseRenwlDt}</strong></dd>
				<dd>Bus Entity Code: <br><strong>${userData.agent.BusEntityCode}</strong></dd>
				<dd>Bus Entity Desc: <br><strong>${userData.agent.BusEntityDesc}</strong></dd>
				<dd>License Id Number: <br><strong>${userData.agent.LicenseIdNumber}</strong></dd>
				<dd>License Status Code: <br><strong>${userData.agent.LicenseStatusCode}</strong></dd>
				<dd>License Status Desc: <br><strong>${userData.agent.LicenseStatusDesc}</strong></dd>
				<dd>Perm License Ind Code: <br><strong>${userData.agent.PermLicenseIndCode}</strong></dd>
				<dd>Marketer License Res Code: <br><strong>${userData.agent.MarketerLicenseResCode}</strong></dd>
				<dd>State Non Res License Fee Amount: <br><strong>${userData.agent.StateNonResLicenseFeeAmout}</strong></dd>
				<dd>License Lob Code: <br><strong>${userData.agent.LicenseLobCode}</strong></dd>
				<dd>License Lob Desc: <br><strong>${userData.agent.LicenseLobDesc}</strong></dd>
				<dd>Marketer Status Tp Code: <br><strong>${userData.agent.MarketerStatusTpCode}</strong></dd>
				<dd>Marketer Status Tp Desc: <br><strong>${userData.agent.MarketerStatusTpDesc}</strong></dd>
			</dl>
		`;
		} else if (userData.type === "recruiter") {
			html += `
			<dl>
				<dt><h2>Recruiter Data</h2></dt>
				<dd>MarketerId: <br><strong>${userData.recruiter.MarketerId}</strong></dd>
				<dd>MarketerTitleDescription: <br><strong>${userData.recruiter.MarketerTitleDescription}</strong></dd>
				<dd>OrgUnitCode: <br><strong>${userData.recruiter.OrgUnitCode}</strong></dd>
				<dd>BusinessPhoneNumber: <br><strong>${userData.recruiter.BusinessPhoneNumber}</strong></dd>
				<dd>BusinessPhoneNumberExtension: <br><strong>${userData.recruiter.BusinessPhoneNumberExtension}</strong></dd>
				<dd>BusinessMailingAddressLine1: <br><strong>${userData.recruiter.BusinessMailingAddressLine1}</strong></dd>
				<dd>BusinessMailingAddressLine2: <br><strong>${userData.recruiter.BusinessMailingAddressLine2}</strong></dd>
				<dd>BusinessMailingAddressLine3: <br><strong>${userData.recruiter.BusinessMailingAddressLine3}</strong></dd>
				<dd>BusinessMailingAddressCity: <br><strong>${userData.recruiter.BusinessMailingAddressCity}</strong></dd>
				<dd>BusinessMailingAddressState: <br><strong>${userData.recruiter.BusinessMailingAddressState}</strong></dd>
				<dd>BusinessMailingAddressZipCode: <br><strong>${userData.recruiter.BusinessMailingAddressZipCode}</strong></dd>
				<dd>BusinessEmailId: <br><strong>${userData.recruiter.BusinessEmailId}</strong></dd>
				<dd>EagleStrategiesIndicator: <br><strong>${userData.recruiter.EagleStrategiesIndicator}</strong></dd>
				<dd>NautilusIndicator: <br><strong>${userData.recruiter.NautilusIndicator}</strong></dd>
				<dd>OrgUnitDescription: <br><strong>${userData.recruiter.OrgUnitDescription}</strong></dd>
				<dd>PreferredFirstName: <br><strong>${userData.recruiter.PreferredFirstName}</strong></dd>
				<dd>LegalFirstName: <br><strong>${userData.recruiter.LegalFirstName}</strong></dd>
				<dd>LegalLastName: <br><strong>${userData.recruiter.LegalLastName}</strong></dd>
			</dl>
			`;
		}

		html += `
		</div>
		<hr>
		<a class="button" href="${process.env.NYLSSO_LOGOUT}">
			Sign Out From NYL Account
		</a>
	</body>
</html>
`;
	} else {
		html += `
	
<html>
	<head>
		<title>Core2.0 SSO Test: Fail</title>
		<style>
			body { font-family: sans-serif; padding: 2rem; }
			dl { margin: 0 1rem; width: 25% }
			dd { margin: 0 0 1rem; }
			hr {
				margin: 1rem 0 1.5rem;
			}
		</style>
	</head>
	<body style="font-family:sans-serif;padding:2rem;">
		<h1>SSO Test: Auth Failed</h1>
		<hr>
		<h2>Could not find user: <br><strong>${userId || "unknown"}</h2>
		<hr>
		<a class="button" href="https://www.pfed.newyorklife.com/idp/SLO.saml2">
			Sign Out From NYL Account
		</a>
	</body>
</html>
`;
	}
	context.res = {
		headers: {
			"Content-Type": "text/html",
		},
		body: html,
	};
};
module.exports = fn;
