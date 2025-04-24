using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("SiparisTeslimatDurumListesi")]
    public class SiparisTeslimatDurumListesi
    {
        [Key]
        public int SiparisTeslimatDurumID { get; set; }

        [ForeignKey("Siparis")]

        public int SiparisID { get; set; }
        public SiparisListesi Siparis { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }

        [Required, StringLength(20, ErrorMessage = "Teslimat Durumu en fazla 50 karakter olabilir.")]

        public string SiparisTeslimatDurumu { get; set; }

        public DateTime SiparisTeslimatDurumTarihi { get; set; }

    }
}
