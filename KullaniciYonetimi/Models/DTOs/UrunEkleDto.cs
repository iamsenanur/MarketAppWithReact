namespace KullaniciYonetimi.Models.DTOs
{
    public class UrunEkleDto
    {
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public string UrunBarkod { get; set; }

        public int UrunKategoriID { get; set; }
        public bool isActive { get; set; }


    }
}
