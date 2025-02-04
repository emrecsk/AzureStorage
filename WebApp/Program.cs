using AzureStrorageLibrary;
using AzureStrorageLibrary.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConnectionString.Con = builder.Configuration.GetSection("AzureConnectionString")["StorageConn"]!;
builder.Services.AddScoped(typeof(INoSqlStorage<>), typeof(TableStorage<>));
builder.Services.AddSingleton<IBlobStorage, BlobStorage>();
builder.Services.AddSession();

builder.Services.AddControllersWithViews()
                .AddSessionStateTempDataProvider();

builder.Services.AddSignalR();
IKernel kernel = new KernelBuilder().WithAzureChatCompletionService(
         "chat",                      // Azure OpenAI Deployment Name
         "https://cog-fy2j2wq6oicv6.openai.azure.com/", // Azure OpenAI Endpoint
         "2437bd26a266403685e01678b51a5e43").Build();

builder.Services.AddSingleton<IKernel>(kernel);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapHub<NotificationHub>("/NotificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
