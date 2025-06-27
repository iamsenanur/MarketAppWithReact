# 🛒 MarketAppWithReact

React + ASP.NET Core kullanarak Nesneye Yönelik Programlama prensipleriyle geliştirmekte olduğum fullstack bir e-ticaret uygulamasıdır.

## 🚀 Özellikler

- JWT ile kullanıcı girişi ve rol yönetimi (Admin & Müşteri)
- Ürün listeleme, arama ve admin tarafından ürün yönetimi
- Sepet işlemleri ve sipariş yönetimi
- Simülasyon tabanlı ödeme sistemi
- Admin paneli üzerinden kategori, sipariş ve kullanıcı kontrolü
  
## 🖼️ Ekran Görüntüleri

### 🔐 Giriş Sayfası  
![Login Sayfası](./loginSayfasi.png)

### 🛒 Müşteri Paneli ve Sepet Yönetimi  
![Müşteri Paneli](./musteriPanelSepetYonetimi.png)

### 📦 Ürün Yönetimi  
![Ürün Yönetimi](./urunYoetimi.png)

### 📋 Ürün Listesi  
![Ürün Listesi](./urunListesi.png)

### 🛠️ Admin Yönetimi  
![Admin Yönetimi](./adminYonetimi.png)

### 👤 Müşteri Yönetimi  
![Müşteri Yönetimi](./musteriYonetimi.png)

### 🗂️ Kategori Yönetimi  
![Kategori Yönetimi](./kategoriYonetimii.png)


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
