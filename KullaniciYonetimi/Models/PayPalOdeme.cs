namespace KullaniciYonetimi.Models
{
    public class PayPalOdeme: IOdemeStratejisi
    {
        public void OdemeYap(double tutar)
        {
            Console.WriteLine($"{tutar} TL PayPal ile ödendi");
        }
    }
}
