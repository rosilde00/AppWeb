using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<LiteContext>(options =>
{
    options.UseSqlite(ConnectionStringLite.listConnectionStrings.Last());
});

builder.Services.AddDbContext<ServerContext>(options =>
{
    options.UseSqlServer(ConnectionStringServer.listConnectionStrings.Last());
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();
app.Run();

