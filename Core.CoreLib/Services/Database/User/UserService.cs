using Core.CoreLib.Extensions;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Models.Database.NYL;
using Core.CoreLib.Services.Database.Base;
using Core.CoreLib.Models.DTO.User;
using Core.CoreLib.Models.Exceptions;

namespace Core.CoreLib.Services.Database.User
{
    public class UserService : DBBase, IUserService
    {
        public UserService(
            DapperContext dapperContext) : base(dapperContext)
        { }

        public async Task<UserAccessInfo> GetRandomUser(bool prod = false)
        {
            var tableName = 
                $"dbo.NYLUserAccessInfo{(prod ? string.Empty : "_Dev")}";

            var offset = 
                new Random().Next(0, 1000);

            var result =
                await
                ExecuteQuery<UserAccessInfo>(
                    $"SELECT TOP {offset} * FROM {tableName} WHERE UserActiveFlag = 'Y' and Email is not null");

            return
                result?.FirstOrDefault() ?? new UserAccessInfo();
        }

        public async Task<UserAccessInfo> GetUserById(
            string nylId, 
            bool prod = false)
        {
            var tableName = 
                $"dbo.NYLUserAccessInfo{(prod ? string.Empty : "_Dev")}";

            var result =
                await
                ExecuteQuery<UserAccessInfo>(
                    $"SELECT TOP 1 * FROM {tableName} WHERE UserActiveFlag = 'Y' and Email is not null AND NYLID = @nylId",
                    new
                    { 
                        @nylId = nylId.ScrubForDB()
                    });

            return
                result?.FirstOrDefault() ?? new UserAccessInfo();
        }

        public async Task<UserAccessInfoDetailDDC?> GetUserAccessInfoDetailDDCData(
           string nylId,
           bool prod = false)
        {
            var tableName =
                $"Core.vw_UserAccessInfoDetailDDC{(prod ? string.Empty : "_Dev")}";

            var result =
                await
                ExecuteQuery<UserAccessInfoDetailDDC>(
                    $"SELECT * FROM {tableName} WHERE NYLID = @nylId",
                    new
                    { 
                        @nylId = nylId.ScrubForDB()
                    });

            return
                result.FirstOrDefault();
        }

        public async Task<List<UserLicenseDetailDDC>?> GetUserLicenseInfoDetailDDCData(
           string nylId,
           bool prod = false)
        {
            var tableName =
                $"Core.vw_UserLicenseDetailDDC{(prod ? string.Empty : "_Dev")}";

            var result =
                await
                ExecuteQuery<UserLicenseDetailDDC>(
                    $"SELECT * FROM {tableName} WHERE NYLID = @nylId",
                    new
                    {
                        @nylId = nylId.ScrubForDB()
                    });

            return
                result?.ToList() ?? null;
        }

        public async Task<UserAcceptance?> GetUserAcceptanceData(
           string userObjectId,
           bool prod = false)
        {
            var result =
                await
                ExecuteQuery<UserAcceptance>(
                    $"SELECT WelcomePagePresented, AcceptedTerms, Created FROM {DatabaseTables.UserAcceptance} WHERE UserObjectId = @userObjectId",
                    new
                    {
                        @userObjectId = userObjectId.ScrubForDB()
                    });

            return
                result.FirstOrDefault();
        }

        public async Task<int> SetUserAcceptanceData(
          string userId,
          string userObjectId,
          bool welcomePagePresented,
          bool acceptedTerms,
          bool prod = false)
        {
            var exists =
                await
                GetUserAcceptanceData(userObjectId) != null;

            var result =
                await
                ExecuteQuery(
                    exists ?
                        $"UPDATE {DatabaseTables.UserAcceptance} SET WelcomePagePresented = @welcomePagePresented, AcceptedTerms = @acceptedTerms WHERE userObjectId = @userObjectId" :
                        $"INSERT INTO {DatabaseTables.UserAcceptance} (WelcomePagePresented, AcceptedTerms, UserId, UserObjectId, Created) VALUES (@welcomePagePresented, @acceptedTerms, @userId, @userObjectId, GETDATE())",
                    new
                    {
                        @welcomePagePresented = welcomePagePresented ? 1 : 0,
                        @acceptedTerms = acceptedTerms ? 1 : 0,
                        @userObjectId = userObjectId.ScrubForDB(),
                        @userId = userId.ScrubForDB()
                    });

            return
                result;
        }

