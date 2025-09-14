
using Core.Backend.Models.Web;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Database.Core;

namespace Core.Backend.Extensions
{
    public static class UserExtension
    {
        public static NYLUser ToWebNYLUser(this ADUser adUser) =>
            ConvertToNYLUser(adUser);

        public static NYLUser ToWebNYLUser(this ADUser adUser, NYLUser OBOUser) =>
            ConvertToNYLUser(adUser, null, OBOUser);

        public static NYLUser ToWebNYLUser(this ADUser adUser, UserAccessInfoDetailDDC ddcUser) =>
            ConvertToNYLUser(adUser, ddcUser, null);

        public static NYLUser ToWebNYLUser(this ADUser adUser, UserAccessInfoDetailDDC ddcUser, NYLUser OBOUser) =>
            ConvertToNYLUser(adUser, ddcUser, OBOUser);

        public static NYLUser ToWebNYLUser(this ADUser adUser, UserAcceptance userAcceptance) =>
            ConvertToNYLUser(adUser, null, null, userAcceptance);

        public static NYLUser ToWebNYLUser(this ADUser adUser, UserAcceptance userAcceptance, UserAccessInfoDetailDDC ddcUser) =>
            ConvertToNYLUser(adUser, ddcUser, null, userAcceptance);

        private static NYLUser ConvertToNYLUser(this ADUser adUser, UserAccessInfoDetailDDC? ddcUser = null, NYLUser? OBOUser = null, UserAcceptance? userAcceptance = null) =>
            new NYLUser
            {
                AccountEnabled = adUser.GraphUserData?.AccountEnabled ?? false,
                BrandId = adUser.BrandId,
                City = adUser.GraphUserData?.City ?? string.Empty,
                Country = adUser.GraphUserData?.Country ?? string.Empty,
                EligiblePersonalizedWebsite = adUser.EligibleForPersonalizedWebsite,
                EmployeeId = adUser.GraphUserData?.EmployeeId ?? string.Empty,
                EmployeeType = adUser.GraphUserData?.EmployeeType ?? string.Empty,
                FirstName = adUser.GraphUserData?.GivenName ?? string.Empty,
                HasWebsiteAgent = adUser.HasPersonalizedWebsiteAgent,
                HasWebsiteRecruiter = adUser.HasPersonalizedWebsiteRecruiter,
                LastName = adUser.GraphUserData?.Surname ?? string.Empty,
                Mail = adUser.GraphUserData?.Mail ?? string.Empty,
                MobilePhone = adUser.GraphUserData?.MobilePhone ?? string.Empty,
                Permissions = adUser.Permissions,
                PostalCode = adUser.GraphUserData?.PostalCode ?? string.Empty,
                SSOId = adUser.SSOId,
                State = adUser.GraphUserData?.State ?? string.Empty,
                StreetAddress = adUser.GraphUserData?.StreetAddress ?? string.Empty,
                OBONYLUser = OBOUser ?? new NYLUser(),
                IsOBOSession = OBOUser != null,
                WelcomePagePresented = userAcceptance != null ? userAcceptance.WelcomePagePresented : false,
                AcceptedTerms = userAcceptance != null ? userAcceptance.AcceptedTerms : false,
                EagleAdvisor = adUser.EagleAdvisor,
                Nautilus = adUser.Nautilus,
                RegisteredRep = adUser.RegisteredRep,
                ApprovedDBA = adUser.ApprovedDBA,
                LongTermCare =adUser.LongTermCare,
                AARP = adUser.AARP,
                DDCUserData =
                    ddcUser != null ?
                        new DDCUserData()
                        { 
                            AgentTitleExternalDesc = ddcUser.AgentTitleExternalDesc,
                            BusEmailId = ddcUser.BusEmailId,
                            BusLocAddrCityNm = ddcUser.BusLocAddrCityNm,
                            BusLocAddrLn1tx =  ddcUser.BusLocAddrLn1tx,
                            BusLocAddrLn2Tx =  ddcUser.BusLocAddrLn2Tx,
                            BusLocAddrLn3Tx = ddcUser.BusLocAddrLn3Tx,
                            BusLocAddrStateCode = ddcUser.BusLocAddrStateCode,
                            BusLocAddrZipCode = ddcUser.BusLocAddrZipCode,
                            BusMailAddrCityName = ddcUser.BusMailAddrCityName,
                            BusMailAddrLn1txt = ddcUser.BusMailAddrLn1txt,
                            BusMailAddrLn2txt = ddcUser.BusMailAddrLn2txt,
                            BusMailAddrLn3txt = ddcUser.BusMailAddrLn3txt,
                            BusMailAddrStateCode = ddcUser.BusMailAddrStateCode,
                            BusMailAddrZipCode = ddcUser.BusMailAddrZipCode,
                            BusPhoneExtnNum = ddcUser.BusPhoneExtnNum,
                            BusPhoneNum= ddcUser.BusPhoneNum,
                            CellPhoneNum= ddcUser.CellPhoneNum,
                            Dba1EmailAddrTxt = ddcUser.Dba1EmailAddrTxt,
                            Dba1LogoTxt = ddcUser.Dba1LogoTxt,
                            Dba2EmailAddrTxt = ddcUser.Dba2EmailAddrTxt,
                            Dba2LogoTxt = ddcUser.Dba2LogoTxt,
                            Dba3EmailAddrTxt = ddcUser.Dba3EmailAddrTxt,
                            Dba3LogoTxt = ddcUser.Dba3LogoTxt,
                            Dba4EmailAddrTxt = ddcUser.Dba4EmailAddrTxt,
                            Dba4LogoTxt = ddcUser.Dba4LogoTxt,
                            Dba5EmailAddrTxt = ddcUser.Dba5EmailAddrTxt,
                            Dba5LogoTxt = ddcUser.Dba5LogoTxt,
                            DBATitle = ddcUser.DBATitle,
                            DDCLicenseData = 
                                (ddcUser.UserLicensesDetailDDC ?? new List<UserLicenseDetailDDC>())
                                .Select(s=> 
                                    new DDCLicenseData() 
                                    {
                                        BusLicenseTpCode = s.BusLicenseTpCode,
                                        BusEntityCode = s.BusEntityCode,
                                        LicenseExpiryDt = s.LicenseExpiryDt,
                                        LicenseIdNumber = s.LicenseIdNumber,
                                        LicenseIssueDt = s.LicenseIssueDt,
                                        LicenseLobCode = s.LicenseLobCode,
                                        LicenseTpCode = s.LicenseTpCode,
                                        LicenseTpDescription = s.LicenseTpDescription,
                                        MarketerStatusTpCode = s.MarketerStatusTpCode,
                                        MarketerStatusTpDesc = s.MarketerStatusTpDesc,
                                        StateCountyCode = s.StateCountyCode,
                                        AgentData = s.AgentData,
                                        EagleData =s.EagleData
                                    })
                            .ToList(),
                            EagleInd = ddcUser.EagleInd,
                            FacebookUrlTxt = ddcUser.FacebookUrlTxt,
                            FaxNumber= ddcUser.FaxNumber,
                            LnkdinUrlTxt = ddcUser.LnkdinUrlTxt,
                            MarketerNo = ddcUser.MarketerNo,
                            MarketerTitleTpDesc = ddcUser.MarketerTitleTpDesc,
                            MDRTLastYear = ddcUser.MDRTLastYear,
                            MktrLglFirstNm = ddcUser.MktrLglFirstNm,
                            MktrLglLastName = ddcUser.MktrLglLastName,
                            MktrLglMiddleNm = ddcUser.MktrLglMiddleNm,
                            MktrLglSfxCode = ddcUser.MktrLglSfxCode,
                            MktrPrefFirstNm = ddcUser.MktrPrefFirstNm,
                            NautilusInd = ddcUser.NautilusInd,
                            PhotoURLProfile = ddcUser.PhotoURLProfile,
                            RegRepInd = ddcUser.RegRepInd,
                            TwitterUrlTxt = ddcUser.TwitterUrlTxt,
                            OrgUnitDesc = ddcUser.OrgUnitDesc,
                            OrgUnitCode = ddcUser.OrgUnitCode
                        } :
                        null!
            };
    }
}