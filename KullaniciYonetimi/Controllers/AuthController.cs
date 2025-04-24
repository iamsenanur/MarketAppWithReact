using Microsoft.AspNetCore.Mvc;//API  kontrolc�lerini kullanmam�za yarayan k�t�phane
using Microsoft.IdentityModel.Tokens;//token imzalama, �ifreleme
using System.IdentityModel.Tokens.Jwt;//token olu�turma ve y�netme
using System.Security.Claims;//token i�ine kullan�c� bilgileri(claim) koyma
using System.Text; //secretkeyi byte dizisine �evirmek i�in
using KullaniciYonetimi.Models;
using Microsoft.AspNetCore.Identity;


namespace KullaniciYonetimi.Controllers;

[ApiController]//bu class bir web api kontrolc�s� oldu�unu belirtir.otomatik model do�rulama,400 hatas� �retme vs sa�lar
[Route("api/[controller]")]// /api/auth �eklinde eri�ilir



public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;//Kullan�c� ekleme, �ifre kontrol�, kullan�c�ya rol atama gibi i�lemleri yapan Identitiy servisi=UserManager
    private readonly IConfiguration _config;
    private readonly RoleManager<IdentityRole> _roleManager;

    //uygulama ayarlar�n�(�rn: appsettings.json okumak i�in kullan�lan IConfiguration servisi)
    public AuthController(UserManager<IdentityUser> userManager, IConfiguration config, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _config = config;
        _roleManager = roleManager;
    }


    [HttpPost("register")]//kay�t olma endpointi
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new IdentityUser { UserName = model.Username, Email = model.Email };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
           
            await _userManager.AddToRoleAsync(user, "M��teri");//kullan�c�ya varsay�lan rol atar


            //Kullan�c�lar �r�naramas� yapabilir ve �r�nlerilisteleyebilir
            Claim a = new Claim("urunAramaYapabilir","true");
            Claim b = new Claim("urunleriListeleyebilir", "true");
            await _userManager.AddClaimAsync(user, a);
            await _userManager.AddClaimAsync(user, b);
            return Ok("Kay�t Ba�ar�l�");
        }
        return BadRequest(result.Errors);
    }

    //Kullan�c� giri� endpointi
    //Kullan�c� ad� ve �ifre ile gir� yap�l�r

    [HttpPost("login")]//post iste�iyle /api/auth/login endpointine istek at�l�r.
   
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        //LoginModel tipi JSON' dan g�vdeye /[FromBody] map edilir

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

            //Kullan�c�n�n claims' lerini �ekip tokena ekleme
            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            // ROL bazl� claim'leri de token'a ekle
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
                message = "Giri� ba�ar�l�",
                token =token
            });
        }
        return Unauthorized("Kullan�c� ad� veya �ifre hatal�");
    }

    //Token olu�turma i�lemi
 
     private string GenerateToken(List<Claim> claims)
    {
        //JWT ayarlar�n� al(appsettingss.json' dan)
        var jwtSettings = _config.GetSection("JwtSettings");

        //Gizli anahtar� al ve byte dizisine �evir
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

        //Token' � imzalamak i�in kullan�lacak algoritma ve anahtar
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        //JWT Token' � olu�tur
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],//Token' � kim olu�turdu
            audience: jwtSettings["Audience"], //Token' � kim bulacak
            claims: claims, //��erik
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])), //Ge�erlilik s�resi
           // expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(60)),  //Ge�erlilik s�resi
            signingCredentials: creds //�mzalama bilgisi
            );

        //Token' � string(metin) haline getir ve d�nd�r
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}




