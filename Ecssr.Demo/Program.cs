using Ecssr.Demo.Common.Extensions;

var app = WebApplication.CreateBuilder(args).ConfigureBuilder().Build().ConfigureApp();

#region Run the app
app.Run();
#endregion
