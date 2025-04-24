using KullaniciYonetimi.Data;
using KullaniciYonetimi.Models;
using KullaniciYonetimi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KullaniciYonetimi.Controllers
{
    [ApiController]//bu class bir web api kontrolcüsü olduğunu belirtir.otomatik model doğrulama,400 hatası üretme vs sağlar
    [Route("api/[controller]")]// /api/auth şeklinde erişilir
    public class AdminController: ControllerBase
    {
        //veritabanı işlemleri için ApplicationDbContex örneği
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(ApplicationDbContext context,UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //Kategori Ekleme
        [HttpPost("KategoriAdd")]
        [Authorize]
        public async Task<IActionResult> KategoriEkle([FromBody] KategoriEkleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var kategori = new KategoriListesi
            {
                KategoriAdi = model.KategoriAdi,
                isActive = true
            };
            //Veritabanına ekle
            _context.Kategoriler.Add(kategori);
            await _context.SaveChangesAsync();
            return Ok("Kategori başarıyla eklendi!");
        }
        //Kategori Silme--> soft delete

        [HttpDelete("KategoriDelete")]
        [Authorize]
        public async Task<IActionResult> KategoriSil(int id)
        {
            var kategori = await _context.Kategoriler.FindAsync(id);

            if (kategori == null)
                return NotFound("Kategori bulunamadı!");

            kategori.isActive = false;
            await _context.SaveChangesAsync();
            return Ok("Kategori başarıyla silindi!");
        }

        //isActive' i 1 yani aktif olan kategorileri listeletiyoruz
        [HttpGet("AktifKategoriler")]
        [Authorize]
        public async Task<IActionResult> GetAktifKategoriler()
        {
            var kategoriler = await _context.Kategoriler
                .Where(k => k.isActive)

                //(k=>k.isActive) aslında (k=>k.isActive==true) anlamına gelir
                .Select(k => new
                {
                    k.KategoriID,
                    k.KategoriAdi
                })
                .ToListAsync();
            return Ok(kategoriler);
        }

        [HttpGet("PasifKategoriler")]
        [Authorize]
        public async Task<IActionResult> GetPasifKategoriler()
        {
            var kategoriler = await _context.Kategoriler
                .Where(k => k.isActive == false)
                .Select(k => new
                {
                    k.KategoriID,
                    k.KategoriAdi
                })
                .ToListAsync();
            return Ok(kategoriler);
        }

        //Ürüne fiyat ekleme
        [HttpPost("UrunFiyatEkle")]
        [Authorize]
        public async Task<IActionResult> UrunFiyatEkle([FromBody] UrunFiyatEkleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //1. Daha önce bu ürün için aktif bir fiyat var mı? Varsa onu pasif yap
            var oncekiFiyat = await _context.FiyatListesi
                .FirstOrDefaultAsync(f => f.UrunBarkod == model.UrunID && f.isActive);
            if (oncekiFiyat != null)
            {
                oncekiFiyat.isActive = false;
            }
            //2. Yeni fiyatı oluştur ve aktif olarak işaretle


            var yeniFiyat = new UrunFyatListesi
            {
                UrunBarkod = model.UrunID,
                UrunFiyati = model.UrunFiyati,
                UrunFiyatTarihi = DateTime.Now,
                isActive = true
            };

            //Veritabanına ekle
            _context.FiyatListesi.Add(yeniFiyat);
            await _context.SaveChangesAsync();
            return Ok("Urun fiyatı başarıyla eklendi");
        }

        //Kullanıcı Listesi
        [HttpGet("KullanicilariListele")]
        [Authorize(Roles="Admin")]
        public IActionResult KullanicilariGetir()
        {
            var kullanicilar = _userManager.Users
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email
                }).ToList();
            return Ok(kullanicilar);
        }

        //Kullanıcı Güncelleme
        [HttpPut("KullaniciGuncelle/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> KullaniciGuncelle(string id, [FromBody] KullaniciGuncelleDto model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı!");

            //Güncellenebilir alanları ata
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok("Kullanıcı bilgileri başarıyla güncellendi.");

            return BadRequest(result.Errors);
           
        }


        //Kullanıcı Sil
        [HttpDelete("KullaniciSil/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> KullaniciSil(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunaamdı!");

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok("Kullanıcı başarıyla silindi.");

            return BadRequest(result.Errors);
        }

    }
}
