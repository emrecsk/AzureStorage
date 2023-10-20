using AzureStrorageLibrary;
using AzureStrorageLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConnectionString.Con = builder.Configuration.GetSection("AzureConnectionString")["StorageConn"]!;
builder.Services.AddScoped(typeof(INoSqlStorage<>), typeof(TableStorage<>));
builder.Services.AddSingleton<IBlobStorage, BlobStorage>();

builder.Services.AddSession();

builder.Services.AddControllersWithViews()
                .AddSessionStateTempDataProvider();

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
