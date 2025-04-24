namespace KullaniciYonetimi.Models
{
    public class OdemeContext
    {
        private IOdemeStratejisi _odemeStratejisi;

        public void SetOdemeStratejisi(IOdemeStratejisi odemeStratejisi)
        {
            _odemeStratejisi = odemeStratejisi;
        }

        public void OdemeYap(double tutar)
        {
            _odemeStratejisi?.OdemeYap(tutar);
        }
    }
}
