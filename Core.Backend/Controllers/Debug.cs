using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Core.CoreLib.Models.DTO.LeadForm;
using Core.CoreLib.Services.Azure.ActiveDirectory;
using Core.CoreLib.Services.Azure.ServiceBus;
using Core.CoreLib.Services.Client.NYL;
using Core.CoreLib.Services.User;
using Core.CoreLib.Services.Security;
using CoreBackend.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using static Core.CoreLib.Models.Constants.Authorization;
using Core.Backend.Extensions;
using Core.CoreLib.Extensions;
using Core.CoreLib.Models.Azure.ServiceBus;
using System.Text.Json;
using Core.CoreLib.Services.Database.Site;
using Core.CoreLib.Models.Constants;
using Core.CoreLib.Services.Database.User;
using Core.Backend.Models.Web;
using Core.CoreLib.Models.Azure.ActiveDirectory;
using Core.CoreLib.Models.Database.Core;
using Microsoft.Azure.Amqp.Framing;
using Core.CoreLib.Models.DTO.User;
using Core.CoreLib.Models.DTO.Profile;
using Dba = Core.CoreLib.Models.DTO.Profile.Dba;
using Core.CoreLib.Models.DTO.Site;
using Core.CoreLib.Models.DTO.Library;
using Microsoft.Extensions.Azure;
using Core.CoreLib.Services.Database.Library;
using Core.CoreLib.Models.DTO.Authentication;
using Microsoft.IdentityModel.Tokens;
using Core.CoreLib.Services.Database.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Graph;
using System.Text;
using Core.CoreLib.Models.SAML;
using Core.Backend.Controllers.Attributes;
using Core.CoreLib.Services.Http;
using System.Security.Policy;

namespace Core.Backend.Controllers
{
    [Route("api/users")]
    [ApiController]
    public sealed class DebugController : ControllerBaseTokenized
    {
        private readonly CoreLib.Services.User.IUserService _userService;
        private readonly CoreLib.Services.Database.User.IUserService _dbUserService;
        private readonly IMessageSender _SBService;
        private readonly IPasswordService _pwService;
        private readonly ITokenExploder _tokenExploder;
        private readonly IActiveDirectoryService _adService;
        private readonly ILeadFormService _leadFormService;
        private readonly IConfiguration _config;
        private readonly IServiceBusTopicSubscription _sbTopicManager;
        private readonly ILogger<Controller> _logger;
        private readonly ISiteService _siteService;
        private readonly ILibraryService _libraryService;
        private readonly IProfileService _profileService;
        private readonly ITokenService _tokenService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticationService _authRepo;

        public DebugController(
            IConfiguration config,
            CoreLib.Services.User.IUserService userService,
            CoreLib.Services.Database.User.IUserService dbUserService,
            IMessageSender sbService,
            IPasswordService pwService,
            ITokenExploder tokenExploder,
            IActiveDirectoryService adService,
            ILeadFormService leadFormService,
            IServiceBusTopicSubscription sbTopicManager,
            ILogger<Controller> logger,
            ISiteService siteService,
            ILibraryService libraryService,
            IProfileService profileService,
            ITokenService tokenService,
            IHttpClientFactory httpClientFactory,
            IAuthenticationService authRepo)
        {
            _config = config;
            _userService = userService;
            _dbUserService = dbUserService;
            _SBService = sbService;
            _pwService = pwService;
            _tokenExploder = tokenExploder;
            _adService = adService;
            _leadFormService = leadFormService;
            _sbTopicManager = sbTopicManager;
            _logger = logger;
            _siteService = siteService;
            _libraryService = libraryService;
            _profileService = profileService;
            _tokenService = tokenService;
            _httpClientFactory = httpClientFactory;
            _authRepo = authRepo;
        }

