using WebAuthCoreBLL.Helpers;
using WebLoginBLL.Services;
using WebObjectsBLL.Services;
using Microsoft.EntityFrameworkCore;
using WebAuthCoreBLL.SecureByRoleClasses;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new LayoutByRoleAttribute());
});

// Íàñòðîéêà Kestrel äëÿ HTTP/HTTPS
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000); // HTTP
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.UseHttps(); // HTTPS
    });
});

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Регистрация сервисов MediaLib
builder.Services.AddScoped<MediaLib.Services.MediaGalleryService>();
builder.Services.AddScoped<MediaLib.Services.AvatarService>();
builder.Services.AddScoped<MediaLib.Services.DocumentService>();

// Регистрация сервисов Auth и Users
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RoleService>();

// Регистрация бизнес-логики
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<TermsAndRulesService>();
builder.Services.AddScoped<DepositService>();
builder.Services.AddScoped<CashbackService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<RegularPaymentService>();
builder.Services.AddScoped<CreditService>();
builder.Services.AddScoped<BankCardService>();
builder.Services.AddScoped<BankAccountService>();

// Регистрация сервисов для работы с типами карт и платежными системами
builder.Services.AddScoped<CardTypesService>();
builder.Services.AddScoped<PaymentSystemService>();






// Ðåãèñòðàöèÿ AutoMapper
builder.Services.AddAutoMapper(
    typeof(WebLoginBLL.MappingProfile),
    typeof(WebObjectsBLL.MappingProfile)
    );

// Ðåãèñòðàöèÿ CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5000") // Óêàæèòå ðàçðåøåííûå äîìåíû
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Ðåãèñòðàöèÿ HttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// ×òåíèå íàñòðîåê àóòåíòèôèêàöèè
var authSettings = builder.Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSettings>();
AuthHelper.ConfigureAuthentication(builder.Services, builder.Configuration, authSettings.EnableJwt, authSettings.EnableCookies);

// Íàñòðîéêà ñòðîêè ïîäêëþ÷åíèÿ
string connectionStringKey = authSettings.ServerName switch
{
    "Host" => "CRMConnection",
    "Developer" => "DeveloperConnection",
    "Vlad" => "VladConnection",
    _ => throw new ArgumentException($"Invalid ServerName: {authSettings.ServerName}")
};
var connectionStringCRM = builder.Configuration.GetConnectionString(connectionStringKey);

// Íàñòðîéêà DbContext
builder.Services.AddDbContext<DAL.Models.BankContext>(options =>
    options.UseSqlServer(connectionStringCRM));

// Äîáàâëåíèå Razor Pages è íàñòðîéêà ñåññèé
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware äëÿ îáðàáîòêè îøèáîê
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
// Âêëþ÷åíèå CORS ïåðåä àóòåíòèôèêàöèåé è àâòîðèçàöèåé
app.UseCors("AllowSpecificOrigins");

// Àóòåíòèôèêàöèÿ
if (authSettings.EnableCookies || authSettings.EnableJwt)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

// Íàñòðîéêà ìàðøðóòîâ äëÿ MVC è API
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
    app.MapControllers(); // Ìàðøðóòû API
}

app.Run();
