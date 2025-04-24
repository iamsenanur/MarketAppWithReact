using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("SepetListesi")]
    public class SepetListesi
    {
        [Key]
       public int SepetID { get; set; }

       public int SepetTutari { get; set; }

        public DateTime SepetTarihi { get; set; }

        public ICollection<SepetIslemleri> sepetIslemleri { get; set; }

    }
}