        [HttpGet("ServerUp")]
        //[ClaimRequirement(new string[] { Permissions.ReadContent })]
        public IActionResult ServerUp()
        {
            try
            {
                return Ok("Up");
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpPost("GenerateAuthToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateAuthToken(AuthLoginDTO payload)
        {
            try
            {
                if (payload != null &&
                    !string.IsNullOrWhiteSpace(payload.Username) &&
                    !string.IsNullOrWhiteSpace(payload.Password))
                {
                    var user =
                        await
                        _userService
                        .GetADUserAsync(
                            payload.Username);

                    if (user == null)
                        return BadRequest("User not found in system");

                    try
                    {
                        await
                        _authRepo
                        .AuthenticateUser(user, payload.Password);
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }

                    var token =
                        await
                        _authRepo
                        .GenerateAuthTokenDTO(user);

                    return
                        Ok(token.Token);
                }
                else
                {
                    return BadRequest("Payload empty or missing data");
                }
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("GetUserId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserId(string userEmailAddress)
        {
            try
            {
                if (!Request.Host.Host.ToLower().Contains("localhost"))
                    throw new Exception("This feature only available in debug mode to developers");

                var user =
                    await
                    _userService
                    .GetADUserAsync(userEmailAddress);

                return
                    Ok(user.GraphUserData.Id);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ShowUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowUser(string userEmailAddress)
        {
            try
            {
                //  var xxx = 
                //     _tokenExploder.ExplodeToken(NYLSSOTokenExploderService.EncodedSampleToken);

                // Verfiy hashed pass
                //var foo =
                //    _pwService.GeneratePassword("eashworth@ft.newyorklife.com");

                //var match =
                //    _pwService.VerifyHashedPassword(foo, "J.McJohnerson@fusion92.com");
                ///////////

                var user =
                    await
                    _userService
                    .GetADUserAsync(userEmailAddress);

                var acceptanceData =
                    await
                    _dbUserService.GetUserAcceptanceData(user.GraphUserData.Id);

                return
                    acceptanceData != null ?
                        Ok(user.ToWebNYLUser(acceptanceData)) :
                        Ok(user.ToWebNYLUser());
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ShowSSOUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowSSOUser(string employeeId)
        {
            try
            {
                var user =
                    await 
                    _userService
                    .GetSSOADUserAsync(employeeId);

                var acceptanceData =
                    await
                    _dbUserService.GetUserAcceptanceData(user.GraphUserData.Id);

                var detail =
                    await
                    _dbUserService.GetUserAccessInfoDetailDDCData(employeeId);

                if (detail != null)
                    detail.UserLicensesDetailDDC =
                       await
                       _dbUserService.GetUserLicenseInfoDetailDDCData(detail.NYLId);

                return
                    Ok(user.ToWebNYLUser(acceptanceData!, detail!));
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ShowOBOUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ShowOBOUser()
        {
            try
            {
                var user =
                    await _userService.GetSSOADUserAsync("ap00p060dpr2t55p");

                if (user == null)
                    throw new Exception("User Id not found");

                var oboUser =
                    await _userService.GetSSOADUserAsync("ap00p060dbs98lff");

                if (oboUser == null)
                    throw new Exception("OBO session detected, OBO user not found: ap00p060dbs98lff");

                return Ok(user.ToWebNYLUser(oboUser.ToWebNYLUser()));
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ResetAcceptance")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetAcceptance(string userEmailAddress)
        {
            try
            {
                var user =
                    await _userService.GetADUserAsync(userEmailAddress);

                if (user == null)
                    throw new Exception("User Id not found");

                var result =
                    await
                    _dbUserService.SetUserAcceptanceData(
                        user.GraphUserData.Mail,
                        user.GraphUserData.Id,
                        false,
                        false);

                return Ok($"{result} row upated");
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("DBUserMatchesADUser")]
        [AllowAnonymous]
        public async Task<IActionResult> DBUserMatchesADUser(string employeeId)
        {
            try
            {
                // Get DB user
                var dbUser =
                    await
                   _dbUserService
                    .GetUserById(
                       employeeId,
                       (_config["ASPNETCORE_ENVIRONMENT"] ?? "").ToLower() == "prod");

                if (dbUser == null || string.IsNullOrEmpty(dbUser.NYLId))
                    throw new Exception($"No user matching NYLID: {employeeId} found in NYLAccessInfo.  Test performed according to environment, dev/staging = NYLAccessInfo_Dev, prod = NYLAccessInfo");

                // Get AD user
                var user =
                    await
                    _userService
                    .GetSSOADUserAsync(dbUser.NYLId);

                if (user == null)
                    throw new Exception($"No user matching NYLID: {employeeId} found in Active Directory.");

                var message = string.Empty;

                if (dbUser.IsGO.HasValue && dbUser.IsGO.Value && user.GraphUserData.EmployeeType.ToLower() != EmployeeType.GeneralOffice)
                    message += $"GO user in DB has EmployeeType: {user.GraphUserData.EmployeeType.ToLower()}.\r\n";

                if (dbUser.IsHO.HasValue && dbUser.IsHO.Value && user.GraphUserData.EmployeeType.ToLower() != EmployeeType.HomeOffice)
                    message += $"HO user in DB has EmployeeType: {user.GraphUserData.EmployeeType.ToLower()}.\r\n";

                if (dbUser.IsOBO.HasValue && dbUser.IsOBO.Value && user.GraphUserData.EmployeeType.ToLower() != EmployeeType.OnBehalfOf)
                    message += $"OBO user in DB has EmployeeType: {user.GraphUserData.EmployeeType.ToLower()}.\r\n";

                if (dbUser.IsAgent.HasValue && dbUser.IsAgent.Value && user.GraphUserData.EmployeeType.ToLower() != EmployeeType.Agent)
                    message += $"Agent user in DB has EmployeeType: {user.GraphUserData.EmployeeType.ToLower()}.\r\n";

                if ((dbUser.HasPersonalizedWebsiteAgent ?? false) != user.HasPersonalizedWebsiteAgent)
                    message += $"HasPersonalizedWebsiteAgent user in DB: {dbUser.HasPersonalizedWebsiteAgent ?? false} has AD value: HasPersonalizedWebsiteAgent = {user.HasPersonalizedWebsiteAgent}.\r\n";

                if ((dbUser.HasPersonalizedWebsiteRecruiter ?? false) != user.HasPersonalizedWebsiteRecruiter)
                    message += $"HasPersonalizedWebsiteAgent user in DB: {dbUser.HasPersonalizedWebsiteRecruiter ?? false} has AD value: HasPersonalizedWebsiteAgent = {user.HasPersonalizedWebsiteRecruiter}.\r\n";

                if ((dbUser.EligibleForPersonalizedWebsite ?? false) != user.EligibleForPersonalizedWebsite)
                    message += $"HasPersonalizedWebsiteAgent user in DB: {dbUser.EligibleForPersonalizedWebsite ?? false} has AD value: HasPersonalizedWebsiteAgent = {user.EligibleForPersonalizedWebsite}.\r\n";

                if ((dbUser.EagleInd ?? false) != user.EagleAdvisor)
                    message += $"EagleInd user in DB: {dbUser.EagleInd ?? false} has AD value: EagleInd = {user.EagleAdvisor}.\r\n";

                if ((dbUser.NautilusInd ?? false) != user.Nautilus)
                    message += $"NautilusInd user in DB: {dbUser.NautilusInd ?? false} has AD value: NautilusInd = {user.Nautilus}.\r\n";

                if ((dbUser.RegisteredRep ?? false) != user.RegisteredRep)
                    message += $"RegisteredRep user in DB: {dbUser.RegisteredRep ?? false} has AD value: RegisteredRep = {user.RegisteredRep}.\r\n";

                if ((dbUser.DBAInd ?? false) != user.ApprovedDBA)
                    message += $"DBAInd user in DB: {dbUser.DBAInd ?? false} has AD value: DBAInd = {user.ApprovedDBA}.\r\n";

                if ((dbUser.LTCInd ?? false) != user.LongTermCare)
                    message += $"LTCInd user in DB: {dbUser.LTCInd ?? false} has AD value: LTCInd = {user.LongTermCare}.\r\n";

                if ((dbUser.AARPInd ?? false) != user.AARP)
                    message += $"AARPInd user in DB: {dbUser.AARPInd ?? false} has AD value: AARPInd = {user.AARP}.\r\n";

                return Ok(
                    new 
                    { 
                        Result = 
                        string.IsNullOrWhiteSpace(message) ? 
                            "Match!" : 
                            $"DOES NOT MATCH: {message}",
                        DBUser =
                            new
                            { 
                                IsGo = dbUser.IsGO.HasValue && dbUser.IsGO.Value,
                                IsHO = dbUser.IsHO.HasValue && dbUser.IsHO.Value,
                                IsOBO = dbUser.IsOBO.HasValue && dbUser.IsOBO.Value,
                                IsAgent = dbUser.IsAgent.HasValue && dbUser.IsAgent.Value,
                                HasPersonalizedWebsiteAgent = dbUser.HasPersonalizedWebsiteAgent ?? false,
                                HasPersonalizedWebsiteRecruiter = dbUser.HasPersonalizedWebsiteRecruiter ?? false,
                                EligibleForPersonalizedWebsite = dbUser.EligibleForPersonalizedWebsite ?? false,
                                EagleAdvisor = dbUser.EagleInd ?? false,
                                Nautilus = dbUser.NautilusInd ?? false,
                                RegisteredRep = dbUser.RegisteredRep ?? false,
                                ApprovedDBA = dbUser.DBAInd ?? false,
                                LongTermCare = dbUser.LTCInd ?? false,
                                AARP = dbUser.AARPInd ?? false
                            },
                        ADUser = 
                            new
                            { 
                                EmployeeType = user.GraphUserData.EmployeeType,
                                HasPersonalizedWebsiteAgent = user.HasPersonalizedWebsiteAgent,
                                HasPersonalizedWebsiteRecruiter = user.HasPersonalizedWebsiteRecruiter,
                                EligibleForPersonalizedWebsite = user.EligibleForPersonalizedWebsite,
                                EagleAdvisor = user.EagleAdvisor,
                                Nautilus = user.Nautilus,
                                RegisteredRep = user.RegisteredRep,
                                ApprovedDBA = user.ApprovedDBA,
                                LongTermCare = user.LongTermCare,
                                AARP = user.AARP
                            }
                    });
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("Environment")]
        [AllowAnonymous]
        public IActionResult Environment()
        {
            try
            {
                return Ok(
                    _config["ASPNETCORE_ENVIRONMENT"]);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("DisplayConfigSettings")]
        [AllowAnonymous]
        public IActionResult DisplayConfigSettings()
        {
            try
            {
                return Ok(
                    _config.AsEnumerable().OrderBy(o => o.Key).ToList());
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("DeadLetterReport")]
        [AllowAnonymous]
        public async Task<IActionResult> DeadLetterReport()
        {
            try
            {
                return Ok(
                    await
                    _sbTopicManager.GetTopicDeadLetterCount());
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ThrowException")]
        [AllowAnonymous]
        public IActionResult ThrowException(string exceptionMessage)
        {
            try
            {
                throw new Exception(exceptionMessage);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("LeadFormSubmitionRaw")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitLeadFormRawTest()
        {
            try
            {
                if (!(_config["ASPNETCORE_ENVIRONMENT"] ?? string.Empty).ToLower().StartsWith("dev"))
                    throw new Exception("Only available in development.");

                //var originalTestData =
                //    @"{""leadSource"":""FUSION"",""leadProcessType"":""agentPersWeb"",""referenceNumber"":"""",""firstName"": ""test"",""lastName"": ""test"",""address"": ""135 South 84th Street"",""city"": ""Milwaukee"",""state"": ""WI"",""zip"": ""53214"",""phoneNumber"": 9088494549,""emailAddress"": ""test@newyorklife.com"",""birthDate"": ""1975-08-29T04:00:00.000+0000"",""dateSubmitted"": ""2017-03-31T14:25:03.827+0000"",""marketerNumber"": ""0380246"",""sourceCode"": ""1Z7H4F"",""sourceCodeName"": ""Source Code 01"",""sourceCodeStartDate"": ""2020-07-21T04:00:00.000+0000"",""sourceCodeComments"": """",""sourceCodeEmailAddress"": ""test@newyorklife.com"",""campaignCode"": ""UL0142"",""campaignName"": ""Internet"",""campaignProgramCode"": ""A2"",""giftType"": ""none"",""pageUrl"": ""http://www.google.com"",""linkedinUrl"":""www.linkedin.com/user/testalexz"",""languagePref"":""hindi"",""bestTimeToCall"":""morning"",""leadConcerns"":""Lead Concerns"",""orgUnitCD"":""a99""}";

                var formData =
                    @"{""firstName"":""CAROL L"",""lastName"":""WILLIAMS"",""address"":""E74 S MUNN AVE APT 5A"",""city"":""JEAST ORANGE"",""state"":""NJ"",""zip"":""07018"",""emailAddress"":""saimanisha_lakkaraju@newyorklife.com"",""phoneNumber"":""9736094324"",""bestTimeToCall"":""Morning"",""sourceCode"":""4M5B6S"",""marketerNumber"":""0380246"",""goCode"":""1234?"",""birthDate"":""1975-08-29T04:00:00.000+0000"",""sourceCodeName"":""Source Code 01"",""sourceCodeStartDate"":""2017-03-13T04:00:00.000+0000"",""sourceCodeComments"":""Lead comments"",""sourceCodeEmailAddress"":""agnt007@go.com"",""campaignCode"":""PGW668"",""campaignName"":""Campaign 02"",""campaignProgramCode"":""p2"",""giftType"":""none"",""pageUrl"":""http://GO_personal_web_site_url"",""linkedinUrl"":""www.linkedin.com/user/"",""languagePref"":""Korean"",""leadConcerns"":""Life Insurance"",""leadProcessType"":""goWeb"",""orgUnitCD"":""S98"",""metaData"":[{""key"":""how many kids?"",""value"":""3""},{""key"":""planned retirement year"",""value"":""2033""}],""referenceNumber"":"""",""dateSubmitted"":""2022-03-31T14:25:03.827+0000"",""leadSource"":""FUSION"",""lastModifiedBy"":""T15K9S8"",""specialLeadIn"":""N"",""contactMethod"":""Email""}";

                var response =
                    await
                    _leadFormService
                    .SubmitNYLCLTLead(formData);

                return Ok(
                    new
                    {
                        Response = response
                    });
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("LeadFormSubmitionDTO")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitLeadFormDTOTest()
        {
            try
            {
                if (!(_config["ASPNETCORE_ENVIRONMENT"] ?? string.Empty).ToLower().StartsWith("dev"))
                    throw new Exception("Only available in development.");

                // Updater per NYL 3/1/2023, this is approved by them as legit payload
                var formData =
                    new SubmitLeadFormDTO()
                    {
                        Address = "E74 S MUNN AVE APT 5A",
                        City = "JEAST ORANGE",
                        State = "NJ",
                        EmailAddress = "saimanisha_lakkaraju@newyorklife.com",
                        FirstName = "CAROL L",
                        LastName = "WILLIAMS",
                        //GoCode = "1234?",
                        Zip = "07018",
                        ReferenceNumber = "",
                        PhoneNumber = "9736094324",
                        MarketerNumber = "0380246",
                        SourceCode = "4M5B6S",
                        SourceCodeStartDate = "2017-03-13T04:00:00.000+0000",
                        SourceCodeComments = "Lead comments",
                        CampaignCode = "PGW668",
                        CampaignName = "Campaign 02",
                        CampaignProgramCode = "p2",
                        SourceCodeName = "Source Code 01",
                        SourceCodeEmailAddress = "agnt007@go.com",
                        GiftType = "none",
                        BirthDate = "1975-08-29T04:00:00.000+0000",
                        DateSubmitted = "2022-03-31T14:25:03.827+0000",
                        PageUrl = "http://GO_personal_web_site_url",
                        LinkedinUrl = "www.linkedin.com/user/",
                        LanguagePref = "Korean",
                        BestTimeToCall = "Morning",
                        LeadConcerns = "Life Insurance",
                        LeadProcessType = "goWeb",
                        LeadSource = "FUSION",
                        LastModifiedBy = "T15K9S8",
                        //SpecialLeadIn = "N",
                        OrgUnitCD = "S98",
                        ContactMethod = "Email",
                        MetaData =
                            new List<MetaDataItem>()
                            {
                                new MetaDataItem() { Key = "how many kids?", Value = "3" },
                                new MetaDataItem() { Key = "planned retirement year", Value = "2033" }
                                //new MetaDataItem() { Key = "protection", Value = "Protection" },
                                //new MetaDataItem() { Key = "retirementplanning", Value = "Retirement Planning" },
                                //new MetaDataItem() { Key = "wealthaccumulation", Value = "Wealth Accumulation" },
                                //new MetaDataItem() { Key = "estateplanning", Value = "Estate Planning" },
                                //new MetaDataItem() { Key = "smallbusinessadvisory", Value = "Small Business Advisory" }
                            }
                    };

                var payload =
                    JsonConvert.SerializeObject(
                        formData,
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            NullValueHandling = NullValueHandling.Ignore
                        });

                var response =
                    await
                    _leadFormService
                    .SubmitNYLCLTLead(payload);

                return Ok(
                    new
                    {
                        Response = response
                    });
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("DBASubmitionRaw")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitDBARawTest()
        {
            try
            {
                if (!(_config["ASPNETCORE_ENVIRONMENT"] ?? string.Empty).ToLower().StartsWith("dev"))
                    throw new Exception("Only available in development.");

                // Need to get this string in proper format
                //var formData =
                //    @"{""smruApprovalDate"":""2023-03-02T20:08:11.4394384Z"",""dba"":{""name"":""Test Name"",""displayName"":""Test Display Name"",""logoCreatedDate"":""2023-03-02T20:08:11.4396384Z"",""image"":{""type"":""image/png"",""data"":""iVBORw0KGgoAAAANSUhEUgAAAsoAAAKMBAMAAAAAuYMCAAAAGFBMVEX/////""}},""webSite"":{""url"":""https://somewebsite.com"",""urlGuid"":""2488d300-2ead-11ed-b41d-0e76bd731e63"",""status"":""active"",""createdDate"":""2023-03-02T20:08:11.4401667Z"",""lastModifiedDate"":""2023-03-02T20:08:11.440309Z"",""owner"":""Test Owner""},""marketers"":[{""marketerId"":""0845357"",""optInInd"":false,""title"":""Test Title"",""prefEmailAddress"":""some_email@newyorklife.com"",""isPrimary"":true},{""marketerId"":""0084621"",""optInInd"":false,""title"":""Test Title"",""prefEmailAddress"":""some_email@newyorklife.com"",""isPrimary"":false}]}";

                //var response =
                //    await
                //    _leadFormService
                //    .SubmitNYLCLTLead(formData);

                return Ok(
                    new
                    {
                        Response = "Under Construction, use DBASubmitionDTO"
                    });
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("DBASubmitionDTO")]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitDBADTOTest()
        {
            try
            {
                if (!(_config["ASPNETCORE_ENVIRONMENT"] ?? string.Empty).ToLower().StartsWith("dev"))
                    throw new Exception("Only available in development.");

                var dbaData =
                    new SubmitDbaDTO()

                    {  
                        SmruApprovalDate = DateTime.UtcNow,
                        Dba =
                            new Dba()
                            {
                                DisplayName = "JOHN P. DUDA INSURANCE AND FINANCIAL SERVICES",
                                Name = "JOHN P. DUDA INSURANCE AND FINANCIAL SERVICES",
                                LogoCreatedDate = DateTime.UtcNow,
                                Image = new CoreLib.Models.DTO.Profile.Image
                                {
                                    Data = "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",
                                    Type = "image/png"
                                }
                            },
                        WebSite =
                            new WebSite()
                            {
                                UrlGuid = Guid.NewGuid().ToString(),//"2488d300-2ead-11ed-b41d-0e76bd731e63",
                                Owner = "Test Owner",
                                Status = "active",
                                Url = "https://www.johnpduda.com",
                                CreatedDate = DateTime.UtcNow,
                                LastModifiedDate = DateTime.UtcNow
                            },
                        Marketers =
                            new List<Marketer>()
                            {
                                new Marketer()
                                {
                                    MarketerId = "0328113",
                                    OptInInd = false,
                                    Title = "Agent",
                                    PrefEmailAddress = "jduda@ft.newyorklife.com",
                                    IsPrimary = true
                                }//,
                                //new Marketer()
                                //{
                                //    MarketerId = "0084621",
                                //    OptInInd = false,
                                //    Title = "Test Title",
                                //    PrefEmailAddress = "some_email@newyorklife.com",
                                //    IsPrimary = true
                                //}
                            }
                    };

                var response =
                    await
                    _profileService
                    .SubmitDba(dbaData);

                var responseContent = await response.Content.ReadAsStringAsync();

                return
                    StatusCode((int)response.StatusCode, responseContent);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("CertDump")]
        [AllowAnonymous]
        public IActionResult CertDump()
        {
            try
            {
                var vaultUrl = _config["KeyVault:AzureKeyVaultEndpoint"];
                var clientId = _config["KeyVault:AZURE_CLIENT_ID"];
                var tenantId = _config["KeyVault:AZURE_TENANT_ID"];
                var clientIdSecret = _config["KeyVault:AZURE_CLIENT_SECRET"];
                var certificateName = _config["NYL:CertificateName"];

                if (string.IsNullOrWhiteSpace(vaultUrl))
                    throw new Exception("KeyValult endpoint config value undefined");

                var credentials = new ClientSecretCredential(tenantId, clientId, clientIdSecret);
                var certificateClient = new CertificateClient(new Uri(vaultUrl), credentials);
                var secretClient = new SecretClient(new Uri(vaultUrl), credentials);

                var certificate =
                    certificateClient.GetCertificate(certificateName).Value;

                // Return a certificate with only the public key if the private key is not exportable.
                if (!certificate.Policy?.Exportable ?? false)
                    return Ok(new X509Certificate2(certificate.Cer));

                // Parse the secret ID and version to retrieve the private key.
                var segments = certificate.SecretId.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length != 3)
                    throw new InvalidOperationException($"Number of segments is incorrect: {segments.Length}, URI: {certificate.SecretId}");

                var secretName = segments[1];
                var secretVersion = segments[2];

                var secret =
                    secretClient.GetSecret(secretName, secretVersion).Value;

                // For PEM, you'll need to extract the base64-encoded message body.
                if ("application/x-pkcs12".Equals(secret.Properties.ContentType, StringComparison.InvariantCultureIgnoreCase))
                {
                    var pfx = Convert.FromBase64String(secret.Value);

                    //var cert =
                    //    new X509Certificate2(pfx);
                }
                else
                    throw new NotSupportedException($"Only PKCS#12 is supported. Found Content-Type: {secret.Properties.ContentType}");

                return Ok(
                    new
                    {
                        SecretName = secretName,
                        SecretVersion = secretVersion,
                        SecretValue = secret == null ? "null" : secret.Value,
                        SecretId = secret == null ? "null" : secret.Id.OriginalString,
                        CretName = certificate == null ? "null" : certificate.Name,
                        Cert = certificate,
                        Secret = secret
                    });
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("PublishSiteMessageViewer")]
        [AllowAnonymous]
        public async Task<IActionResult> PublishSiteMessageViewer(int brandId, int siteId)
        {
            try
            {
                await
                new MessageSender()
               .SendMessageAsync(
                    new PublishSiteMessage()
                    {
                        BrandId = brandId,
                        SiteId = siteId,
                        Action = PublishSiteMessageActions.Publish
                    },
                    _config["ConnectionStrings:SBUserIngest"] ?? string.Empty,
                    PublishSiteMessageTopic.Publishing,
                    JsonNamingPolicy.CamelCase);
               
                return
                    Ok("Test Message Sent");
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("AddADUserProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> AddADUserProfile()
        {
            try
            {
                if (!Request.Host.Host.ToLower().Contains("localhost"))
                    throw new Exception("This feature only available in debug mode to developers");

                //Joseph :  j.shavit@fusion92.com
                //Jelena Colak j.colak@fusion92.com
                //Nikole Misiuro n.misiuro@fusion92.com
                //Michele June m.june@fusion92.com
                //Jessica Pedersen j.pedersen@fusion92.com
                //Sarah Ramsey s.ramsey@fusion92.com
                //Drew McDonald d.mcdonald@fusion92.com
                var firstName = "Drew";
                var lastName = "McDonald2";
                var email = "d.mcdonald2@fusion92.com";

                if (string.IsNullOrEmpty(firstName) ||
                    string.IsNullOrEmpty(lastName) ||
                    string.IsNullOrEmpty(email))
                    throw new Exception("Invalid data");

                await
                _userService
                .AddADUserAsync(
                new ADUser()
                {
                    GraphUserData =
                        new Microsoft.Graph.User()
                        {
                            DisplayName = $"{firstName} {lastName}",
                            GivenName = firstName,
                            Surname = lastName,
                            StreetAddress = "444 Ontario St.",
                            City = "Chicago",
                            State = "IL",
                            PostalCode = "60654",
                            Country = "USA",
                            Mail = email,
                            MobilePhone = "555-555-1212",
                            EmployeeType = "Agent",
                            EmployeeId = General.NONSSO_EmployeeId,
                            CreatedDateTime = DateTime.Now,
                            AccountEnabled = true
                        },
                    CustomAttributes =
                        new CustomAttribute()
                        {
                            BrandId = "3",
                            SSOId = string.Empty,
                            HasPersonalizedWebsiteAgent = true,
                            HasPersonalizedWebsiteRecruiter = true,
                            EligibleForPersonalizedWebsite = true,
                            AARP = false,
                            ApprovedDBA = true,
                            EagleAdvisor = false,
                            LongTermCare = false,
                            Nautilus = false,
                            RegisteredRep = false
                        },
                    ADGroupIds = new List<string>()
                });

                var user =
                    await
                    _userService
                    .GetADUserAsync(email);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("RebuildADuserProfile")]
        [AllowAnonymous]
        public async Task<IActionResult> RebuildADUserProfile(string email)
        {
            // This is meant to allow us to quickly update an AD profile and set custom attributes which are not
            // easy to set in the azure portal.  This is a powerful tool and can orphan a user if used incorrectly.
            // 9 out of 10 times we will only want to chang the custom attrs here, the rest are easy to do in the portal.
            try
            {
                if (!Request.Host.Host.ToLower().Contains("localhost"))
                    throw new Exception("This feature only available in debug mode to developers");

                // Get user profile
                var user =
                    await
                    _userService.GetADUserAsync(email);

                if (user != null)
                {
                    // Set new settings
                    user.GraphUserData.EmployeeType = "Admin";

                    if (user.CustomAttributes == null)
                    {
                        user.CustomAttributes =
                            new CustomAttribute()
                            {
                                BrandId = !string.IsNullOrEmpty(user.BrandId) ? user.BrandId : "3",
                                SSOId = user.SSOId ?? "",
                                HasPersonalizedWebsiteAgent = user.HasPersonalizedWebsiteAgent,
                                HasPersonalizedWebsiteRecruiter = user.HasPersonalizedWebsiteRecruiter,
                                EligibleForPersonalizedWebsite = user.EligibleForPersonalizedWebsite,
                                EagleAdvisor = user.EagleAdvisor,
                                Nautilus = user.Nautilus,
                                RegisteredRep = user.RegisteredRep,
                                ApprovedDBA = user.ApprovedDBA,
                                LongTermCare = user.LongTermCare,
                                AARP = user.AARP
                            };
                    }

                    // Updatable fields, pick and choose based on need
                    //user.GraphUserData.DisplayName = "";
                    //user.GraphUserData.GivenName = "";
                    //user.GraphUserData.Surname = "";
                    //user.GraphUserData.StreetAddress = "";
                    //user.GraphUserData.City = "" || "None";
                    //user.GraphUserData.State = "";
                    //user.GraphUserData.PostalCode = "";
                    //user.GraphUserData.Country = "";
                    //user.GraphUserData.AccountEnabled = false/true;
                    //user.GraphUserData.MobilePhone = "";
                    //exiuser.GraphUserDatasting.CompanyName = "";
                    //user.GraphUserData.MailNickname = "";
                    //user.GraphUserData.EmployeeType = "";
                    //user.GraphUserData.EmployeeId = "";

                    //user.GraphUserData.ADGroupIds = user.ADGroupIds;

                    // Update
                    await _userService.UpdateADUserAsync(user);
                }
                else
                    return BadRequest("User not found");

                var updatedUser =
                    await
                    _userService.GetADUserAsync(email);

                // When merged into branch with .ToNYLUser(), add that here
                return
                    Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("GetUserDDCData")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDDCData(string ssoId)
        {
            try
            {
                var detail =
                    await
                    _dbUserService.GetUserAccessInfoDetailDDCData(ssoId);

                if (detail == null)
                    throw new Exception($"DDC data unavailable for {ssoId}");

                detail.UserLicensesDetailDDC = 
                    await
                    _dbUserService.GetUserLicenseInfoDetailDDCData(detail.NYLId);
                
                return
                    Ok(detail);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ShowHost")]
        [AllowAnonymous]
        public IActionResult ShowHost()
        {
            try
            {
                return
                    Ok(Request.Host);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("SiteUpdateTest")]
        [AllowAnonymous]
        public async Task<IActionResult> SiteUpdateTest()
        {
            try
            {
                var siteId = 178;

                var site =
                    await _siteService.GetSite(siteId);

                if (site == null)
                    return NotFound($"Site matching id {siteId} not found");

                //brandId: 3
                //description: null
                //footer: "{\"showFooter\":true,\"logo\":\"https://agentdeployments.blob.core.windows.net/$web/brands/3/websites/154/uploads/SimpleFinancialSolutions_LogoandTag.2020.09.png\",\"logoAltText\":\"\",\"logoLink\":\"\",\"name\":\"{{{ user.ddcUserData.mktrLglFirstNm }}} {{{ user.ddcUserData.mktrLglMiddleNm }}} {{{ user.ddcUserData.mktrLglLastName }}} {{{ user.ddcUserData.mktrLglSfxCode }}}\",\"jobTitle\":\"{{{ user.ddcUserData.agentTitleExternalDesc }}}\",\"showSignUpForm\":false,\"signUpFormDisclosure\":\"\",\"facebook\":\"{{{ user.ddcUserData.facebookUrlTxt }}}\",\"linkedin\":\"{{{ user.ddcUserData.lnkdinUrlTxt }}}\",\"instagram\":\"\",\"twitter\":\"{{{ user.ddcUserData.twitterUrlTxt }}}\",\"youtube\":\"\",\"disclosures\":[\"<p>Neither New York Life Insurance Company, nor its agents provide tax, legal, or accounting advice. Please consult your own tax, legal, or accounting professionals before making any decisions.</p>\",\"<p></p>\"],\"logoStyle\":\"both\",\"showDisclosures\":true,\"nameFooter\":\"{{{ user.ddcUserData.mktrLglFirstNm }}} {{{ user.ddcUserData.mktrLglMiddleNm }}} {{{ user.ddcUserData.mktrLglLastName }}} {{{ user.ddcUserData.mktrLglSfxCode }}}\",\"jobTitleFooter\":\"{{{ user.ddcUserData.agentTitleExternalDesc }}}\",\"copyright\":\"© Copyright 2022 Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod temporincididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitationullamco laboris nisi ut aliquip ex ea commodo consequat.\",\"address\":\"{{{user.streetAddress}}} {{{user.city}}}, {{{user.state}}} {{{user.postalCode}}}\"}"
                //handle: "9c44eab5-36dd-4dbb-97f7-b93a9c9cf050"
                //homepageId: 299
                //id: 154
                //isPrivate: false
                //navigation: "{\"logo\":\"https://agentdeployments.blob.core.windows.net/$web/brands/3/websites/154/uploads/SimpleFinancialSolutions_LogoandTag.2020.09.png\",\"logoAltText\":\"\",\"logoLink\":\"\",\"name\":\"{{{ user.ddcUserData.mktrLglFirstNm }}} {{{ user.ddcUserData.mktrLglMiddleNm }}} {{{ user.ddcUserData.mktrLglLastName }}} {{{ user.ddcUserData.mktrLglSfxCode }}}\",\"jobTitle\":\"{{{ user.ddcUserData.agentTitleExternalDesc }}}\",\"button\":false,\"buttonText\":\"\",\"buttonLink\":\"\",\"buttonTarget\":true,\"logoStyle\":\"medium\",\"menuAlignment\":\"right\"}"
                //pages: [{,…}, { blocks: "[]", settings: "null", redirectUrl: null, navigationTitle: null, seoTitle: null,…},…]
                //presets: [{ id: 108, type: "image", title: "Untitled Image", description: null, clientCode: null,…},…]
                //seats: []
                //            seoDescription: "Simple Financial Solutions"
                //seoIsPrivate: false
                //seoTitle: "Simple Financial Solutions"
                //settings: "null"
                //style: null
                //themes: { base: "nyl-01", color: null, font: null}
                //title: "Simple Financial Solutions"

                //var siteUpdate =
                //    new UpdateSiteDTO()
                //    {
                //        //BrandId = ,
                //        Description = null,
                //        Footer = "{\"showFooter\":true,\"logo\":\"https://agentdeployments.blob.core.windows.net/$web/brands/3/websites/154/uploads/SimpleFinancialSolutions_LogoandTag.2020.09.png\",\"logoAltText\":\"\",\"logoLink\":\"\",\"name\":\"{{{ user.ddcUserData.mktrLglFirstNm }}} {{{ user.ddcUserData.mktrLglMiddleNm }}} {{{ user.ddcUserData.mktrLglLastName }}} {{{ user.ddcUserData.mktrLglSfxCode }}}\",\"jobTitle\":\"{{{ user.ddcUserData.agentTitleExternalDesc }}}\",\"showSignUpForm\":false,\"signUpFormDisclosure\":\"\",\"facebook\":\"{{{ user.ddcUserData.facebookUrlTxt }}}\",\"linkedin\":\"{{{ user.ddcUserData.lnkdinUrlTxt }}}\",\"instagram\":\"\",\"twitter\":\"{{{ user.ddcUserData.twitterUrlTxt }}}\",\"youtube\":\"\",\"disclosures\":[\"<p>Neither New York Life Insurance Company, nor its agents provide tax, legal, or accounting advice. Please consult your own tax, legal, or accounting professionals before making any decisions.</p>\",\"<p></p>\"],\"logoStyle\":\"both\",\"showDisclosures\":true,\"nameFooter\":\"{{{ user.ddcUserData.mktrLglFirstNm }}} {{{ user.ddcUserData.mktrLglMiddleNm }}} {{{ user.ddcUserData.mktrLglLastName }}} {{{ user.ddcUserData.mktrLglSfxCode }}}\",\"jobTitleFooter\":\"{{{ user.ddcUserData.agentTitleExternalDesc }}}\",\"copyright\":\"© Copyright 2022 Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod temporincididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitationullamco laboris nisi ut aliquip ex ea commodo consequat.\",\"address\":\"{{{user.streetAddress}}} {{{user.city}}}, {{{user.state}}} {{{user.postalCode}}}\"}",
                //        Handle = "9c44eab5-36dd-4dbb-97f7-b93a9c9cf050",
                //        HomepageId = 250,
                //        //id
                //        IsPrivate = false,
                //        Navigation = "{\"logo\":\"https://agentdeployments.blob.core.windows.net/$web/brands/3/websites/154/uploads/SimpleFinancialSolutions_LogoandTag.2020.09.png\",\"logoAltText\":\"\",\"logoLink\":\"\",\"name\":\"{{{ user.ddcUserData.mktrLglFirstNm }}} {{{ user.ddcUserData.mktrLglMiddleNm }}} {{{ user.ddcUserData.mktrLglLastName }}} {{{ user.ddcUserData.mktrLglSfxCode }}}\",\"jobTitle\":\"{{{ user.ddcUserData.agentTitleExternalDesc }}}\",\"button\":false,\"buttonText\":\"\",\"buttonLink\":\"\",\"buttonTarget\":true,\"logoStyle\":\"medium\",\"menuAlignment\":\"right\"}",
                //        //Pages = 
                //        //    new List<UpdatePageDTO>()
                //        //    { 
                //        //        new UpdatePageDTO() { },
                //        //        new UpdatePageDTO() { }
                //        //    },
                //        //Presets = ,
                //        //Seats =
                //        SeoIsPrivate = false,
                //        SeoTitle = "Simple Financial Solutions",
                //        Settings = "null",
                //        Style = null,
                //        Themes = 
                //            new UpdateSiteThemesDTO()
                //            {
                //                Base = "nyl-01-dark",
                //                Color = null,
                //                Font = null
                //            },
                //        Title = "Simple Financial Solutions"
                //    };

                var updateSite =
                    new CoreLib.Models.DTO.Site.SiteDTO()
                    {
                        Handle = site.Handle,
                        Title = site.Title,
                        Description = site.Description,
                        Settings = site.Settings,
                        Style = site.Style,
                        Navigation = site.Navigation,
                        Footer = site.Footer,
                        SeoTitle = site.SeoTitle,
                        SeoDescription = site.SeoDescription,
                        SeoIsPrivate = site.SeoIsPrivate,
                        IsPrivate = site.IsPrivate,
                        HomepageId = site.HomepageId,
                        Pages =
                            (site?.Pages ?? new List<PageDTO>())
                            .Select(s =>
                                new PageDTO()
                                {
                                    Blocks = s.Blocks,
                                    Description = s.Description,
                                    Handle = s.Handle,
                                    Id = s.Id,
                                    IsDeleted = false,
                                    IsPrivate = s.IsPrivate,
                                    NavigationTitle = s.NavigationTitle,
                                    ParentPageId = s.ParentPageId,
                                    RedirectUrl = s.RedirectUrl,
                                    SeoDescription = s.SeoDescription,
                                    SeoIsPrivate = s.SeoIsPrivate,
                                    SeoTitle = s.SeoTitle,
                                    Settings = s.Settings,
                                    Title = s.Title
                                })
                            .ToList(),
                        Themes =
                            new SiteThemesDTO()
                            {
                                //Id = Id correlated on commit
                                Base = "nyl-01",
                                Color = null,
                                Font = null
                            }
                    };

                var record = 
                    await _siteService.UpdateSite(siteId, updateSite);

                var updatedSite =
                    await _siteService.GetSite(siteId);

                return
                    record != null ?
                        Ok(updatedSite) :
                        NotFound($"Site matching id {siteId} not found");
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("DuplicateSite")]
        [AllowAnonymous]
        public async Task<IActionResult> DuplicateSite(int siteId)
        {
            try
            {
                var site =
                    await _siteService.GetSite(siteId);

                if (site == null)
                    return NotFound($"Site matching id {siteId} not found");

                var newSite = 
                    await
                    _siteService.DuplicateSiteUseWithCaution(site);

                return
                    Ok(newSite);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ChangeSiteUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeSiteUser(int siteId, string userEmailAddress)
        {
            try
            {
                var user =
                    await
                    _userService
                    .GetADUserAsync(userEmailAddress);

                if (user == null)
                    throw new Exception("User not found");

                return
                    Ok(await _siteService.AssignSiteToNewUser(siteId, user.GraphUserData.Mail, user.GraphUserData.Id));
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("HardDeleteSiteNORECOVERYFROMTHIS")]
        [AllowAnonymous]
        public async Task<IActionResult> HardDeleteSite(int siteId, string handle)
        {
            try
            {
                await _siteService.HardDeleteSiteUseWithCaution(siteId, handle);

                return
                    Ok();
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("GenerateNYLBearerToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GenerateNYLBearerToken()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient(HttpNamedClients.NYLCLTLeadSubmissionClient);
                var clientId = _config["NYL:ClientId"] ?? "";
                var clientSecret = _config["NYL:ClientId"] ?? "";
                var tokenUrl = _config["NYL:TokenUrl"] ?? "";

                var content =
                    new Dictionary<string, string>
                        {
                            {"grant_type", "client_credentials"},
                            {"client_id", _config["NYL:ClientId"]},
                            {"client_secret", _config["NYL:ClientSecret"]},
                            {"scope", "READ"},
                        };

                var url = _config["NYL:TokenUrl"];

                HttpResponseMessage? tokenResponse = null;
                try
                {
                    tokenResponse =
                       await
                       httpClient
                       .PostAsync(
                           tokenUrl,
                           new FormUrlEncodedContent(content));
                }
                catch (Exception e)
                {
                    return StatusCode(500, 
                        $"Message: {e.Message} :: " +
                        $"Inner: {(e.InnerException != null ? e.InnerException.Message : "")} :: " +
                        $"Inner-Inner: {(e.InnerException?.InnerException != null ? e.InnerException?.InnerException?.Message : "")} :: " +
                        $"Inner-Inner-Inner: {(e.InnerException?.InnerException?.InnerException != null ? e.InnerException?.InnerException?.InnerException.Message : "")}");
                }

                if (tokenResponse == null)
                    return StatusCode(500, "Null token response");

                var jsonContent =
                    await
                    tokenResponse.Content.ReadAsStringAsync();

                var token =
                    JsonConvert
                    .DeserializeObject<Token>(jsonContent ?? string.Empty);

                return
                    Ok(
                        new 
                        { 
                            ClientId = clientId,
                            ClientSecret = clientSecret,
                            TokenUrl = tokenUrl,
                            TokenResponse = tokenResponse,
                            TokenResponseContent = jsonContent,
                            Token = token
                        });
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        [HttpGet("ListAvailableThemes")]
        [AllowAnonymous]
        public async Task<IActionResult> ListAvailableThemes()
        {
            try
            {
                var availableThemes =
                    await _libraryService.GetThemeList(3);

                return Ok(availableThemes);
            }
            catch (Exception ex)
            {
                return ex.PackageExceptionResponse(_logger);
            }
        }

        internal class Token
        {
            [JsonProperty("access_token")]
            public string AccessToken { get; set; }

            [JsonProperty("token_type")]
            public string TokenType { get; set; }

            [JsonProperty("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonProperty("refresh_token")]
            public string RefreshToken { get; set; }
        }
    }
}