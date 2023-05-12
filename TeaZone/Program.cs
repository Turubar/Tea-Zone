using Microsoft.AspNetCore.Authentication.Cookies;

// Создаем специальный класс-строитель WebApplicationBuilder c помощью метода CreateBuilder()
var builder = WebApplication.CreateBuilder(args);

// Сервисы:
builder.Services.AddMvc();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
{ 
    options.LoginPath = "/Account/EntryPage"; 
    options.AccessDeniedPath = "/Main/HomePage";
    options.Cookie.HttpOnly = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(7);
});
builder.Services.AddAuthorization();

// Создаем объект WebApplication, вызвав на объекте WebApplicationBuilder метод Build()
var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();


app.UseRouting();

// Устанавливаем сопоставление машрутов с контроллерами
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=HomePage}/{id?}");


app.Run();
