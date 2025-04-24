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
    public class PaymentController: ControllerBase
    {
        //veritabanı işlemleri için ApplicationDbContex örneği
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("OdemeYap")]
        [Authorize]
        public async Task<IActionResult> OdemeYap([FromBody] OdemeTalepDto dto)
        {
            var siparis = await _context.Siparisler.FindAsync(dto.SiparisID);
            if (siparis == null)
                return NotFound("Sipariş bulunamadı.");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı doğrulanamadı.");

            // 1. Stratejiyi belirle
            IOdemeStratejisi strateji = dto.OdemeYontemi.ToLower() switch
            {
                "kredi" => new KrediKartiOdeme(),
                "paypal" => new PayPalOdeme(),
                _ => throw new Exception("Geçersiz ödeme yöntemi")
            };

            // 2. OdemeContext'e stratejiyi ata ve ödeme yap
            var odemeContext = new OdemeContext();
            odemeContext.SetOdemeStratejisi(strateji);
            odemeContext.OdemeYap(dto.Tutar); // Konsola "xxx TL ... ile ödendi" yazar

            // 3. Rastgele başarı durumu
            var basariliMi = new Random().Next(0, 2) == 0;
            var odemeDurumu = basariliMi ? "Başarılı" : "Başarısız";

            // 4. Ödeme durumunu kaydet
            var odemeKaydi = new SiparisOdemeDurumListesi
            {
                SiparisID = dto.SiparisID,
                UserID = userId,
                SiparisOdemeDurumu = odemeDurumu,
                SiparisOdemeDurumTarihi = DateTime.Now
            };

            _context.OdemeDurumListeleri.Add(odemeKaydi);
            await _context.SaveChangesAsync();

            return Ok($"Ödeme sonucu: {odemeDurumu}");
        }


    }
}
