using KullaniciYonetimi.Data;
using KullaniciYonetimi.Models;
using KullaniciYonetimi.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KullaniciYonetimi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController: ControllerBase
    {
        //veritabanı işlemleri için ApplicationDbContex örneği
        private readonly ApplicationDbContext _context;

        //Dependency Injection ile ApplicationDbContext'i alıyoruz
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        //Admin Ürün Ekleyebilmeli!
        [HttpPost("Add")]//POST isteği ile /api/product/add adresine erişilir
        [Authorize]
        public async Task<IActionResult> AddProduct([FromBody] UrunEkleDto model)
        {
            var hasClaim = User.HasClaim(c => c.Type == "urunEklemeYetkisi" && c.Value == "true");
            if (!hasClaim)
            {
                return Forbid("Ürün ekleme yetkiniz yok.");
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kategori kontrolü
            var kategori = await _context.Kategoriler.FindAsync(model.UrunKategoriID);
            if (kategori == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            // DTO'dan alınan verileri model'e dönüştürüyoruz
            var urun = new UrunListesi
            {
                UrunAdi = model.UrunAdi,
                UrunBarkod = model.UrunBarkod,
                UrunKategoriID = model.UrunKategoriID,
                isActive = model.isActive,
                Kategori = kategori
            };

            // Veritabanına ekle
            _context.Urunler.Add(urun);
            await _context.SaveChangesAsync();

            return Ok("Ürün başarıyla eklendi.");
        }


        [AllowAnonymous]
        //Kullanıcılar Tüm Ürünleri Listeleyebilmeli!
        [HttpGet("GetAll")]//Get isteği ile /api/product/getall adresine erişir
        public async Task<IActionResult> GetAllProduct()
        {
            //Aktif ürünleri getir
            var products = await _context.Urunler
                .Where(p => p.isActive == true)
                .ToListAsync();

            //Eğer ürün yoksa 404 not found döndür
            if (products.Count == 0)
                return NotFound("Ürün Bulunamadı");

            //Ürünleri döndür
            return Ok(products);
        }



        //Kullanıcılar Ürün Arama Yapabilmeli!
        [AllowAnonymous]
        [HttpGet("Search/{name}")]//Get isteği ile /api/product/search/{name} adresine erişilir
        public async Task<IActionResult> Search(string name)
        {
            //Aranan isme uygun aktif ürünleri getir
            var products = await _context.Urunler
                .Where(p => p.UrunAdi.Contains(name) && p.isActive == true)
                .ToListAsync();

            //Eğer ürün yoksa 404 notfound döndür
            if (products.Count == 0)
                return NotFound("Arama kriterlerine uygun ürün bulunamadı");


            //Eşleşen ürünleri döndür
            return Ok(products);

        }

        //Admin Ürün Güncelleyebilmeli!
     
        [HttpPut("Update/{id}")] //PUT isteği ile /api/product/update/{id} adresine erişilir
        [Authorize]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UrunGuncelleDto model)
        {
            var hasClaim = User.HasClaim(c => c.Type == "urunGuncellemeYetkisi" && c.Value == "true");
            if (!hasClaim)
            {
                return Forbid("Ürün güncelleme yetkiniz yok.");
            }
            if (id != model.UrunID)
            {
                return BadRequest("ID eşleşmiyor.");
            }

            // Güncellenecek ürünü bul
            var product = await _context.Urunler.FindAsync(id);
            if (product == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            // Kategori kontrolü
            var kategori = await _context.Kategoriler.FindAsync(model.UrunKategoriID);
            if (kategori == null)
            {
                return NotFound("Kategori bulunamadı.");
            }

            // DTO'dan alınan verileri model'e aktar
            product.UrunAdi = model.UrunAdi;
            product.UrunBarkod = model.UrunBarkod;
            product.UrunKategoriID = model.UrunKategoriID;
            product.isActive = model.isActive;
            product.Kategori = kategori;

            // Veritabanında güncelle
            _context.Urunler.Update(product);
            await _context.SaveChangesAsync();

            return Ok("Ürün başarıyla güncellendi.");
        }


        //Admin Ürün Silebilmeli!
     
        [HttpDelete("Delete/{id}")] //delete isteği ile /api/product/delete/{id} adresine erişilir
        [Authorize]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var hasClaim = User.HasClaim(c => c.Type == "urunSilmeYetkisi" && c.Value == "true");
            if (!hasClaim)
            {
                return Forbid("Ürün silme yetkiniz yok.");
            }

            //Silinecek ürünü bul
            var product = await _context.Urunler.FindAsync(id);
            if (product == null)
                return NotFound("Ürün bulunamadı");

            //Ürünü kaldır
            _context.Urunler.Remove(product);

            //Değişiklikleri kaydet

            await _context.SaveChangesAsync();
            return Ok("Ürün başarıyla silindi.");
        }


        //Kullanıcılar Ürün Detaylarını Getirtebilmeli!
       
        [HttpGet("GetById/{id}")]//GET isteği ile /api/product/getbyid/{id} adresine erişilir
        public async Task<IActionResult> GetProductById(int id)
        {

            //Belirtilen id' ye sahip aktif ürünü getir
            var product = await _context.Urunler
                .Where(p => p.UrunID == id && p.isActive == true)
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound("Ürün bulunamadı");

            //Ürünü döndür

            return Ok(product);
        }

        //Stok ekleme(totelEklenen=girenUrunSayisi)
        [HttpPost("StockAdd")]
        [Authorize]
        public async Task<IActionResult> AddStock([FromBody] IslemEkleDto model)
        {
            var hasClaim = User.HasClaim(c => c.Type == "stokYonetmeYetkisi" && c.Value == "true");
            if (!hasClaim)
            {
                return Forbid("Stok yönetme yetkiniz yok.");
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //1. Ürün var mı kontrol et
            var urun = await _context.Urunler.FindAsync(model.UrunID);
            if (urun == null)
                return NotFound("Ürün Bulunamadı!");

            //2. İşlem tipi geçerli mi (1:giriş, 2:çıkış)
            var islemTipi = await _context.IslemTipiListesi.FindAsync(model.IslemTipiID);
            if (islemTipi == null)
                return NotFound("İşlem tipi geçersiz!");

            //3. Giriş yapan kullanıcının ID' sini al
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı bilgisi alınamadı!");

            //Eğer islemTipi=2 yani çıkışsa stok kontrolü yapılmalı!

            if (model.IslemTipiID == 2)
            {
                var toplamGiris = await _context.IslemlerListesi
                .Where(x => x.UrunID == urun.UrunID && x.IslemTipiID == 1)
                .SumAsync(x => x.Adet);
                var toplamCikis = await _context.IslemlerListesi
                 .Where(x => x.UrunID == urun.UrunID && x.IslemTipiID == 2)
                 .SumAsync(x => x.Adet);
                var mevcutStok = toplamGiris - toplamCikis;
                if (model.Adet > mevcutStok)
                    return BadRequest($"Yetersiz stok! Mevcut: {mevcutStok}, İstenen: {model.Adet}");

            }

            //4. İşlem kaydını oluştur
            var islem = new IslemlerListesi
            {
                UrunID = model.UrunID,
                IslemTipiID = model.IslemTipiID,
                Adet = model.Adet,
                IslemTarihi = DateTime.Now,
                UserID = userId
            };

            //5. İşlem kaydını veritabanına ekle
            await _context.IslemlerListesi.AddAsync(islem);
            await _context.SaveChangesAsync();

            return Ok("Stok işlemi başarıyla kaydedildi!");
        }


        //Stok Gösterimi
        [HttpGet("StokBarkodla/{barkod}")]
        public async Task<IActionResult> GetStokByBarkod(string barkod)
        {
            var urun = await _context.Urunler.FirstOrDefaultAsync(u => u.UrunBarkod == barkod);
            if (urun == null)
                return NotFound("Barkoda ait ürün bulunamadı!");

            var toplamGiris = await _context.IslemlerListesi
                .Where(x => x.UrunID == urun.UrunID && x.IslemTipiID == 1)
                .SumAsync(x => x.Adet);
            var toplamCikis = await _context.IslemlerListesi
                .Where(x => x.UrunID == urun.UrunID && x.IslemTipiID == 2)
                .SumAsync(x => x.Adet);

            var dto = new StokBilgiDto
            {
                UrunID=urun.UrunID,
                UrunAdi=urun.UrunAdi,
                Barkod=urun.UrunBarkod,
                MevcutStok=toplamGiris-toplamCikis
            };
            return Ok(dto);
        }

    }


}
