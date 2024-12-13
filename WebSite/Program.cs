using WebAuthCoreBLL.Helpers;
using WebLoginBLL.Services;
using WebObjectsBLL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Настройка Kestrel для HTTP/HTTPS
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

// Регистрация AutoMapper
builder.Services.AddAutoMapper(
    typeof(WebLoginBLL.MappingProfile),
    typeof(WebObjectsBLL.MappingProfile)
    );

// Регистрация CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5000") // Укажите разрешенные домены
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Регистрация HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Чтение настроек аутентификации
var authSettings = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();
AuthHelper.ConfigureAuthentication(builder.Services, builder.Configuration, authSettings.EnableJwt, authSettings.EnableCookies);

// Настройка строки подключения
string connectionStringKey = authSettings.ServerName switch
{
    "Host" => "CRMConnection",
    "Developer" => "DeveloperConnection",
    "Vlad" => "VladConnection",
    _ => throw new ArgumentException($"Invalid ServerName: {authSettings.ServerName}")
};
var connectionStringCRM = builder.Configuration.GetConnectionString(connectionStringKey);

// Настройка DbContext
builder.Services.AddDbContext<DAL.Models.BankContext>(options =>
    options.UseSqlServer(connectionStringCRM));

// Добавление Razor Pages и настройка сессий
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware для обработки ошибок
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
// Включение CORS перед аутентификацией и авторизацией
app.UseCors("AllowSpecificOrigins");

// Аутентификация
if (authSettings.EnableCookies || authSettings.EnableJwt)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// Настройка маршрутов для MVC и API
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
    app.MapControllers(); // Маршруты API
}

app.Run();
