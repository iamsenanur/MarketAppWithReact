import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { FaPlus, FaTrash } from "react-icons/fa";

const KategoriYonetimi = () => {
  const [aktifKategoriler, setAktifKategoriler] = useState([]);
  const [pasifKategoriler, setPasifKategoriler] = useState([]);
  const [yeniKategoriModal, setYeniKategoriModal] = useState(false);
  const [yeniKategoriAdi, setYeniKategoriAdi] = useState('');

  useEffect(() => {
    fetchKategoriler();
  }, []);

  const fetchKategoriler = () => {
    axios.get('https://localhost:7264/api/admin/aktifkategoriler')
      .then(res => setAktifKategoriler(res.data))
      .catch(err => console.error("Aktif kategori çekilemedi:", err));

    axios.get('https://localhost:7264/api/admin/pasifkategoriler')
      .then(res => setPasifKategoriler(res.data))
      .catch(err => console.error("Pasif kategori çekilemedi:", err));
  };

  const handleKategoriSil = async (id) => {
    if (window.confirm("Bu kategoriyi silmek istediğinize emin misiniz?")) {
      try {
        const token = localStorage.getItem("token");
        await axios.delete(`https://localhost:7264/api/admin/KategoriDelete?id=${id}`, {
          headers: { Authorization: `Bearer ${token}` }
        });
        alert("Kategori başarıyla silindi!");
        fetchKategoriler();
      } catch (error) {
        console.error("Kategori silme hatası:", error);
        alert("Kategori silinemedi.");
      }
    }
  };

  const handleKategoriEkle = async () => {
    try {
      const token = localStorage.getItem("token");
      await axios.post('https://localhost:7264/api/admin/KategoriAdd', {
        kategoriAdi: yeniKategoriAdi
      }, {
        headers: { Authorization: `Bearer ${token}` }
      });

      alert("Kategori başarıyla eklendi!");
      fetchKategoriler();
      setYeniKategoriModal(false);
      setYeniKategoriAdi('');
    } catch (error) {
      console.error("Kategori ekleme hatası:", error);
      alert("Kategori eklenemedi.");
    }
  };

  return (
    <div style={sayfaStil}>
      {/* Başlık ve Ekle Butonu */}
      <div style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: '1rem', marginBottom: '2rem' }}>
        <h2 style={baslikStil}>Kategori Yönetimi</h2>
        <button
          onClick={() => setYeniKategoriModal(true)}
          title="Kategori Ekle"
          style={{
            padding: "0.3rem 0.6rem",
            backgroundColor: "#4CAF50",
            color: "white",
            border: "none",
            borderRadius: "5px",
            cursor: "pointer",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            height: "36px",
            width: "36px"
          }}
        >
          <FaPlus size={18} />
        </button>
      </div>

      <div style={kategoriAlanWrapper}>
        {/* Aktif Kategoriler */}
        <div style={kategoriKolon}>
          <h3 style={{ ...altBaslikStil, color: "#28a745" }}>✅ Aktif Kategoriler</h3>
          <div style={kartlarAlani}>
            {aktifKategoriler.length === 0 ? (
              <p style={bilgiYazi}>Aktif kategori bulunamadı.</p>
            ) : (
              aktifKategoriler.map(k => (
                <div key={k.kategoriID} style={kutuStil("#d4edda")}>
                  <h4 style={kutuBaslik}>{k.kategoriAdi}</h4>
                  <p style={kutuParagraf}>ID: {k.kategoriID}</p>
                  <button
                    onClick={() => handleKategoriSil(k.kategoriID)}
                    style={{
                      marginTop: "0.5rem",
                      backgroundColor: "#f44336",
                      color: "white",
                      border: "none",
                      padding: "0.4rem 0.8rem",
                      borderRadius: "5px",
                      cursor: "pointer",
                      display: "flex",
                      alignItems: "center",
                      gap: "0.3rem"
                    }}
                  >
                    <FaTrash /> Sil
                  </button>
                </div>
              ))
            )}
          </div>
        </div>

        {/* Pasif Kategoriler */}
        <div style={kategoriKolon}>
          <h3 style={{ ...altBaslikStil, color: "#dc3545" }}>🚫 Pasif Kategoriler</h3>
          <div style={kartlarAlani}>
            {pasifKategoriler.length === 0 ? (
              <p style={bilgiYazi}>Pasif kategori bulunamadı.</p>
            ) : (
              pasifKategoriler.map(k => (
                <div key={k.kategoriID} style={kutuStil("#f8d7da")}>
                  <h4 style={kutuBaslik}>{k.kategoriAdi}</h4>
                  <p style={kutuParagraf}>ID: {k.kategoriID}</p>
                </div>
              ))
            )}
          </div>
        </div>
      </div>

      {/* Yeni Kategori Ekle Modalı */}
      {yeniKategoriModal && (
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
          <h3>Yeni Kategori Ekle</h3>

          <input
            type="text"
            placeholder="Kategori Adı"
            value={yeniKategoriAdi}
            onChange={(e) => setYeniKategoriAdi(e.target.value)}
            style={{ width: "100%", marginBottom: "1rem", padding: "0.5rem" }}
          />

          <button onClick={handleKategoriEkle} style={{ backgroundColor: "#388E3C", color: "white", border: "none", padding: "0.5rem 1rem", borderRadius: "5px", cursor: "pointer", width: "100%" }}>
            Kaydet
          </button>

          <button onClick={() => setYeniKategoriModal(false)} style={{ marginTop: "0.5rem", backgroundColor: "#ccc", border: "none", padding: "0.5rem 1rem", borderRadius: "5px", cursor: "pointer", width: "100%" }}>
            İptal
          </button>
        </div>
      )}
    </div>
  );
};

// 🎨 STİLLER
const sayfaStil = {
  padding: '2rem',
  backgroundColor: '#f4f6f8',
  fontFamily: 'Segoe UI, sans-serif',
};

const baslikStil = {
  fontSize: '2rem',
  textAlign: 'center',
  color: '#2a2f5b',
  margin: 0,
};

const kategoriAlanWrapper = {
  display: 'flex',
  justifyContent: 'space-between',
  gap: '2rem',
  flexWrap: 'wrap',
};

const kategoriKolon = {
  flex: '1',
  minWidth: '300px',
};

const altBaslikStil = {
  fontSize: '1.3rem',
  marginBottom: '1rem',
  borderBottom: '2px solid #ccc',
  paddingBottom: '0.5rem',
};

const kartlarAlani = {
  display: 'flex',
  flexDirection: 'column',
  gap: '1rem',
};

const bilgiYazi = {
  color: '#6c757d',
  fontStyle: 'italic',
};

const kutuStil = (bgColor) => ({
  backgroundColor: bgColor,
  padding: '1rem',
  borderRadius: '10px',
  boxShadow: '0 2px 6px rgba(0,0,0,0.1)',
  transition: 'transform 0.3s ease',
  border: '1px solid #ccc',
});

const kutuBaslik = {
  fontSize: '1.1rem',
  color: '#343a40',
  marginBottom: '0.5rem',
};

const kutuParagraf = {
  margin: 0,
  color: '#555',
};

export default KategoriYonetimi;
