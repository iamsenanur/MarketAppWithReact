using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("SepetIslemleri")]
    public class SepetIslemleri
    {
        [Key]
        public int SepetIslemleriID { get; set; }

    

        [ForeignKey("Sepet")]
        public int SepetID { get; set; }
        public SepetListesi Sepet { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }


        [ForeignKey("Urun")]
        public int UrunBarkod { get; set; }
        public UrunListesi Urun { get; set; }

        [ForeignKey("Fiyat")]
        public int UrunFiyatID { get; set; }
        public UrunFyatListesi Fiyat { get; set; }

        public int Adet { get; set; }
    }
}
