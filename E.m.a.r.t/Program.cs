using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using System.Globalization;
using E.m.a.r.t.Services;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura��o da Base de Dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configura��o do Identity com roles
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

// Ativar sess�es
builder.Services.AddSession(); 

// Servi�o de envio de emails (ex: SendGrid)
builder.Services.AddTransient<IEmailSender, SendGridEmailSender>();

// Configurar redirecionamento para login caso n�o autenticado
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login"; // Caminho padr�o do Identity
});

var app = builder.Build();

// Ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware de sess�o
app.UseSession(); 

app.UseAuthentication();
app.UseAuthorization();

// Definir rota padr�o
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

// Cultura (moeda, datas, etc.)
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Seed dos dados iniciais (roles, admin, etc.)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.InicializarAsync(services);
}

app.Run();
