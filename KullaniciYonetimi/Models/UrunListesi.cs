using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KullaniciYonetimi.Models
{
    [Table("UrunListesi")]
    public class UrunListesi
    {
        [Key]
        public int UrunID { get; set; }

        
        [Required, StringLength(50, ErrorMessage = "Ürün adı en fazla 50 olmalıdır")]
        public string UrunAdi { get; set; }

        [Required, StringLength(20, ErrorMessage = "Barkod en fazla 20 karakter olabilir.")]
        public string UrunBarkod { get; set; }


        //[ForeignKey("Kategori")]
        public int UrunKategoriID { get; set; }

   
        public KategoriListesi? Kategori { get; set; }

        public bool isActive { get; set; }
        //public int UrunFiyati { get; set; }

    }
}
