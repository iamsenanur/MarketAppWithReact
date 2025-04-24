using System.ComponentModel.DataAnnotations;

namespace KullaniciYonetimi.Models.DTOs
{
    public class KategoriEkleDto
    {
        [Required(ErrorMessage ="Kategori adı boş olamaz.")]
        [StringLength(50,ErrorMessage ="Kategori adı en fazla 50 karakter olmalıdır!")]
        public string KategoriAdi { get; set; }
    }
}
