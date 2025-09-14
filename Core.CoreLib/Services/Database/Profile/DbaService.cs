
using Microsoft.ApplicationInsights;
using Core.CoreLib.Models.DTO.Profile;
using DbaObject = Core.CoreLib.Models.Database.Core.Dba;
using Core.CoreLib.Services.Database.Base;

namespace Core.CoreLib.Services.Database.Profile
{
    public class DbaService : DBBase, IDbaService
    {
        protected TelemetryClient _telemetryClient;

        public DbaService(
            DapperContext dapperContext, 
            TelemetryClient telemetryClient) : base(dapperContext)
        {
            _telemetryClient = telemetryClient;
        }

        public async Task<string> InsertDba(SubmitDbaDTO dbaData, string dbaLogoGuid)
        {
            _telemetryClient.TrackTrace($"Inserting dba data for {dbaLogoGuid}.");

            var dbaGuid = 
                await ExecuteScalarQuery(
                    "INSERT INTO [Core].[Dba] ([DbaLogoGuid], [UrlGuid], [ImageGuid], [ImageBinary], [CreatedDateTime]) OUTPUT INSERTED.DbaLogoGuid VALUES (@DbaLogoGuid, @UrlGuid, @ImageGuid, @ImageBinary, @CreatedDateTime)",
                    new
                    {
                        @DbaLogoGuid = dbaLogoGuid,
                        @UrlGuid = dbaData.WebSite.UrlGuid,
                        @ImageGuid = Guid.NewGuid().ToString(),
                        @ImageBinary = dbaData.Dba.Image.Data,
                        @CreatedDateTime = DateTime.UtcNow
                    });

            _telemetryClient.TrackTrace($"Dba inserted successfully for {dbaLogoGuid}.  dbaGuid: {dbaGuid ?? string.Empty}");

            return dbaGuid;
        }

        public async Task<string> UpdateDba(SubmitDbaDTO dbaData, string urlGuid)
        {
            _telemetryClient.TrackTrace($"Updating dba data for url guid {urlGuid}.");

            // This doesn't look right, how are returning an inserted value that we aren't inserting?
            // Should be update and return bool updatedrows > 0
            var dbaGuid = 
                await ExecuteScalarQuery(
                    "UPDATE [Core].[Dba] SET ImageBinary = @ImageBinary, LastModifiedDateTime = @LastModifiedDateTime OUTPUT INSERTED.DbaLogoGuid WHERE UrlGuid = @UrlGuid",
                    new
                    {
                        @UrlGuid = urlGuid,
                        @ImageBinary = dbaData.Dba.Image.Data,
                        @LastModifiedDateTime = DateTime.UtcNow
                    });

            _telemetryClient.TrackTrace($"Dba updated successfully for dbaGuid: {dbaGuid ?? string.Empty}");

            return dbaGuid;
        }

        public async Task<DbaObject> GetDbaUrlGuid(string urlGuid)
        {
            _telemetryClient.TrackTrace($"Fetching the dba data for url guid: {urlGuid}.");

            var dba = 
                (await ExecuteQuery<DbaObject>("SELECT DbaLogoGuid, UrlGuid, ImageGuid, ImageBinary FROM [Core].[Dba] WHERE UrlGuid = @UrlGuid",
                new { @UrlGuid = urlGuid }))
                .FirstOrDefault();

            _telemetryClient.TrackTrace($"Fetching the dba data for url guid: {urlGuid} yields: {(dba == null ? "NULL" : dba)}");

            return dba;
        }
    }
}