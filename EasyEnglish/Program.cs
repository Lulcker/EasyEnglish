using EasyEnglish.Configurators;
using EasyEnglish.Filters;
using EasyEnglish.Handlers;
using EasyEnglish.Middlewares;
using Hangfire;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureHash()
    .ConfigureAuth()
    .ConfigureCors()
    .ConfigureLogging()
    .ConfigureAutofac()
    .ConfigureDatabase()
    .ConfigureHangfire()
    .ConfigureAesCrypto()
    .ConfigureEmailService();

builder.Services.AddControllers();

var app = builder.Build();

app.MapHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new AllowAllDashboardAuthorizationFilter()]
});

app.UseCors("CorsPolicy");

app.UseExceptionHandler(error => error.Run(GlobalExceptionHandler.Handle));

app.UseRouting();
app.UseHttpMetrics();

app.UseAuthentication();
app.UseAuthorization();

app.UseUserInfo();

app.MapMetrics();

app.MapControllers();

app.MigrateDatabase();

app.Run();
