using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("KategoriListesi")]
    public class KategoriListesi
    {
        [Key]
        public int KategoriID { get; set; }

        [Required, StringLength(50, ErrorMessage = "Kategori adı en fazla 50 olmalıdır")]
        public string KategoriAdi { get; set; }

        public bool isActive { get; set; }

        //Bir kategorinin birden çok ürünü olabilir:
        public ICollection<UrunListesi> Urunler { get; set; }
    }
}
