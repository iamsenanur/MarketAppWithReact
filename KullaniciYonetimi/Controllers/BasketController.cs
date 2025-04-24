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
    public class BasketController: ControllerBase
    {
        //veritabanı işlemleri için ApplicationDbContex örneği
        private readonly ApplicationDbContext _context;
         public BasketController(ApplicationDbContext context)
        {
            _context = context;
        }


        //Sepet Listesindeki SepetTutarını güncel tutmak için bir yardımcı metot yazıyoruz:

        private async Task GuncelleSepetTutari(int sepetId)
        {
            var sepet = await _context.Sepetler
                .Include(s => s.sepetIslemleri)
                .ThenInclude(i => i.Fiyat)
                .FirstOrDefaultAsync(s => s.SepetID == sepetId);
            if(sepet != null)
            {
                decimal toplamTutar = sepet.sepetIslemleri
                    .Sum(i => i.Adet * i.Fiyat.UrunFiyati);

                sepet.SepetTutari = (int)toplamTutar;
                await _context.SaveChangesAsync();
            }
        }

        [HttpPost("Add")]
        [Authorize]
        public async Task<IActionResult> SepeteUrunEkle([FromBody] SepetIslemleriDto model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            //Stok kontrolü için ürün ID'si
            var urunId = model.UrunID;

            //Mevcut stok kontrolü
            var toplamGiris = await _context.IslemlerListesi
                .Where(x => x.UrunID == urunId && x.IslemTipiID == 1) //1 giriş
                .SumAsync(x => x.Adet);
            var toplamCikis = await _context.IslemlerListesi
                .Where(x => x.UrunID == urunId && x.IslemTipiID == 2)//2 çıkış
                .SumAsync(x => x.Adet);

            var mevcutStok = toplamGiris - toplamCikis;

            //Sepetteki mevcut ürün miktarını da hesaba kat
            var sepettekiUrunAdeti = await _context.SepetIslemleri
                .Where(x => x.UrunBarkod == urunId && x.UserID == userId)
                .SumAsync(x => x.Adet);

            if(model.Adet + sepettekiUrunAdeti > mevcutStok)
            {
                return BadRequest($"Yetersiz stok! Mevcut: {mevcutStok}, Sepette zaten: {sepettekiUrunAdeti}, Eklenmek istenen:{model.Adet}");
            }


            // Kullanıcının sepeti var mı?
            var sepet = await _context.Sepetler
                .Include(s => s.sepetIslemleri)
                .FirstOrDefaultAsync(s=> s.SepetID==model.SepetID);

            if (sepet == null)
            {
                sepet = new SepetListesi
                {
                    SepetTarihi = DateTime.Now,
                    SepetTutari=0
                };
                _context.Sepetler.Add(sepet);
                await _context.SaveChangesAsync();
            }

            // Sepette ürün var mı kontrolü
            var mevcut = await _context.SepetIslemleri
                .FirstOrDefaultAsync(si => si.SepetID == sepet.SepetID && si.UserID == userId && si.UrunBarkod == model.UrunID);

            if (mevcut != null)
            {
                mevcut.Adet += model.Adet;
            }
            else
            {
                var yeni = new SepetIslemleri
                {
                    SepetID = model.SepetID,
                    UserID = userId,
                    //buradaki UrunBarkod UrunListesindeki Urunbarkod değil ,UrunID' ye karşılık gelen kısımdır!!!!
                    UrunBarkod = model.UrunID,
                    UrunFiyatID = model.UrunFiyatID,
                    Adet = model.Adet
                };
                _context.SepetIslemleri.Add(yeni);
            }

            await _context.SaveChangesAsync();
            await GuncelleSepetTutari(sepet.SepetID);
            return Ok("Ürün sepete eklendi.");
        }

        [HttpDelete("Remove")]
        [Authorize]
        public async Task<IActionResult> SepettenUrunCikar([FromBody] SepettenSilDto model)
        {
            var userID = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userID == null)
                return Unauthorized();

            //Sepette ilgili ürün var mı?
            var islem = await _context.SepetIslemleri
                .FirstOrDefaultAsync(x => x.SepetID == model.SepetID &&
                                     x.UserID == userID &&
                                     x.UrunBarkod == model.UrunID);
            if (islem == null)
                return NotFound("Sepette böyle bir ürün yok!");

            //Slinecek adet sepettekinden fazlaysa hata ver
            if (model.Adet > islem.Adet)
            {
                return BadRequest($"Sepette bu üründen yalnızca {islem.Adet} adet var");
            }

            //eşitse--> tamamen sil
            if(model.Adet==islem.Adet)
            {
                _context.SepetIslemleri.Remove(islem);
            }
            else
            {
                //küçükse--> sadece adeti azaltırız
                islem.Adet -= model.Adet;
            }
            await _context.SaveChangesAsync();

            //Sepet boşaldıysa sepeti de sil
            var kalanUrun = await _context.SepetIslemleri
                .AnyAsync(x => x.SepetID == model.SepetID);

            if (!kalanUrun)
            {
                var sepet = await _context.Sepetler.FindAsync(model.SepetID);
                if(sepet != null)
                {
                    _context.Sepetler.Remove(sepet);
                    await _context.SaveChangesAsync();
                }
            }

            await GuncelleSepetTutari(model.SepetID);
                return Ok("Ürün sepetten çıkarıldı!");
        }

        [HttpGet("List/{sepetId}")]
        [Authorize]
        public async Task<IActionResult> SepetiListele(int sepetId)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            var sepet = await _context.Sepetler
                .Include(s => s.sepetIslemleri)
                .ThenInclude(i => i.Urun)
                .Include(s => s.sepetIslemleri)
                .ThenInclude(i => i.Fiyat)
                .FirstOrDefaultAsync(s => s.SepetID == sepetId &&
                                        s.sepetIslemleri.Any(x => x.UserID == userId));
            if (sepet == null)
                return NotFound("Sepet bulunamadı!");

            var detaylar = sepet.sepetIslemleri.Select(x => new
            {
                UrunAdi=x.Urun.UrunAdi,
                Adet=x.Adet,
                Fiyat=x.Fiyat.UrunFiyati,
                Toplam=x.Adet*x.Fiyat.UrunFiyati

            });

            var toplamTutar = detaylar.Sum(x => x.Toplam);
            return Ok(new
            {
                SepetID=sepet.SepetID,
                Tarih=sepet.SepetTarihi,
                ToplamTutar=toplamTutar,
                Urunler=detaylar
            });
        }

        [HttpPost("Update")]
        [Authorize]
        public async Task<IActionResult> SepetUrunGuncelle([FromBody] SepetGuncelleDto model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            //İlgili ürün sepette var mı?
            var islem = await _context.SepetIslemleri
                .FirstOrDefaultAsync(x => x.SepetID == model.SepetID &&
                                     x.UserID == userId &&
                                     x.UrunBarkod == model.UrunID);
            if (islem == null)
                return NotFound("Sepette bu ürün bulunamadı!");

            //Adet kontrolü
            if (model.YeniAdet <= 0)
            {
                return BadRequest("Adet en az 1 olmalı");
            }

            //Adeti güncelle
            islem.Adet = model.YeniAdet;
            await _context.SaveChangesAsync();
            await GuncelleSepetTutari(model.SepetID);
            return Ok("Sepetteki ürün adeti güncellendi!");
        }
    }
}
