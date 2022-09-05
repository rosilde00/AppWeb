using Microsoft.EntityFrameworkCore;
using ProgettoAppWeb.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<LiteContext>(options =>
{
    options.UseSqlite(ConnectionStringLite.listConnectionStrings.ElementAt(ConnectionStringLite.actual));
});

builder.Services.AddDbContext<ServerContext>(options =>
{
    options.UseSqlServer(ConnectionStringServer.listConnectionStrings.ElementAt(ConnectionStringServer.actual));
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

