using EasyEnglish.Configurators;
using EasyEnglish.Handlers;
using EasyEnglish.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureHash()
    .ConfigureAuth()
    .ConfigureCors()
    .ConfigureLogging()
    .ConfigureAutofac()
    .ConfigureDatabase()
    .ConfigureAesCrypto();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseExceptionHandler(error => error.Run(GlobalExceptionHandler.Handle));

app.UseAuthentication();
app.UseAuthorization();

app.UseUserInfo();

app.MapControllers();

app.MigrateDatabase();

app.Run();
