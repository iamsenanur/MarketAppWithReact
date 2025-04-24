using KullaniciYonetimi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Security.Claims;
using System.Data;
using Microsoft.OpenApi.Models;
using KullaniciYonetimi.Middlewares;

// ASP.NET CORE uygulamasýnýn temel yapýlandýrýcýsýdýr. Servisleri, ayarlarý ve ortamý buradan yönetiriz
var builder = WebApplication.CreateBuilder(args);

//MSSQL baðlantýsý
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity kullanýmý(Identity tabloalrýný oluþturur)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//JWT Ayarlarýný okuyoruz
//Bu bilgiler appsettings.json' dan geliyor
//Token kontrolü için gerekli olan ayarlar burada yapýlýr






var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

//JWT Authentication Sistemini Tanýtýyoruz
//Bu sayede gelen isteklerde token olup olmadýðýný ve geçerli olup olmadýðýný kontrol eder


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
        ValidateIssuer=true,//Token kimin tarafýndan üretildi kontrol edilir
        ValidateAudience=true, //Token kimin için üretildi kontrol edilir
        ValidateLifetime=true, //Token süresi geçmiþ mi kontrol edilir
        ValidateIssuerSigningKey=true, //Tokenýn imzasý doðru mu kontrol edilir

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

    });
//Gerekli temel servisleri ekliyoruz
//Controller desteði
//Swagger (dokümantasyon ve test için)

//CORS ayarý: React tarafýna izin veriyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // ? sadece HTTP yeterli
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});




builder.Services.AddControllers();//api controller desteðini ekler
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); //Swagger' a endpoint bilgilerini verir.
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SMART API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Fatih Akbaba",
            Email = "fatih.akbaba@royrobson.com",
        },
        Description = "SMART API Swagger Surface",
    });
});

//swagger ui(api test arayüzü) saðlar.

var app = builder.Build();//uygulama oluþturuluyor


//IdentityRole Start
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "Admin", "Müþteri" };

    foreach (var role in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);
        if (!roleExist)
            await roleManager.CreateAsync(new IdentityRole(role));    
    }
    //Admin rolü için claim ekleme iþlemi
    var adminRole = await roleManager.FindByNameAsync("Admin");
    if (adminRole != null)
    {
        var claims = new List<Claim>
        {
            new Claim("urunEklemeYetkisi","true"),
            new Claim("urunGuncellemeYetkisi","true"),
            new Claim("urunSilmeYetkisi", "true"),
            new Claim("stokYonetmeYetkisi","true")
        };

        //mevcut claimleri kontrol ederek tekrar eklemiyoruz
        var existingClaims = await roleManager.GetClaimsAsync(adminRole);
        foreach(var claim in claims)
        {
            if(!existingClaims.Any(c=>c.Type ==claim.Type && c.Value==claim.Value))
            {
                await roleManager.AddClaimAsync(adminRole,claim);
            }
        }
    }
  
}
//IdentityRole End
//Uygulama ortamý kontrolü
//Eðer development ortamýndaysak, swagger arayüzü aktif olsun
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Middlewareleri ekliyoruz
//Middleware=gelen isteklere ne olacak sýrasýný belirler
//HTTPS' e yönlendirir
//JWT token kontrolünü yap(UseAuthentication)
//Controllerý çalýþtýr

app.UseHttpsRedirection();//HTTP isteklerini otomatik olarak HTTPS' e çevirir


// CORS ayarýný middleware olarak kullan
app.UseCors("ReactCors");

app.UseJwtMiddleware();

app.UseAuthentication();//JWT token kontrolünü yapar(zorunlu!)
app.UseAuthorization();

app.MapControllers();//Controller' larý tanýtýr(örn: /api/auth/login)


app.Run();//Uygulamayý baþlat
