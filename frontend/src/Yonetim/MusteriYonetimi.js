import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { jwtDecode } from 'jwt-decode';
import { FaPlus, FaEdit, FaRegTimesCircle, FaTrash  } from "react-icons/fa";



const MusteriYonetimi = () => {
  const [kullanicilar, setKullanicilar] = useState([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [yetkisiz, setYetkisiz] = useState(false);
  const [selectedKullanici, setSelectedKullanici] = useState(null);
  const [yeniMusteriModal, setYeniMusteriModal] = useState(false);
  const [yeniMusteri, setYeniMusteri] = useState({ userName: '', email: '', password: '' });

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
      headers: { Authorization: `Bearer ${token}` }
    })
    .then(res => setKullanicilar(res.data))
    .catch(err => {
      console.error("Veri çekme hatası:", err);
      setYetkisiz(true);
    });
  };

  const filteredKullanicilar = kullanicilar
    .filter(u => u.role === "Müşteri")
    .filter(u => u.userName?.toLowerCase().includes(searchTerm.toLowerCase()));

  const handleDeleteClick = async (id) => {
    if (window.confirm("Bu kullanıcıyı silmek istediğinize emin misiniz?")) {
      try {
        const token = localStorage.getItem("token");
        await axios.delete(`https://localhost:7264/api/admin/KullaniciSil/${id}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        alert("Kullanıcı başarıyla silindi!");
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
        phoneNumber: selectedKullanici.phoneNumber?.trim() === "" ? null : selectedKullanici.phoneNumber
      }, {
        headers: { Authorization: `Bearer ${token}` }
      });

      alert("Kullanıcı başarıyla güncellendi!");
      setKullanicilar(prev => prev.map(k => (k.id === selectedKullanici.id ? selectedKullanici : k)));
      setSelectedKullanici(null);
    } catch (error) {
      console.error("Güncelleme hatası:", error);
      alert("Kullanıcı güncellenemedi.");
    }
  };

  const handleYeniMusteriEkle = async () => {
    try {
      await axios.post('https://localhost:7264/api/auth/register', {
        username: yeniMusteri.userName,
        email: yeniMusteri.email,
        password: yeniMusteri.password
      });

      alert("Müşteri başarıyla eklendi!");
      fetchKullanicilar();
      setYeniMusteriModal(false);
      setYeniMusteri({ userName: '', email: '', password: '' });
    } catch (error) {
      console.error("Müşteri ekleme hatası:", error);
      alert("Müşteri eklenemedi.");
    }
  };

  if (yetkisiz) {
    return <h3 style={{ textAlign: "center", color: "red" }}>Bu sayfaya erişim yetkiniz yok.</h3>;
  }

  return (
    <div style={{ textAlign: 'center', marginTop: '1rem' }}>
      {/* Başlık + Buton + Arama */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', margin: '0 50px', marginBottom: '1rem' }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <h2 style={{ color: "#2a2f5b", margin: 0 }}>Müşteriler</h2>
          <button 
            onClick={() => setYeniMusteriModal(true)} 
            title="Müşteri Ekle"
            style={{
              padding: "0.3rem 0.6rem",
              backgroundColor: "#4CAF50",
              color: "white",
              border: "none",
              borderRadius: "5px",
              cursor: "pointer",
              display: "flex",
              alignItems: "center",
              justifyContent: "center"
            }}
          >
            <FaPlus  size={18} />
          </button>
        </div>

        <input
          type="text"
          placeholder="Müşteri ara..."
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

      {/* Müşteri Kartları */}
      <div style={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'center' }}>
        {filteredKullanicilar.map((kullanici) => (
          <div key={kullanici.id} style={{
            borderRadius: "10px",
            margin: "1rem",
            padding: "1rem",
            width: "220px",
            backgroundColor: "white",
            color: "#333",
            boxShadow: "0 2px 6px rgba(0,0,0,0.1)"
          }}>
            <h3>{kullanici.userName}</h3>
            <p>Email: {kullanici.email}</p>
            <p style={{ fontSize: "0.85rem", color: "#777" }}>Rol: {kullanici.role}</p>

            <div style={{ marginTop: "1rem", display: "flex", justifyContent: "space-between" }}>
              <button
                onClick={() => handleEditClick(kullanici)}
                style={{
                  padding: "0.6rem 1rem",
                  borderRadius: "5px",
                  border: "none",
                  backgroundColor: "#4CAF50",
                  color: "white",
                  cursor: "pointer"
                }}
              >
                Düzenle <FaEdit />
              </button>
              <button
                onClick={() => handleDeleteClick(kullanici.id)}
                style={{
                  padding: "0.6rem 1rem",
                  borderRadius: "5px",
                  border: "none",
                  backgroundColor: "#f44336",
                  color: "white",
                  cursor: "pointer"
                }}
              >
                Sil <FaTrash />
              </button>
            </div>
          </div>
        ))}
      </div>

      {/* Yeni Müşteri Ekle Modalı */}
      {yeniMusteriModal && (
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
          <h3>Yeni Müşteri Ekle</h3>

          <input type="text" placeholder="Kullanıcı Adı" value={yeniMusteri.userName} onChange={(e) => setYeniMusteri({ ...yeniMusteri, userName: e.target.value })} style={{ width: "100%", marginBottom: "0.5rem", padding: "0.5rem" }} />
          <input type="email" placeholder="Email" value={yeniMusteri.email} onChange={(e) => setYeniMusteri({ ...yeniMusteri, email: e.target.value })} style={{ width: "100%", marginBottom: "0.5rem", padding: "0.5rem" }} />
          <input type="password" placeholder="Şifre" value={yeniMusteri.password} onChange={(e) => setYeniMusteri({ ...yeniMusteri, password: e.target.value })} style={{ width: "100%", marginBottom: "1rem", padding: "0.5rem" }} />

          <button onClick={handleYeniMusteriEkle} style={{ backgroundColor: "#388E3C", color: "white", border: "none", padding: "0.5rem 1rem", borderRadius: "5px", cursor: "pointer", width: "100%" }}>
            Kaydet
          </button>

          <button onClick={() => setYeniMusteriModal(false)} style={{ marginTop: "0.5rem", backgroundColor: "#ccc", border: "none", padding: "0.5rem 1rem", borderRadius: "5px", cursor: "pointer", width: "100%" }}>
            İptal
          </button>
        </div>
      )}

      {/* Kullanıcı Düzenleme Modalı */}
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

          <input type="text" value={selectedKullanici.userName ?? ""} onChange={(e) => setSelectedKullanici({ ...selectedKullanici, userName: e.target.value })} placeholder="Kullanıcı Adı" style={{ width: "100%", marginBottom: "0.5rem", padding: "0.5rem" }} />
          <input type="email" value={selectedKullanici.email ?? ""} onChange={(e) => setSelectedKullanici({ ...selectedKullanici, email: e.target.value })} placeholder="Email" style={{ width: "100%", marginBottom: "0.5rem", padding: "0.5rem" }} />
          <input type="text" value={selectedKullanici.phoneNumber ?? ""} onChange={(e) => setSelectedKullanici({ ...selectedKullanici, phoneNumber: e.target.value })} placeholder="Telefon numarası" style={{ width: "100%", marginBottom: "1rem", padding: "0.5rem" }} />

          <button onClick={handleSaveChanges} style={{ backgroundColor: "#388E3C", color: "white", border: "none", padding: "0.5rem 1rem", borderRadius: "5px", cursor: "pointer", width: "100%" }}>
            Kaydet
          </button>

          <button onClick={() => setSelectedKullanici(null)} style={{ marginTop: "0.5rem", backgroundColor: "#ccc", border: "none", padding: "0.5rem 1rem", borderRadius: "5px", cursor: "pointer", width: "100%" }}>
            İptal
          </button>
        </div>
      )}
    </div>
  );
};

export default MusteriYonetimi;
