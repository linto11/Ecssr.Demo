using Ecssr.Demo.Application;
using Ecssr.Demo.Infrastructure;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ecssr.Demo.Common.Extensions
{
    /// <summary>
    /// Extension calss to configure the services for DI
    /// </summary>
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder)
        {
            #region AntiForgery
            _ = builder.Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-XSRF-TOKEN";
                options.SuppressXFrameOptionsHeader = false;
            });
            #endregion

            #region Logging
            _ = builder.Logging.ClearProviders();
            _ = builder.Logging.AddConsole();
            #endregion

            #region All urls to be lowercase
            _ =  builder.Services.AddRouting(options => options.LowercaseUrls = true);
            #endregion

            #region AppSettings
            var appSetting = new AppSetting();
            builder.Configuration.GetSection("AppSetting").Bind(appSetting);

            _ = builder.Services.AddSingleton(provider => appSetting);
            #endregion

            #region Create RefId in every Request
            _ = builder.Services.AddScoped<IRefId, RefId>();
            #endregion

            #region Add template once
            _ = builder.Services.AddSingleton<IFileTemplate, FileTemplate>();
            #endregion

            #region Api Versioning
            _ = builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
            });
            #endregion

            #region Controller Binding and Serialization
            _ = builder.Services.AddControllersWithViews()
                .AddJsonOptions(options => 
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            #endregion

            #region Get Dummy News Data
            var data = File.ReadAllText(appSetting.DatabaseSetting.DataFilePath);
            #endregion

            #region Project Dependencies

            _ = builder.Services.AddInfrastructure(appSetting, data);
            _ = builder.Services.AddApplication();

            #endregion Project Dependencies 

            return builder;
        }
    }
}
