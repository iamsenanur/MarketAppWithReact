# 🛒 MarketAppWithReact

React + ASP.NET Core kullanarak Nesneye Yönelik Programlama prensipleriyle geliştirmekte olduğum fullstack bir e-ticaret uygulamasıdır.

## 🚀 Özellikler

- JWT ile kullanıcı girişi ve rol yönetimi (Admin & Müşteri)
- Ürün listeleme, arama ve admin tarafından ürün yönetimi
- Sepet işlemleri ve sipariş yönetimi
- Simülasyon tabanlı ödeme sistemi
- Admin paneli üzerinden kategori, sipariş ve kullanıcı kontrolü

## ⚙️ Teknolojiler

- React (frontend)
- ASP.NET Core + EF Core + MySQL (backend)
- Code First yaklaşımı

## 🛠️ Kurulum

```bash
# Backend
cd KullaniciYonetimi
dotnet restore
dotnet ef database update
dotnet run

# Frontend
cd frontend
npm install
npm start
