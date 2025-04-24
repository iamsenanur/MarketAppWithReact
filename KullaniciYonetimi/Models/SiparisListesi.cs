using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("SiparisListesi")]
    public class SiparisListesi
    {
        [Key]
        public int SiparisID { get; set; }

        [ForeignKey("Sepet")]
        public int SepetID { get; set; }

        public SepetListesi Sepet { get; set; }


        [ForeignKey("User")]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }


        public decimal SiparisFiyatiTutar { get; set; }
        public DateTime SiparisTarihi { get; set; }
    }
}
