const sql = require("mssql");
const config = {
	server: process.env.NYLDB_HOST,
	user: process.env.NYLDB_USERNAME,
	password: process.env.NYLDB_PASSWORD,
	database: process.env.NYLDB_DATABASE,
	requestTimeout: 30000,
};

const fn = async (userId) => {
	console.debug(`fetching user data for ${userId} from database`);

	try {
		await sql.connect(config);
		console.debug(`connection to database secured`);

		// PrefFName from user file
		// PrefLName from user file
		// Fusion92Role from user file

		// skipped this field for now
		// 			NYLAgentDetails_ADF.ProfilePageInd AS Profile_Page_OptIn_OptOut_Indicator,

		//
		// A GO user is recognized because their NYLIAM_UserDetails_ADF.Role containes partner
		// A Home Office user is recognized by their Fusion92 Role

		const responseData = {
			type: "field",
			user: null,
		};

		const userQuery = await sql.query(`

			SELECT TOP 1

			NYLIAM_UserDetails_ADF.PrefFName AS PreferredFirstName_User,
			NYLIAM_UserDetails_ADF.PrefLName AS PreferredLastName_User,
			NYLIAM_UserDetails_ADF.Fusion92Role,
			NYLIAM_UserDetails_ADF.Role,
			NYLIAM_UserDetails_ADF.LocationCode

			FROM NYLIAM_UserDetails_ADF
			WHERE NYLIAM_UserDetails_ADF.NylId='${userId}'
			ORDER BY NYLIAM_UserDetails_ADF.InsertDate DESC

		`);

		console.debug(`database query complete: user`);
		responseData.user = userQuery.recordset[0];

		const fusion92RolesThatAreHomeOffice = [
			"Content Librarian",
			"Product Owner Editor",
			"Viewer",
			"Agency Standards Viewer Reporter",
			"Copmliance Reviewer",
			"Field Editor",
			"Home Office Field User",
			"Analytics Reporter",
			"GO Website User",
		];

		if (
			fusion92RolesThatAreHomeOffice.includes(
				responseData.user.Fusion92Role
			)
		) {
			responseData.type = "corporate";
			console.debug("skipping corporate query");
		} else if (responseData.user.Role.indexOf("Partner") >= 0) {
			responseData.type = "recruiter";
			console.debug("about to do recruiter query");
			const recruiterQuery = await sql.query(`

				SELECT TOP 1

				NYLRecruiterDetails_ADF.MarketerId AS MarketerId,
				NYLRecruiterDetails_ADF.MarketerTitleTpDesc AS MarketerTitleDescription,
				NYLRecruiterDetails_ADF.OrgUnitCode AS OrgUnitCode,
				NYLRecruiterDetails_ADF.BusPhoneNum AS BusinessPhoneNumber,
				NYLRecruiterDetails_ADF.BusPhoneExtnNum AS BusinessPhoneNumberExtension,
				NYLRecruiterDetails_ADF.BusMailAddrLn1txt AS BusinessMailingAddressLine1,
				NYLRecruiterDetails_ADF.BusMailAddrLn2txt AS BusinessMailingAddressLine2,
				NYLRecruiterDetails_ADF.BusMailAddrLn3txt AS BusinessMailingAddressLine3,
				NYLRecruiterDetails_ADF.BusMailAddrCityName AS BusinessMailingAddressCity,
				NYLRecruiterDetails_ADF.BusMailAddrStateCode AS BusinessMailingAddressState,
				NYLRecruiterDetails_ADF.BusMailAddrZipCode AS BusinessMailingAddressZipCode,
				NYLRecruiterDetails_ADF.BusEmailId AS BusinessEmailId,
				NYLRecruiterDetails_ADF.EagleInd AS EagleStrategiesIndicator,
				NYLRecruiterDetails_ADF.NautilusInd AS NautilusIndicator,
				NYLRecruiterDetails_ADF.OrgUnitDesc AS OrgUnitDescription,
				NYLRecruiterDetails_ADF.MktrPrefFirstNm AS PreferredFirstName,
				NYLRecruiterDetails_ADF.MktrLglFirstNm AS LegalFirstName,
				NYLRecruiterDetails_ADF.MktrLglLastName AS LegalLastName

				FROM NYLRecruiterDetails_ADF
				
				WHERE NYLRecruiterDetails_ADF.NylId='${userId}'
				ORDER BY NYLRecruiterDetails_ADF.InsertDate DESC

			`);
			console.debug(`database query complete: recruiter`, recruiterQuery);
			responseData.recruiter = recruiterQuery.recordset[0];
		} else {
			console.debug("about to do agent query");
			const agentQuery = await sql.query(`

				SELECT TOP 1 

				NYLAgentDetails_ADF.MktrPrefFirstNm AS PreferredFirstName_Agent,
				NYLAgentDetails_ADF.MktrprefLastNm AS PreferredLastName_Agent,
				NYLAgentDetails_ADF.MktrLglFirstNm AS Legal_First_Name,
				NYLAgentDetails_ADF.MktrLglLastName AS Legal_Last_Name,
				NYLAgentDetails_ADF.BusPhoneNum AS Office_Phone_Number,
				NYLAgentDetails_ADF.BusEmailId AS Email_Address,
				NYLAgentDetails_ADF.BusLocAddrLn1tx AS Business_Address_Line_1,
				NYLAgentDetails_ADF.BusLocAddrCityNm AS Business_Address_City,
				NYLAgentDetails_ADF.BusLocAddrStateCode AS Business_Address_State,
				NYLAgentDetails_ADF.BusLocAddrZipCode AS Business_Address_Zip_Code,
				NYLAgentDetails_ADF.AgentTitleExternalDesc AS Agent_Title_Description,
				NYLAgentDetails_ADF.NylId AS NYLID,
				NYLAgentDetails_ADF.MarketerId AS Marketer_ID,
				NYLAgentDetails_ADF.EagleInd AS Eagle_Strategies_Indicator,
				NYLAgentDetails_ADF.NautilusInd AS Nautilus_Indicator,
				NYLAgentDetails_ADF.OrgUnitCode AS Org_Unit_Code,
				NYLAgentDetails_ADF.MarketerUrlTxt AS Marketer_URL,
				NYLAgentDetails_ADF.MktrDesg1code AS Agent_Designation_1_Code,
				NYLAgentDetails_ADF.Mktrdesg1yr AS Agent_Designation_1_Year,
				NYLAgentDetails_ADF.LangDesc AS Languages_Spoken,
				NYLAgentDetails_ADF.MktrNylProfileUrlTxt AS Agent_Profile_Website_URL,
				NYLAgentDetails_ADF.RegRepInd AS Registered_Representative_Indicator,
				NYLAgentDetails_ADF.MktrDetachInd AS Detached_Indicator,
				NYLAgentDetails_ADF.MarketerNewOrgInd AS New_Organization_Indicator,
				NYLAgentDetails_ADF.NylId  AS Global_Identification_Number,
				NYLAgentDetails_ADF.BizResAddressMatchInd  AS Business_and_Residential_addresses_match_indicator,
				NYLAgentDetails_ADF.ContactStatusDesc AS Contact_Status,

				NYLAgentDetails_ADF.LastUpdateDate AS Last_Update_Date,

				NYLAgentLicense_ADF.StateCountyCode,
				NYLAgentLicense_ADF.BusLicenseTpCode,
				NYLAgentLicense_ADF.BusLicenseTpDesc,
				NYLAgentLicense_ADF.LicenseTpCode,
				NYLAgentLicense_ADF.LicenseTpDescription,
				NYLAgentLicense_ADF.MultipleLicenseCntrlCode,
				NYLAgentLicense_ADF.LicenseExpiryDt,
				NYLAgentLicense_ADF.LicenseIssueDt,
				NYLAgentLicense_ADF.LicenseRenwlDt,
				NYLAgentLicense_ADF.BusEntityCode,
				NYLAgentLicense_ADF.BusEntityDesc,
				NYLAgentLicense_ADF.LicenseIdNumber,
				NYLAgentLicense_ADF.LicenseStatusCode,
				NYLAgentLicense_ADF.LicenseStatusDesc,
				NYLAgentLicense_ADF.PermLicenseIndCode,
				NYLAgentLicense_ADF.MarketerLicenseResCode,
				NYLAgentLicense_ADF.StateNonResLicenseFeeAmout,
				NYLAgentLicense_ADF.LicenseLobCode,
				NYLAgentLicense_ADF.LicenseLobDesc,
				NYLAgentLicense_ADF.MarketerStatusTpCode,
				NYLAgentLicense_ADF.MarketerStatusTpDesc,

				NYLAgentCertification_ADF.CertificationCode,
				NYLAgentCertification_ADF.CertificationDescription,
				NYLAgentCertification_ADF.CertificationEffDt,
				NYLAgentCertification_ADF.CertificationExpiryDt

				FROM NYLAgentDetails_ADF
				INNER JOIN NYLAgentCertification_ADF on NYLAgentDetails_ADF.NylId=NYLAgentCertification_ADF.NylId
				INNER JOIN NYLAgentLicense_ADF on NYLAgentDetails_ADF.NylId=NYLAgentLicense_ADF.NylId
				
				WHERE NYLAgentDetails_ADF.NylId='${userId}'
				ORDER BY NYLAgentDetails_ADF.InsertDate DESC

			`);
			console.debug(`database query complete: agent`);
			responseData.agent = agentQuery.recordset[0];
		}

		return responseData;
	} catch (err) {
		console.error(err);
		return null;
	}
};

module.exports = fn;
