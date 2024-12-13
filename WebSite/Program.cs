using WebAuthCoreBLL.Helpers;
using WebLoginBLL.Services;
using WebObjectsBLL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ��������� Kestrel ��� HTTP/HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000); // HTTP
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<TransactionService>();

// ����������� AutoMapper
builder.Services.AddAutoMapper(
    typeof(WebLoginBLL.MappingProfile),
    typeof(WebObjectsBLL.MappingProfile)
    );

// ����������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5000") // ������� ����������� ������
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// ����������� HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// ������ �������� ��������������
var authSettings = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();
AuthHelper.ConfigureAuthentication(builder.Services, builder.Configuration, authSettings.EnableJwt, authSettings.EnableCookies);

// ��������� ������ �����������
string connectionStringKey = authSettings.ServerName switch
{
    "Host" => "CRMConnection",
    "Developer" => "DeveloperConnection",
    "Vlad" => "VladConnection",
    _ => throw new ArgumentException($"Invalid ServerName: {authSettings.ServerName}")
};
var connectionStringCRM = builder.Configuration.GetConnectionString(connectionStringKey);

// ��������� DbContext
builder.Services.AddDbContext<DAL.Models.BankContext>(options =>
    options.UseSqlServer(connectionStringCRM));

// ���������� Razor Pages � ��������� ������
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware ��� ��������� ������
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
// ��������� CORS ����� ��������������� � ������������
app.UseCors("AllowSpecificOrigins");

// ��������������
if (authSettings.EnableCookies || authSettings.EnableJwt)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// ��������� ��������� ��� MVC � API
if (authSettings.EnableCookies)
{
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Auth}/{action=LoginBasic}/{id?}");
}
else
{
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
}

if (authSettings.EnableJwt)
{
    app.MapControllers(); // �������� API
}

app.Run();
