
namespace Core.CoreLib.Models.Constants
{
    public static class Authorization
    {
        public static List<string> GetPermissionsFromEmployeeType(
            string employeeType,
            bool eligibleForPersonalizedWebsite)
        {
            switch (employeeType?.ToLower() ?? string.Empty)
            {
                case EmployeeType.Admin:
                    return Roles.BrandAdmin;

                case EmployeeType.Agent:
                    return Roles.Agent(eligibleForPersonalizedWebsite);

                case EmployeeType.Reviewer:
                    return Roles.Reviewer;

                case EmployeeType.DomainReviewer:
                    return Roles.DomainReviewer;

                case EmployeeType.ContentLibrarian:
                    return Roles.ContentLibrarian;

                //TODO: Support permission sets for these roles
                case EmployeeType.HomeOffice:
                case EmployeeType.GeneralOffice:    
                case EmployeeType.OnBehalfOf:
                    return Roles.BrandAdmin;

                default:
                    return new List<string>();
            }
        }

        public static class Roles
        {
            public static List<string> God =>
                new List<string>() { EmployeeType.God }; // Permissions do not apply, use sparingly, probably only devs, can act cross tenants, brands, etc.

            public static List<string> BrandAdmin =>
                new List<string>() 
                { 
                    Permissions.ReadSite, Permissions.SaveSite, Permissions.SubmitToCompliance, Permissions.ReadContent,
                    Permissions.ListSites, Permissions.ReadSite, Permissions.ApproveSite, Permissions.PublishSite,
                    Permissions.CreateContent, Permissions.ReadContent, Permissions.UpdateContent, Permissions.PublishSite,
                    Permissions.CreateSite, Permissions.DeleteSite, Permissions.DeleteContent
                };

            public static List<string> Agent(bool eligibleForPersonalizedWebsite) =>
                eligibleForPersonalizedWebsite ?
                new List<string>() { Permissions.ReadSite, Permissions.SaveSite, Permissions.SubmitToCompliance, Permissions.ReadContent, Permissions.CreateSite, Permissions.DeleteSite } :
                new List<string>() { Permissions.ReadSite, Permissions.SaveSite, Permissions.SubmitToCompliance, Permissions.ReadContent };

            public static List<string> Reviewer =>
                new List<string>() { Permissions.ListSites, Permissions.ReadSite, Permissions.ApproveSite, Permissions.PublishSite };

            public static List<string> DomainReviewer =>
                new List<string>() { Permissions.ListSites, Permissions.ReadSite, Permissions.ApproveSite, Permissions.PublishSite };

            public static List<string> ContentLibrarian =>
                new List<string>() { Permissions.CreateContent, Permissions.ReadContent, Permissions.UpdateContent, Permissions.PublishSite };
        }

        public static class Permissions
        {
            // Site
            public const string ApproveSite = "approvesite";
            public const string ListSites = "listsites";
            public const string PublishSite = "publishsite";
            public const string ReadSite = "readsite";
            public const string SaveSite = "savesite";
            public const string SubmitToCompliance = "submit";
            public const string CreateSite = "createsite";
            public const string DeleteSite = "deletesite";

            // Content
            public const string CreateContent = "createcontent";
            public const string ReadContent = "readcontent";
            public const string UpdateContent = "updatecontent";
            public const string DeleteContent = "deletecontent";
        }

        public static class EmployeeType
        {
            public const string God = "god";
            public const string Admin = "admin";
            public const string BrandAdmin = "brandadmin";
            public const string Agent = "agent";
            public const string GeneralOffice = "go";
            public const string HomeOffice = "ho";
            public const string OnBehalfOf = "obo";

            public const string Reviewer = "reviewer";
            public const string DomainReviewer = "domainreviewer";
            public const string ContentLibrarian = "contentlibrarian";
        }
    }
}



//Global(Fusion92) Admin
//Can do everything within the Platform

//Brand Admin
//Can do everything within a Brand

//Agent
//Read own Sites for Brand
//Save own Sites for Brand
//Submit own Sites to compliance
//Read Content for Brand

//Agent Group (Agents can be placed in groups)

//Reviewer
//List Brand Sites
//Read Brand Sites
//Approve Brand Sites
//Publish Brand Sites (approve and publish are 2 separate processes but once an approval happens, a publish is automatic)

//Domain Reviewer(different type of reviewer that only reviews domains)
//List Brand Sites
//Read Brand Sites
//Approve Brand Sites
//Publish Brand Sites

//Content Librarian
//Create/Read/Update/Delete Content for Brand