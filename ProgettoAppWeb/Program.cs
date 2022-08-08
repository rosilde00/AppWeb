using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<LiteContext>(options =>
{
    options.UseSqlite(ConnectionStringLite.connectionString);
});

builder.Services.AddDbContext<ServerContext>(options =>
{
    options.UseSqlServer(ConnectionStringServer.connectionString);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

InitContext();

app.Run();

void InitContext()
{
    using var scope = app.Services.CreateScope();
    using var liteContext = scope.ServiceProvider.GetRequiredService<LiteContext>();
    using var serverContext = scope.ServiceProvider.GetRequiredService<ServerContext>();
}

