using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Unilife.Data;
using Unilife.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
options.LoginPath = "/Account/Login";
options.AccessDeniedPath = "/Account/AccessDenied";
});
builder.Services.AddScoped<Unilife.Services.RecomendadorEventosService>();

builder.Services.AddScoped<Unilife.Services.RecomendadorLugaresService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
app.UseExceptionHandler("/Home/Error");
app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
name: "default",
pattern: "{controller=Account}/{action=Login}/{id?}");

using (var scope = app.Services.CreateScope())
{
var services = scope.ServiceProvider;

var context = services.GetRequiredService<ApplicationDbContext>();
await context.Database.MigrateAsync();

await SeedData.InicializarAsync(services);
await SeedData.SeedValoracionesAsync(services);

}

app.Run();
