using Core.CoreLib.Models.DTO.User;
using Core.CoreLib.Services.Client.NYL;
using Core.CoreLib.Services.Database.Profile;
using Core.CoreLib.Services.Database;
using Core.CoreLib.Services.Http;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CoreLib.Services.Database.User;
using Core.CoreLib.Models.Exceptions;
using Core.CoreLib.Services.User;

namespace Core.Backend.Test.Tests
{
    public class DBUserServiceTests : TestBase
    {
        private CoreLib.Services.Database.User.IUserService _dbUserService;

        public DBUserServiceTests()
        {
            var config = CreateConfig();
            CreateServices(config);

            _dbUserService =
                new CoreLib.Services.Database.User.UserService(
                    new DapperContext(config));
        }

        [Fact]
        public async Task UserMigrationInfoTests()
        {
            var data = 
                ScaffoldUserMigrationRecord();

            // Insert
            var result =
                await
                _dbUserService
                .SetUserMigrationInfo(data);

            Assert.True(result > 0);

            // Read
            var result2 =
                await
                _dbUserService
                .GetUserMigrationInfo(data.MarketerNo);

            Assert.True(result2 != null);

            // Update
            var updateData = "UpdateTest";
            data.MarketerPreferredFirstName = updateData;
            var result3 =
                await
                _dbUserService
                .SetUserMigrationInfo(data);

            Assert.True(result3 > 0);

            // Read
            var result4 =
                await
                _dbUserService
                .GetUserMigrationInfo(data.MarketerNo);

            Assert.True(result4 != null);
            Assert.True(result4.MarketerPreferredFirstName == updateData);

            // Delete
            var result5 =
                await
                _dbUserService
                .RetireUserMigrationInfo(data.MarketerNo, true);

            Assert.True(result5 > 0);

            // Validate Delete
            var ex =
                await Assert.ThrowsAsync<DataNotFoundException>(async () => await _dbUserService.GetUserMigrationInfo(data.MarketerNo));

            Assert.True(ex.Message == $"No user migration data found for marketer: {data.MarketerNo}");
        }

        private UserMigrationInfoDTO ScaffoldUserMigrationRecord() =>
            new UserMigrationInfoDTO()
            {
                MarketerNo = "0123456",
                NYLId = "appf8b99jjyXXX",
                AgentSearchEmail = "OkToDelete@test.com",
                AgentSearchMarketerId = "02345678",
                AgentTitleExternalDesc = "Some Title for this guy's account (apostrophe and paren test).  Deleting this record will cause no harm.",
                AltPhone1 = "(248)444-4444",
                AltPhone2 = "(313) 555-555",
                BusinessEmail = "Some data",
                BusinessLocAddrCityName = "Troy",
                BusinessLocAddrLn1 = "123 Fake St",
                BusinessLocAddrLn2 = "Suite 102",
                BusinessLocAddrLn3 = "3rd door on the right",
                BusinessLocAddrState = "MI",
                BusinessLocAddrZipCode = "48098",
                BusinessMailAddrCityName = "Clawson",
                BusinessMailAddrLn1 = "501 Park Dr",
                BusinessMailAddrLn2 = "",
                BusinessMailAddrLn3 = "",
                BusinessMailAddrState = "MI",
                BusinessMailAddrZipCode = "48017",
                BusinessPhoneExtNum = "1234",
                BusinessPhoneNum = "555-777-8888",
                CalendlyId = "No idea what this is",
                CellPhone = "111-222-3333",
                DBATitle = "DBA Title",
                DisplayName = "Friendly Name",
                EagleIndicator = "Y",
                FacebookUrl = "www.facebook.com/steve",
                FaxNumber = "112-333-4444",
                LinkedInUrl = "www.linkedin.com/user/4455",
                MarketerLegalFirstName = "Tester",
                MarketerLegalLastName = "AutomatedTest",
                MarketerLegalMiddleName = "OkToDelete",
                MarketerLegalSuffix = "Jr",
                MarketerPreferredFirstName = "Test",
                NautilusIndicator = "Y",
                RegisteredRepIndicator = "N",
                TwitterUrl = "https://www.twitter.com/user",
                TemplateType = "GO",
                DBAIndicator = "N",
                PrebuiltPages = "",
                Calculators = "???"
            };
    }
}
