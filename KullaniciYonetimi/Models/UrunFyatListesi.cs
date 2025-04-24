using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace KullaniciYonetimi.Models
{
    [Table("UrunFiyatListesi")]
    public class UrunFyatListesi
    {
        [Key]
        public int UrunFiyatID { get; set; }


        [ForeignKey("Urun")]
        public int UrunBarkod { get; set; }
        public UrunListesi Urun { get; set; }
        public decimal UrunFiyati { get; set; }
        public DateTime UrunFiyatTarihi { get; set; }
        public bool isActive { get; set; }
    }
}
