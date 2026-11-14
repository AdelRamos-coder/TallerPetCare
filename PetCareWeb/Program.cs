using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PetCareWeb.Components;
using PetCareWeb.Datos;
using PetCareWeb.Servicios;

// Pesos colombianos y fechas en español en toda la aplicación.
var cultura = new CultureInfo("es-CO");
CultureInfo.DefaultThreadCurrentCulture = cultura;
CultureInfo.DefaultThreadCurrentUICulture = cultura;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Las páginas nunca crean el contexto: los servicios piden uno corto a la fábrica en cada operación.
builder.Services.AddDbContextFactory<PetCareDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("PetCare")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opciones =>
    {
        opciones.Cookie.Name = "PetCare.Sesion";
        opciones.Cookie.HttpOnly = true;
        opciones.LoginPath = "/iniciar-sesion";
        opciones.AccessDeniedPath = "/iniciar-sesion";
        opciones.ExpireTimeSpan = TimeSpan.FromHours(8);
        opciones.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<ServicioAutenticacion>();
builder.Services.AddScoped<ServicioCitas>();
builder.Services.AddScoped<ServicioHospitalizaciones>();
builder.Services.AddScoped<ServicioLotes>();
builder.Services.AddScoped<ServicioFacturas>();
builder.Services.AddScoped<ServicioInventario>();

var app = builder.Build();

// Crea la base de datos o aplica las migraciones que falten (incluye usuarios de prueba y jaulas).
using (var alcance = app.Services.CreateScope())
{
    var fabrica = alcance.ServiceProvider.GetRequiredService<IDbContextFactory<PetCareDbContext>>();
    using var db = fabrica.CreateDbContext();
    db.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Después de cerrar sesión, el botón Atrás del navegador no debe mostrar páginas guardadas en caché.
app.Use(async (contexto, siguiente) =>
{
    contexto.Response.Headers.CacheControl = "no-store";
    await siguiente();
});

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/cerrar-sesion", async (HttpContext contexto) =>
{
    await contexto.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.LocalRedirect("~/iniciar-sesion");
});

app.Run();
