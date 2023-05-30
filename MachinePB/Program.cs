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
// Av Björn Lindqvist 2023-02-25
// * I appsetting.json är Database nedkortad till Database=ServerAPIContext.
// * Genom att högerklicka på ServerAPI - Properties - Debug så kan man bocka av Swagger.
// * Högerklicka på Solution - Properties och välj vilket/vilka som ska starta: MachinePB och ServerAPI.
