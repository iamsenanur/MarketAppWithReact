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

// ASP.NET CORE uygulamas�n�n temel yap�land�r�c�s�d�r. Servisleri, ayarlar� ve ortam� buradan y�netiriz
var builder = WebApplication.CreateBuilder(args);

//MSSQL ba�lant�s�
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity kullan�m�(Identity tabloalr�n� olu�turur)
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
//JWT Ayarlar�n� okuyoruz
//Bu bilgiler appsettings.json' dan geliyor
//Token kontrol� i�in gerekli olan ayarlar burada yap�l�r






var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

//JWT Authentication Sistemini Tan�t�yoruz
//Bu sayede gelen isteklerde token olup olmad���n� ve ge�erli olup olmad���n� kontrol eder


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
        ValidateIssuer=true,//Token kimin taraf�ndan �retildi kontrol edilir
        ValidateAudience=true, //Token kimin i�in �retildi kontrol edilir
        ValidateLifetime=true, //Token s�resi ge�mi� mi kontrol edilir
        ValidateIssuerSigningKey=true, //Token�n imzas� do�ru mu kontrol edilir

            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };

    });
//Gerekli temel servisleri ekliyoruz
//Controller deste�i
//Swagger (dok�mantasyon ve test i�in)

//CORS ayar�: React taraf�na izin veriyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactCors", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // ? sadece HTTP yeterli
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});




builder.Services.AddControllers();//api controller deste�ini ekler
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

//swagger ui(api test aray�z�) sa�lar.

var app = builder.Build();//uygulama olu�turuluyor


//IdentityRole Start
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = new[] { "Admin", "M��teri" };

    foreach (var role in roles)
    {
        var roleExist = await roleManager.RoleExistsAsync(role);
        if (!roleExist)
            await roleManager.CreateAsync(new IdentityRole(role));    
    }
    //Admin rol� i�in claim ekleme i�lemi
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
//Uygulama ortam� kontrol�
//E�er development ortam�ndaysak, swagger aray�z� aktif olsun
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Middlewareleri ekliyoruz
//Middleware=gelen isteklere ne olacak s�ras�n� belirler
//HTTPS' e y�nlendirir
//JWT token kontrol�n� yap(UseAuthentication)
//Controller� �al��t�r

app.UseHttpsRedirection();//HTTP isteklerini otomatik olarak HTTPS' e �evirir


// CORS ayar�n� middleware olarak kullan
app.UseCors("ReactCors");

app.UseJwtMiddleware();

app.UseAuthentication();//JWT token kontrol�n� yapar(zorunlu!)
app.UseAuthorization();

app.MapControllers();//Controller' lar� tan�t�r(�rn: /api/auth/login)


app.Run();//Uygulamay� ba�lat
