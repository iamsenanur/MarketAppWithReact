using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("IslemlerListesi")]
    public class IslemlerListesi
    {
        [Key]
        public int IslemlerID { get; set; }

        [ForeignKey("Urun")]
        public int UrunID { get; set; }
        public UrunListesi Urun { get; set; }


        [ForeignKey("User")]
        public string UserID { get; set; }
       public IdentityUser User { get; set; }

        public DateTime IslemTarihi { get;set; }

        [ForeignKey("IslemTipi")]
        public int IslemTipiID { get; set; }
        public IslemTipiListesi IslemTipi { get; set; }

        public int Adet { get; set; }
    }
}
