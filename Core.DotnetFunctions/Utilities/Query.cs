using Core.DotnetFunctions.AppSettings;

namespace Core.DotnetFunctions.Utilities
{
    public static class Query
    {
        public static string GetUsers(string environment)
        {
            var tableName =
                AdjustTableNameToEnvironment(environment, "NYLUserAccessInfo");

            return
                @$"SELECT [NYLID]
                    ,[PrefFName]
                    ,[PrefMName]
                    ,[PrefLName]
                    ,[PrefAddr1]
                    ,[PrefAddr2]
                    ,[PrefAddr3]
                    ,[City]
                    ,[State]
                    ,[Zip]
                    ,[AreaCode]
                    ,[Phone]
                    ,[Email]
                    ,[LocationName]
                    ,[Fusion92Role]
                    ,[Role]
                    ,[UserActiveFlag]
                    ,[Country]
	                ,[BrandId]
                    ,[IsGo]
                    ,[IsHomeOffice]
                    ,[IsAgent]
                    ,[IsOnBehalf]
                    ,[HasPersonalizedWebsite_Agent]
                    ,[HasPersonalizedWebsite_Recruiter]
                    ,[EligibleForPersonalizedWebsite]
                    ,[EagleInd]
                    ,[NautilusInd]
                    ,[RegRepInd]
                    ,[DBAInd]
                    ,[LTCInd]
                    ,[AARPInd]
                FROM [dbo].[{tableName}]
                WHERE UserActiveFlag = 'Y' and Email is not null";
        }

        private static string AdjustTableNameToEnvironment(string environment, string tableName) =>
            //TODO: staging is currently pointing to dev table but this will change once we have all the environments setup.
            // This method is stable as long as the pradigm is append "_Dev" to non-prod environs
            environment.ToLower() == ConfigurationKey.Prod ? 
                tableName :
                environment.ToLower() == ConfigurationKey.Staging ? 
                    $"{tableName}_Dev" :
                    $"{tableName}_Dev"; 

        public static string GetBrands() =>
             @"SELECT [Id]
                ,[Handle]
                ,[Title]
                ,[Settings]
                ,[Description]
                ,[IsDeleted]
                ,[Host]
            FROM[dbo].[Brands]
            WHERE IsDeleted = 0";

        public static string GetGroupId(string brandId, bool prod) =>
            $"SELECT AzureObjectId FROM [dbo].BrandADGroup WHERE BrandId = {brandId} AND Prod = {(prod ? 1 : 0)}";
    }
}
