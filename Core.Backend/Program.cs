
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Core.CoreLib.Services;
using Azure.Identity;
using Core.CoreLib.Models.Settings;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.SnapshotCollector;
using Microsoft.ApplicationInsights.DependencyCollector;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

builder.Services.AddOptions();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core2.0 API", Description = "Large scale website design and deployment.", Version = "v1" });

    // Allow ability to run swagger as an auth'd session
    c.AddSecurityDefinition(
        "Bearer", 
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "bearer"
        });

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[]{}
            }
        });
});

builder.Services.AddLogging(logging =>
{
    logging.AddAzureWebAppDiagnostics();
});
//builder.Services.Configure<AzureFileLoggerOptions>(builder.Configuration.GetSection("Logging"));

//Azure App Configuration
var environment = builder.Configuration["ASPNETCORE_ENVIRONMENT"];

// Load configuration from Azure App Configuration
try
{
    builder.Configuration.AddAzureAppConfiguration(options =>
        {
            //managed identity(for )
            if (environment != "Development")
            {
                if (string.IsNullOrEmpty(builder.Configuration["AppConfigConnectionString"] ?? ""))
                {
                    throw new Exception("Missing the Azure App Configuration Service connection string");
                }
                else
                {
                    //Load configuration from Azure App Configuration
                    options.Connect(builder.Configuration["AppConfigConnectionString"]);
                    options.ConfigureKeyVault(options =>
                    {
                        options.SetCredential(new DefaultAzureCredential());
                    });
                }
            }
            else
            {
                /* Get settings from keyvault by specifying the keyvault client id and secret id.
                   we have to do this to connect to Core 2.0 active directory when 
                   running the app locally */
                if (!string.IsNullOrWhiteSpace(builder.Configuration["KeyVault:AZURE_TENANT_ID"]) && 
                    !string.IsNullOrWhiteSpace(builder.Configuration["KeyVault:AZURE_CLIENT_ID"]) && 
                    !string.IsNullOrWhiteSpace(builder.Configuration["KeyVault:AZURE_CLIENT_SECRET"]))
                {
                    //when it's not managed identity (for dev when we still want to read configs from Azure) 
                    options.ConfigureKeyVault(kv =>
                    {
                        kv.SetCredential(new ClientSecretCredential(
                            builder.Configuration["KeyVault:AZURE_TENANT_ID"],
                            builder.Configuration["KeyVault:AZURE_CLIENT_ID"],
                            builder.Configuration["KeyVault:AZURE_CLIENT_SECRET"])
                            );
                    });
                    options.Connect(builder.Configuration["AppConfigConnectionString"]);
                }
                else
                    throw new Exception("Missing Azure Key Vault config values");
            }
        });
}
catch (Exception ex)
{
    throw new Exception(ex.Message, ex);
}

//Load configuration from appSettings.Development.json file (for local settings only)
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", true, true);
builder.Configuration.AddEnvironmentVariables();

// Once these are called will override previous.  Line 48 loads from AppSettings, then this is called and will reload KeyVault section from Azure.  100%
builder.Services.Configure<KeyVault>(builder.Configuration.GetSection("KeyVault"));
builder.Services.Configure<AppConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<AzureAd>(builder.Configuration.GetSection("AzureAd"));
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<SSORedirect>(builder.Configuration.GetSection("SSORedirect"));
builder.Services.Configure<NYL>(builder.Configuration.GetSection("NYL"));

var options = new ApplicationInsightsServiceOptions() { ConnectionString = builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"] };
builder.Services.AddApplicationInsightsTelemetry(options);

builder.Services.AddSnapshotCollector((configuration) =>
    builder.Configuration.Bind(nameof(SnapshotCollectorConfiguration), configuration));

builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>(
    (module, c) => { module.EnableSqlCommandTextInstrumentation = true; });

//builder.Services.AddSingleton<ITelemetryProcessorFactory>(sp => new SnapshotCollectorTelemetryProcessorFactory(sp));

IOCDefaults.RegisterDefaults(builder.Services, builder.Configuration);

// Authentication
var tokenValidationParameters =
    new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
    };

builder.Services.AddSingleton(tokenValidationParameters);
builder.Services.AddAuthorization();
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = tokenValidationParameters;
});

// Auto-Mapper
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

if ((builder.Configuration["AllowDebug"] ?? "") == "true")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core.Backend API");
    });
}

app.MapGet("/", () => "Hello World (2023/02/28 15:32pm)!");
app.MapGet("/api", () => "Hello World (2023/02/28 15:32pm)!");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers()
    .RequireAuthorization();

app.UseCors(
    options => options.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.Run();
