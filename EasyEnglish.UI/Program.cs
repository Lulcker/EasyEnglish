using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EasyEnglish.UI;
using EasyEnglish.UI.Configurators;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder
    .ConfigureHelpers()
    .ConfigureMudBlazor()
    .ConfigureHttpClient()
    .ConfigureLocalStorage()
    .ConfigureAuthorization()
    .ConfigureExceptionHandler();

await builder.Build().RunAsync();