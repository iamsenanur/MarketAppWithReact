using KullaniciYonetimi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KullaniciYonetimi.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        
        public DbSet<KategoriListesi> Kategoriler { get; set; }
        public DbSet<UrunListesi> Urunler { get; set; }
        public DbSet<IslemTipiListesi> IslemTipiListesi { get; set; } // 🔧 DÜZENLENDİ
        public DbSet<IslemlerListesi> IslemlerListesi { get; set; }
   
        public DbSet<UrunFyatListesi> FiyatListesi { get; set; }

        public DbSet<SepetListesi> Sepetler { get; set; }

        public DbSet<SepetIslemleri> SepetIslemleri { get; set; }
        public DbSet<SiparisListesi> Siparisler { get; set; }

        public DbSet<SiparisOdemeDurumListesi> OdemeDurumListeleri { get; set; }
        public DbSet<SiparisTeslimatDurumListesi> TeslimatDurumListeleri { get; set; }
        //public object IslemTipiListesi { get; internal set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            // SiparisListesi için SiparisFiyatiTutar alanının veri tipi
            builder.Entity<SiparisListesi>()
                .Property(s => s.SiparisFiyatiTutar)
                .HasColumnType("decimal(18,2)");  // decimal(18,2) tipi

            // UrunFyatListesi için UrunFiyati alanının veri tipi
            builder.Entity<UrunFyatListesi>()
                .Property(u => u.UrunFiyati)
                .HasColumnType("decimal(18,2)");  // decimal(18,2) tipi


            // IslemlerListesi ile Urun ve User ilişkisi
            builder.Entity<IslemlerListesi>()
                .HasOne(i => i.Urun)
                .WithMany()
                .HasForeignKey(i => i.UrunID)
                .OnDelete(DeleteBehavior.NoAction); // Veritabanında ilişkili ürün silindiğinde, islemler de silinir.

            builder.Entity<IslemlerListesi>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserID)
                .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silindiğinde islemler de silinir.

            builder.Entity<IslemlerListesi>()
                .HasOne(i => i.IslemTipi)
                .WithMany(t => t.Islemler)
                .HasForeignKey(i => i.IslemTipiID)
                .OnDelete(DeleteBehavior.NoAction); // İşlem tipi silindiğinde islemler de silinir.

            // IslemTipiListesi ile Islemler ilişkisi
            builder.Entity<IslemTipiListesi>()
                .HasMany(i => i.Islemler)
                .WithOne(i => i.IslemTipi)
                .HasForeignKey(i => i.IslemTipiID)
                .OnDelete(DeleteBehavior.NoAction); // İşlem tipi silindiğinde ilgili islemler silinir.

            // UrunListesi ile KategoriListesi ilişkisi
            builder.Entity<UrunListesi>()
                .HasOne(u => u.Kategori)
                .WithMany(k => k.Urunler)
                .HasForeignKey(u => u.UrunKategoriID)
                .OnDelete(DeleteBehavior.NoAction); // Kategori silindiğinde ilgili ürünler de silinsin.

            // UrunFyatListesi ile UrunListesi ilişkisi
            builder.Entity<UrunFyatListesi>()
                .HasOne(u => u.Urun)
                .WithMany()
                .HasForeignKey(u => u.UrunBarkod)
                .OnDelete(DeleteBehavior.NoAction); // Ürün silindiğinde fiyatları da silinsin.

            // KategoriListesi ile UrunListesi ilişkisi (Kategori silindiğinde ilgili ürünler de silinsin)
            builder.Entity<KategoriListesi>()
                .HasMany(k => k.Urunler)
                .WithOne(u => u.Kategori)
                .HasForeignKey(u => u.UrunKategoriID)
                .OnDelete(DeleteBehavior.NoAction);
            // SepetIslemleri ile SepetListesi ilişkisi
            builder.Entity<SepetIslemleri>()
                .HasOne(s => s.Sepet)
                .WithMany(sep => sep.sepetIslemleri)
                .HasForeignKey(s => s.SepetID)
                .OnDelete(DeleteBehavior.NoAction); // Sepet silindiğinde, sepet işlemleri de silinsin.

            // SepetIslemleri ile User ilişkisi
            builder.Entity<SepetIslemleri>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silindiğinde, sepet işlemleri de silinsin.

            // SepetIslemleri ile Urun ilişkisi
            builder.Entity<SepetIslemleri>()
                .HasOne(s => s.Urun)
                .WithMany()
                .HasForeignKey(s => s.UrunBarkod)
                .OnDelete(DeleteBehavior.NoAction); // Ürün silindiğinde, sepet işlemleri silinmesin.

            // SepetIslemleri ile Fiyat ilişkisi
            builder.Entity<SepetIslemleri>()
                .HasOne(s => s.Fiyat)
                .WithMany()
                .HasForeignKey(s => s.UrunFiyatID)
                .OnDelete(DeleteBehavior.NoAction); // Fiyat silindiğinde, sepet işlemleri silinmesin

            builder.Entity<SiparisListesi>()
                .HasOne(s => s.Sepet)
                .WithMany()
                .HasForeignKey(s => s.SepetID)
                .OnDelete(DeleteBehavior.Restrict); // Sepet silindiğinde sipariş de silinmesin



            // SiparisListesi ile User ilişkisi
            builder.Entity<SiparisListesi>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde, sipariş de silinsin.

            // SiparisOdemeDurumListesi ile Siparis ilişkisi
            builder.Entity<SiparisOdemeDurumListesi>()
                .HasOne(s => s.Siparis)
                .WithMany()
                .HasForeignKey(s => s.SiparisID)
                .OnDelete(DeleteBehavior.Restrict); // Sipariş silindiğinde ödeme durumu silinsin.


            // SiparisOdemeDurumListesi ile User ilişkisi
            builder.Entity<SiparisOdemeDurumListesi>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silindiğinde, ödeme durumu da silinsin.

            // SiparisTeslimatDurumListesi ile Siparis ilişkisi
            builder.Entity<SiparisTeslimatDurumListesi>()
                .HasOne(s => s.Siparis)
                .WithMany()
                .HasForeignKey(s => s.SiparisID)
                .OnDelete(DeleteBehavior.Restrict); // Sipariş silindiğinde, teslimat durumu da silinsin.

            // SiparisTeslimatDurumListesi ile User ilişkisi
            builder.Entity<SiparisTeslimatDurumListesi>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.NoAction); // Kullanıcı silindiğinde, teslimat durumu da silinsin.

        }

    }
}
