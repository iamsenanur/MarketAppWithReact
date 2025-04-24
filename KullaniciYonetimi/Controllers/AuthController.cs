using Microsoft.AspNetCore.Mvc;//API  kontrolcülerini kullanmamýza yarayan kütüphane
using Microsoft.IdentityModel.Tokens;//token imzalama, þifreleme
using System.IdentityModel.Tokens.Jwt;//token oluþturma ve yönetme
using System.Security.Claims;//token içine kullanýcý bilgileri(claim) koyma
using System.Text; //secretkeyi byte dizisine çevirmek için
using KullaniciYonetimi.Models;
using Microsoft.AspNetCore.Identity;


namespace KullaniciYonetimi.Controllers;

[ApiController]//bu class bir web api kontrolcüsü olduðunu belirtir.otomatik model doðrulama,400 hatasý üretme vs saðlar
[Route("api/[controller]")]// /api/auth þeklinde eriþilir



public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;//Kullanýcý ekleme, þifre kontrolü, kullanýcýya rol atama gibi iþlemleri yapan Identitiy servisi=UserManager
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;

    //uygulama ayarlarýný(örn: appsettings.json okumak için kullanýlan IConfiguration servisi)
    public AuthController(UserManager<IdentityUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
    }


    [HttpPost("register")]//kayýt olma endpointi
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new IdentityUser { UserName = model.Username, Email = model.Email };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
           
            await _userManager.AddToRoleAsync(user, "Müþteri");//kullanýcýya varsayýlan rol atar


            //Kullanýcýlar ürünaramasý yapabilir ve ürünlerilisteleyebilir
            Claim a = new Claim("urunAramaYapabilir","true");
            Claim b = new Claim("urunleriListeleyebilir", "true");
            await _userManager.AddClaimAsync(user, a);
            await _userManager.AddClaimAsync(user, b);
            return Ok("Kayýt Baþarýlý");
        }
        return BadRequest(result.Errors);
    }

    //Kullanýcý giriþ endpointi
    //Kullanýcý adý ve þifre ile girþ yapýlýr

    [HttpPost("login")]//post isteðiyle /api/auth/login endpointine istek atýlýr.
   
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        //LoginModel tipi JSON' dan gövdeye /[FromBody] map edilir

        var user = await _userManager.FindByNameAsync(model.Username);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password)) 
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            foreach (var role in userRoles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            //Kullanýcýnýn claims' lerini çekip tokena ekleme
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // ROL bazlý claim'leri de token'a ekle
            foreach (var role in userRoles)
            {
                var identityRole = await _roleManager.FindByNameAsync(role);
                if (identityRole != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(identityRole);
                    claims.AddRange(roleClaims);
                }
            }

            var token = GenerateToken(claims);
            return Ok(new {
                message = "Giriþ baþarýlý",
                token =token
            });
        }
        return Unauthorized("Kullanýcý adý veya þifre hatalý");
    }

    //Token oluþturma iþlemi
 
     private string GenerateToken(List<Claim> claims)
    {
        //JWT ayarlarýný al(appsettingss.json' dan)
        var jwtSettings = _config.GetSection("JwtSettings");

        //Gizli anahtarý al ve byte dizisine çevir
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

        //Token' ý imzalamak için kullanýlacak algoritma ve anahtar
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        //JWT Token' ý oluþtur
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],//Token' ý kim oluþturdu
            audience: jwtSettings["Audience"], //Token' ý kim bulacak
            claims: claims, //Ýçerik
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])), //Geçerlilik süresi
           // expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(60)),  //Geçerlilik süresi
            signingCredentials: creds //Ýmzalama bilgisi
            );

        //Token' ý string(metin) haline getir ve döndür
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}




