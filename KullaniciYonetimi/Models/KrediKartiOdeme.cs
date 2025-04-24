namespace KullaniciYonetimi.Models
{
    public class KrediKartiOdeme: IOdemeStratejisi
    {
        public void OdemeYap(double tutar)
        {
            var sonuc = new Random().Next(0, 2) == 0 ? "Başarılı" : "Başarısız";
            Console.WriteLine($"{tutar} TL Kredi Kartı ile ödeme işlemi: {sonuc}");
        }
    }
}
