using Microsoft.AspNetCore.Authentication.Cookies;

// ������� ����������� �����-��������� WebApplicationBuilder c ������� ������ CreateBuilder()
var builder = WebApplication.CreateBuilder(args);

// �������:
builder.Services.AddMvc();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => 
{ 
    options.LoginPath = "/Account/EntryPage"; 
    options.AccessDeniedPath = "/Main/HomePage";
    options.Cookie.HttpOnly = true;
    options.Cookie.MaxAge = TimeSpan.FromDays(7);
});
builder.Services.AddAuthorization();

// ������� ������ WebApplication, ������ �� ������� WebApplicationBuilder ����� Build()
var app = builder.Build();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();


app.UseRouting();

// ������������� ������������� �������� � �������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=HomePage}/{id?}");


app.Run();
