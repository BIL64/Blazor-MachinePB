using MachinePB;
using MachinePB.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// NuGet: Microsoft.Extensions.Http
builder.Services.AddHttpClient<IMachClient, MachClient>();

await builder.Build().RunAsync();
// Av Bj�rn Lindqvist 2023-02-25
// * I appsetting.json �r Database nedkortad till Database=ServerAPIContext.
// * Genom att h�gerklicka p� ServerAPI - Properties - Debug s� kan man bocka av Swagger.
// * H�gerklicka p� Solution - Properties och v�lj vilket/vilka som ska starta: MachinePB och ServerAPI.
