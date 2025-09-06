using EasyEnglish.Configurators;
using EasyEnglish.Handlers;
using EasyEnglish.Middlewares;
using Hangfire;

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

app.UseCors("CorsPolicy");

app.UseExceptionHandler(error => error.Run(GlobalExceptionHandler.Handle));

app.UseAuthentication();
app.UseAuthorization();

app.UseUserInfo();

app.MapControllers();

app.MapHangfireDashboard();

app.MigrateDatabase();

app.Run();
