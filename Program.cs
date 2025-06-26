using LucyContainerFront;
using LucyContainerFront.ViewModels;
using LucyShared.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var apiBaseUrl = builder.Configuration["ApiBaseUrl"];
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IContainerService, ContainerService>();
builder.Services.AddScoped<AuthorizationMessageHandler>();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
//Auth Client
builder.Services
    .AddHttpClient("AuthClient", client => client.BaseAddress = new Uri(apiBaseUrl));
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient"));
//Auth Client End

//builder.Services
//    .AddHttpClient("ApiClient", client => client.BaseAddress = new Uri(apiBaseUrl))
//    .AddHttpMessageHandler<AuthorizationMessageHandler>();
// shared ContainerService **as a typed client**
builder.Services
  .AddHttpClient<IContainerService, ContainerService>(client =>
  {
      client.BaseAddress = new Uri(apiBaseUrl);
  })
  .AddHttpMessageHandler<AuthorizationMessageHandler>();

// 3) register a default HttpClient pointing at the API


//builder.Services.AddScoped(sp =>
//    sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"));

builder.Services.AddScoped<Func<HttpClient>>(sp =>
    () => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient")
);

builder.Services
    .AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
// 4) enable Blazor auth support
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<LoginViewModel>();
builder.Services.AddScoped<ContainerSearchViewModel>();
await builder.Build().RunAsync();
