using Core.CoreLib.Models.Constants;
using Core.CoreLib.Models.Database.Core;
using Core.CoreLib.Services.Database;
using Dapper;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;

namespace Core.CoreLib.Services.Form
{
    // TODO: This class should be moved under Database directory
    public class FormService : IFormService
    {
        protected DapperContext _context;
        protected IConfiguration _configuration; // UNUSED remove
        protected TelemetryClient _telemetryClient;

        public FormService(IConfiguration configuration, DapperContext dapperContext, TelemetryClient telemetryClient)
        {
            _context = dapperContext;
            _configuration = configuration; // UNUSED
            _telemetryClient = telemetryClient;
        }

        public async Task<string> GenerateReferenceNumber(int siteid)
        {
            const string description = "fusion 92";

            using var connection = _context.CreateConnection();

            // Check if there if the form was already submitted
            _telemetryClient.TrackTrace($"Creating the lead form reference id for site {siteid}.");

            var leadForm = 
                new LeadForm()
                {
                    Description = description,
                    SiteId = siteid,
                    IsSubmitted = true,
                    DateSubmitted = DateTime.UtcNow
                };

            //var referenceNumber = 
            //    await connection.ExecuteScalarAsync<string>($"INSERT INTO LeadForm(Description, SiteId, IsSubmitted, DateSubmitted) OUTPUT INSERTED.ReferenceNumber VALUES (@Description, @SiteId, @IsSubmitted, @DateSubmitted)", new { leadForm.Description, leadForm.SiteId, leadForm.IsSubmitted, leadForm.DateSubmitted });

            var coreReferenceNumber =
                await connection.ExecuteScalarAsync<string>($"INSERT INTO {DatabaseTables.LeadForm} (Description, SiteId, IsSubmitted, DateSubmitted) OUTPUT INSERTED.ReferenceNumber VALUES (@Description, @SiteId, @IsSubmitted, @DateSubmitted)", new { leadForm.Description, leadForm.SiteId, leadForm.IsSubmitted, leadForm.DateSubmitted });

            //Format the reference number
            var formattedReferenceNumber = $"{LeadFormFields.FusionReferenceNumberPrefix}{coreReferenceNumber.PadLeft(9, '0')}";

            _telemetryClient.TrackTrace($"Created the lead form reference id for site {siteid}.  refNumber: {coreReferenceNumber}, formattedRefNumber: {formattedReferenceNumber}");

            return formattedReferenceNumber;
        }
    }
}