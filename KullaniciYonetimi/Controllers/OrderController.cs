using KullaniciYonetimi.Data;
using KullaniciYonetimi.Models;
using KullaniciYonetimi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KullaniciYonetimi.Controllers
{
    [ApiController]//bu class bir web api kontrolcüsü olduğunu belirtir.otomatik model doğrulama,400 hatası üretme vs sağlar
    [Route("api/[controller]")]// /api/auth şeklinde erişilir
    public class OrderController:ControllerBase
    {
        //veritabanı işlemleri için ApplicationDbContex örneği
        private readonly ApplicationDbContext _context;
        
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }


     

        //girilen sepetID' ye göre o sepeti siparişe dönüştürme
        [HttpPost("SiparisAdd")]
        [Authorize]
        public async Task<IActionResult> SiparisEkle([FromBody] SiparisOlusturDto model)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Kullanıcının ID' sini al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı doğrulananamadı!");

            //Sepeti bul!
            var sepet = await _context.Sepetler
               
                .FirstOrDefaultAsync(s => s.SepetID == model.SepetID);
            if (sepet == null)
                return NotFound("Sepet Bulunamadı");


            var siparis = new SiparisListesi
            {
                SepetID = sepet.SepetID,
                UserID=userId,
                SiparisFiyatiTutar=sepet.SepetTutari,
                SiparisTarihi=DateTime.Now
            };

            //Veritabanına ekle
            _context.Siparisler.Add(siparis);
    
            
            //kaydet
            await _context.SaveChangesAsync();
            return Ok("Sepetiniz başarıyla siparişe dönüştürüldü");
        }

        //Kullanıcılar sipariş geçmişlerini görüntüleyebilmeli
        [HttpGet("Siparislerim")]
        [Authorize]
        public async Task<IActionResult> SiparisGecmisi()
        {
            //Kullanıcıyı al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı doğrulanamadı!");

            //Siparişleri getir
            var siparisler = await _context.Siparisler
                .Where(s => s.UserID == userId)
                .Include(s => s.Sepet)
                .Select(s => new
                {
                    s.SiparisID,
                    s.SiparisFiyatiTutar,
                    s.SiparisTarihi,
                    s.SepetID,
                    SepetTarihi = s.Sepet.SepetTarihi,
                    SepetTutari = s.Sepet.SepetTutari
                })
                .OrderByDescending(s => s.SiparisTarihi)
                .ToListAsync();
            return Ok(siparisler);
        }

        //Ödeme durumu ekleme
        [HttpPost("OdemeDurumAdd")]
        [Authorize]
        public async Task<IActionResult> OdemeDurumuEkle([FromBody] OdemeDurumuEkleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Kullanıcının ID' sini al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı doğrulanamadı!");

            //Siparişi bul
            var siparis = await _context.Siparisler
                .FirstOrDefaultAsync(s => s.SiparisID == model.SiparisID);
            if(siparis==null)
            return NotFound("Sipariş bulunamadı!");

            var odemeDurumu = new SiparisOdemeDurumListesi
            {
                SiparisID = siparis.SiparisID,
                UserID=userId,
                SiparisOdemeDurumu=model.SiparisOdemeDurumu,
                SiparisOdemeDurumTarihi=DateTime.Now
            };

            //Ekleme ve kaydetme kısmı ekleniyor:

            _context.OdemeDurumListeleri.Add(odemeDurumu);
            await _context.SaveChangesAsync();
            return Ok("Ödeme durumu başarıyla kaydedildi!");
        }

        //Teslimat durumu ekleme
        [HttpPost("TeslimatDurumAdd")]

        public async Task<IActionResult> TeslimatDurumuEkle([FromBody] TeslimatDurumuEkleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            //Kullanıcının ID' sini al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı doğrulanamadı!");

            //Siparişi bul
            var siparis = await _context.Siparisler
                .FirstOrDefaultAsync(s => s.SiparisID == model.SiparisID);

            if (siparis == null)
                return NotFound("Sipariş bulunamadı!");

            var teslimatDurumu = new SiparisTeslimatDurumListesi
            {
                SiparisID=siparis.SiparisID,
                UserID=userId,
                SiparisTeslimatDurumu=model.SiparisTeslimatDurumu,
                SiparisTeslimatDurumTarihi=DateTime.Now
            };

            //Eklemeve ve kaydetme kısmı
            _context.TeslimatDurumListeleri.Add(teslimatDurumu);
            await _context.SaveChangesAsync();
            return Ok("Teslimat duurmu başarıyla kaydedildi!");
        }


    }
}
