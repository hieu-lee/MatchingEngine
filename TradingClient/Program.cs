var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(s => OrderService.OrderServiceBuildAsync());

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7007") });

builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<StockService>();

builder.Services.AddScoped<TransactionService>();

await builder.Build().RunAsync();
