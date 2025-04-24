using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("IslemTipiListesi")]
    public class IslemTipiListesi
    {
        [Key]
        public int IslemID { get; set; }

        [Required, StringLength(50, ErrorMessage = "İşlemtipi adı en fazla 50 olmalıdır")]
        public string IslemTipi { get; set; }

        public ICollection<IslemlerListesi> Islemler { get; set; }
    }
}
