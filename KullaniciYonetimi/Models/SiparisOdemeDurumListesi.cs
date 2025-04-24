using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KullaniciYonetimi.Models
{
    [Table("SiparisOdemeDurumListesi")]
    public class SiparisOdemeDurumListesi
    {
        [Key]
        public int SiparisOdemeDurumID { get; set; }

        [ForeignKey("Siparis")]
 
        public int SiparisID { get; set; }
        public SiparisListesi Siparis { get; set; }

        [ForeignKey("User")]
        public string UserID { get; set; }
        public IdentityUser User { get; set; }

        
        [Required, StringLength(20, ErrorMessage = "Ödeme Durumu en fazla 50 karakter olabilir.")]

        public string SiparisOdemeDurumu { get; set; }

        public DateTime SiparisOdemeDurumTarihi { get; set; }



    }
}