        public async Task<UserMigrationInfoDTO> GetUserMigrationInfo(
            string markerterNo)
        {
            var result =
                await
                ExecuteQuery<UserMigrationInfoDTO>(
                   $"SELECT * FROM {DatabaseTables.UserMigrationInfo} WHERE MarketerNo = '{markerterNo}' AND RetiredDt IS NULL");

            return
                result?.FirstOrDefault() ?? throw new DataNotFoundException($"No user migration data found for marketer: {markerterNo}");
        }

        public async Task<int> SetUserMigrationInfo(
            UserMigrationInfoDTO userMigrationInfo)
        {
            // Blindly retire, no impact if nothing is affected
            await RetireUserMigrationInfo(userMigrationInfo.MarketerNo);

            return
                await
                ExecuteQuery(
                        $"INSERT INTO {DatabaseTables.UserMigrationInfo} " +
                        $"(MarketerNo, NYLId, AgentSearchEmail, AgentSearchMarketerId, CurrentAgent, AgentTitleExternalDesc, AltPhone1, AltPhone2, BusinessEmail, BusinessLocAddrCityName, BusinessLocAddrLn1, " +
                        $"BusinessLocAddrLn2, BusinessLocAddrLn3, BusinessLocAddrState, BusinessLocAddrZipCode, BusinessMailAddrCityName, BusinessMailAddrLn1, BusinessMailAddrLn2, BusinessMailAddrLn3, " +
                        $"BusinessMailAddrState, BusinessMailAddrZipCode, BusinessPhoneExtNum, BusinessPhoneNum, CalendlyId, CellPhone, DBATitle, DisplayName, EagleIndicator, FacebookUrl, FaxNumber, " +
                        $"LinkedInUrl, MarketerLegalFirstName, MarketerLegalLastName, MarketerLegalMiddleName, MarketerLegalSuffix, MarketerPreferredFirstName, NautilusIndicator, RegisteredRepIndicator, " +
                        $"TwitterUrl, TemplateType, DBAIndicator, PrebuiltPages, Calculators, CreatedDt, RetiredDt) " +
                        $"VALUES " +
                        $"(@MarketerNo, @NYLId, @AgentSearchEmail, @AgentSearchMarketerId, NULL, @AgentTitleExternalDesc, @AltPhone1, @AltPhone2, @BusinessEmail, @BusinessLocAddrCityName, @BusinessLocAddrLn1, " +
                        $"@BusinessLocAddrLn2, @BusinessLocAddrLn3, @BusinessLocAddrState, @BusinessLocAddrZipCode, @BusinessMailAddrCityName, @BusinessMailAddrLn1, @BusinessMailAddrLn2, @BusinessMailAddrLn3, " +
                        $"@BusinessMailAddrState, @BusinessMailAddrZipCode, @BusinessPhoneExtNum, @BusinessPhoneNum, @CalendlyId, @CellPhone, @DBATitle, @DisplayName, @EagleIndicator, @FacebookUrl, @FaxNumber, " +
                        $"@LinkedInUrl, @MarketerLegalFirstName, @MarketerLegalLastName, @MarketerLegalMiddleName, @MarketerLegalSuffix, @MarketerPreferredFirstName, @NautilusIndicator, @RegisteredRepIndicator, " +
                        $"@TwitterUrl, @TemplateType, @DBAIndicator, @PrebuiltPages, @Calculators, GETDATE(), NULL)",
                    new
                    {
                        @MarketerNo = userMigrationInfo.MarketerNo.ScrubForDB(), 
                        @NYLId = userMigrationInfo.NYLId.ScrubForDB(), 
                        @AgentSearchEmail = userMigrationInfo.AgentSearchEmail.ScrubForDB(), 
                        @AgentSearchMarketerId = userMigrationInfo.AgentSearchMarketerId.ScrubForDB(), 
                        //@CurrentAgent =  userMigrationInfo.CurrentAgent, 
                        @AgentTitleExternalDesc = userMigrationInfo.AgentTitleExternalDesc.ScrubForDB(), 
                        @AltPhone1 = userMigrationInfo.AltPhone1.ScrubForDB(),
                        @AltPhone2 = userMigrationInfo.AltPhone2.ScrubForDB(),
                        @BusinessEmail = userMigrationInfo.BusinessEmail.ScrubForDB(),
                        @BusinessLocAddrCityName = userMigrationInfo.BusinessLocAddrCityName.ScrubForDB(),
                        @BusinessLocAddrLn1 = userMigrationInfo.BusinessLocAddrLn1.ScrubForDB(),
                        @BusinessLocAddrLn2 = userMigrationInfo.BusinessLocAddrLn2.ScrubForDB(),
                        @BusinessLocAddrLn3 = userMigrationInfo.BusinessLocAddrLn3.ScrubForDB(),
                        @BusinessLocAddrState = userMigrationInfo.BusinessLocAddrState.ScrubForDB(),
                        @BusinessLocAddrZipCode = userMigrationInfo.BusinessLocAddrZipCode.ScrubForDB(),
                        @BusinessMailAddrCityName = userMigrationInfo.BusinessMailAddrCityName.ScrubForDB(),
                        @BusinessMailAddrLn1 = userMigrationInfo.BusinessMailAddrLn1.ScrubForDB(),
                        @BusinessMailAddrLn2 = userMigrationInfo.BusinessMailAddrLn2.ScrubForDB(),
                        @BusinessMailAddrLn3 = userMigrationInfo.BusinessMailAddrLn3.ScrubForDB(),
                        @BusinessMailAddrState = userMigrationInfo.BusinessMailAddrState.ScrubForDB(),
                        @BusinessMailAddrZipCode = userMigrationInfo.BusinessMailAddrZipCode.ScrubForDB(),
                        @BusinessPhoneExtNum = userMigrationInfo.BusinessPhoneExtNum.ScrubForDB(),
                        @BusinessPhoneNum = userMigrationInfo.BusinessPhoneNum.ScrubForDB(),
                        @CalendlyId = userMigrationInfo.CalendlyId.ScrubForDB(),
                        @CellPhone = userMigrationInfo.CellPhone.ScrubForDB(),
                        @DBATitle = userMigrationInfo.DBATitle.ScrubForDB(),
                        @DisplayName = userMigrationInfo.DisplayName.ScrubForDB(),
                        @EagleIndicator = userMigrationInfo.EagleIndicator.ScrubForDB(),
                        @FacebookUrl = userMigrationInfo.FacebookUrl.ScrubForDB(),
                        @FaxNumber = userMigrationInfo.FaxNumber.ScrubForDB(),
                        @LinkedInUrl = userMigrationInfo.LinkedInUrl.ScrubForDB(),
                        @MarketerLegalFirstName = userMigrationInfo.MarketerLegalFirstName.ScrubForDB(),
                        @MarketerLegalLastName = userMigrationInfo.MarketerLegalLastName.ScrubForDB(),
                        @MarketerLegalMiddleName = userMigrationInfo.MarketerLegalMiddleName.ScrubForDB(),
                        @MarketerLegalSuffix = userMigrationInfo.MarketerLegalSuffix.ScrubForDB(),
                        @MarketerPreferredFirstName = userMigrationInfo.MarketerPreferredFirstName.ScrubForDB(),
                        @NautilusIndicator = userMigrationInfo.NautilusIndicator.ScrubForDB(),
                        @RegisteredRepIndicator = userMigrationInfo.RegisteredRepIndicator.ScrubForDB(),
                        @TwitterUrl = userMigrationInfo.TwitterUrl.ScrubForDB(),
                        @TemplateType = userMigrationInfo.TemplateType.ScrubForDB(),
                        @DBAIndicator = userMigrationInfo.DBAIndicator.ScrubForDB(),
                        @PrebuiltPages = userMigrationInfo.PrebuiltPages.ScrubForDB(),
                        @Calculators = userMigrationInfo.Calculators.ScrubForDB()
                    });
        }

        public async Task<int> RetireUserMigrationInfo(string markerterNo, bool hardDelete = false) =>
            await
            ExecuteQuery(
                hardDelete ?
                    $"DELETE FROM {DatabaseTables.UserMigrationInfo} WHERE MarketerNo = '{markerterNo}'" :
                    $"UPDATE {DatabaseTables.UserMigrationInfo} SET RetiredDt = GETDATE() WHERE MarketerNo = '{markerterNo}'");
    }
}