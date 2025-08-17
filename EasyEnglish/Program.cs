using EasyEnglish.Configurators;
using EasyEnglish.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder
    .ConfigureCors()
    .ConfigureLogging()
    .ConfigureAutofac()
    .ConfigureDatabase();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseExceptionHandler(error => error.Run(GlobalExceptionHandler.Handle));

app.MapControllers();

app.MigrateDatabase();

app.Run();
