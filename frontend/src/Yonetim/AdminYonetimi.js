import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { FaPlus, FaEdit, FaRegTimesCircle, FaTrash } from "react-icons/fa";
import AdminKart from '../Components/KullaniciKarti';

const AdminYonetimi = () => {
  const [kullanicilar, setKullanicilar] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [yetkisiz, setYetkisiz] = useState(false);
  const [selectedKullanici, setSelectedKullanici] = useState(null);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      setYetkisiz(true);
      return;
    }

    let role = null;
    try {
      const decoded = jwtDecode(token);
      role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    } catch (err) {
      setYetkisiz(true);
      return;
    }

    if (role !== "Admin") {
      setYetkisiz(true);
      return;
    }

    fetchKullanicilar();
  }, []);

  const fetchKullanicilar = () => {
    const token = localStorage.getItem("token");
    axios.get('https://localhost:7264/api/admin/kullanicilarilistele', {
      headers: {
        Authorization: `Bearer ${token}`
      }
    })
      .then(res => setKullanicilar(res.data))
      .catch(err => {
        console.error("Veri çekme hatası:", err);
        setYetkisiz(true);
      });
  };

  const filteredKullanicilar = kullanicilar
    .filter(u => u.role === "Admin")
    .filter(u => u.userName?.toLowerCase().includes(searchTerm.toLowerCase()));

  const handleDeleteClick = async (id) => {
    if (window.confirm("Bu kullanıcıyı silmek istediğinize emin misiniz?")) {
      try {
        const token = localStorage.getItem("token");
        await axios.delete(`https://localhost:7264/api/admin/KullaniciSil/${id}`, {
          headers: {
            Authorization: `Bearer ${token}`
          }
        });
        alert("Kullanıcı başarıyla silindi!");
        // Silinen kullanıcıyı ekrandan da kaldır
        setKullanicilar(prev => prev.filter(k => k.id !== id));
      } catch (error) {
        console.error("Silme hatası:", error);
        alert("Kullanıcı silinemedi.");
      }
    }
  };

  const handleEditClick = (kullanici) => {
    setSelectedKullanici(kullanici);
  };

  const handleSaveChanges = async () => {
    try {
      const token = localStorage.getItem("token");
      await axios.put(`https://localhost:7264/api/admin/KullaniciGuncelle/${selectedKullanici.id}`, {
        userName: selectedKullanici.userName,
        email: selectedKullanici.email,
        phoneNumber: selectedKullanici.phoneNumber
      }, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });

      alert("Kullanıcı başarıyla güncellendi!");

      // Güncellenen kullanıcıyı listede değiştir
      setKullanicilar(prev =>
        prev.map(k => (k.id === selectedKullanici.id ? selectedKullanici : k))
      );

      setSelectedKullanici(null); // Modalı kapat
    } catch (error) {
      console.error("Güncelleme hatası:", error);
      alert("Kullanıcı güncellenemedi.");
    }
  };

  if (yetkisiz) {
    return <h3 style={{ textAlign: "center", color: "red" }}>Bu sayfaya erişim yetkiniz yok.</h3>;
  }

  return (
    <div style={{ textAlign: 'center', marginTop: '1rem' }}>
      {/* Başlık + Arama Kutusu */}
      <div style={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        margin: '0 50px',
        marginBottom: '1rem'
      }}>
        <h2 style={{ color: "#2a2f5b" }}>Adminler</h2>

        <input
          type="text"
          placeholder="Admin ara..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          style={{
            padding: "0.5rem 1rem",
            borderRadius: "10px",
            border: "1px solid #ccc",
            outline: "none",
            fontSize: "1rem",
            width: "220px"
          }}
        />
      </div>

      {/* Admin Kartları */}
      <div style={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'center' }}>
        {filteredKullanicilar.map((kullanici) => (
       <AdminKart
       key={kullanici.id}
       kullanici={kullanici}
       onEdit={handleEditClick}
       onDelete={handleDeleteClick}
       kartTipi="admin"
       />
        ))}
      </div>

      {/* Düzenleme Formu */}
      {selectedKullanici && (
        <div style={{
          position: "fixed",
          top: "20%",
          left: "50%",
          transform: "translate(-50%, -20%)",
          backgroundColor: "white",
          padding: "2rem",
          boxShadow: "0 2px 8px rgba(0,0,0,0.2)",
          zIndex: 1000,
          borderRadius: "10px",
          width: "300px"
        }}>
          <h3>Kullanıcıyı Düzenle</h3>

          <input
            type="text"
            value={selectedKullanici.userName}
            onChange={(e) => setSelectedKullanici({ ...selectedKullanici, userName: e.target.value })}
            placeholder="Kullanıcı Adı"
            style={{ width: "100%", marginBottom: "0.5rem", padding: "0.5rem" }}
          />
          <input
            type="email"
            value={selectedKullanici.email}
            onChange={(e) => setSelectedKullanici({ ...selectedKullanici, email: e.target.value })}
            placeholder="Email"
            style={{ width: "100%", marginBottom: "0.5rem", padding: "0.5rem" }}
          />
          <input
            type="text"
            value={selectedKullanici.phoneNumber}
            onChange={(e) => setSelectedKullanici({ ...selectedKullanici, phoneNumber: e.target.value })}
            placeholder="Telefon"
            style={{ width: "100%", marginBottom: "1rem", padding: "0.5rem" }}
          />

          <button
            onClick={handleSaveChanges}
            style={{
              backgroundColor: "#388E3C",
              color: "white",
              border: "none",
              padding: "0.5rem 1rem",
              borderRadius: "5px",
              cursor: "pointer",
              width: "100%"
            }}
          >
            Kaydet
          </button>

          <button
            onClick={() => setSelectedKullanici(null)}
            style={{
              marginTop: "0.5rem",
              backgroundColor: "#ccc",
              border: "none",
              padding: "0.5rem 1rem",
              borderRadius: "5px",
              cursor: "pointer",
              width: "100%"
            }}
          >
            İptal
          </button>
        </div>
      )}
    </div>
  );
};

export default AdminYonetimi;
