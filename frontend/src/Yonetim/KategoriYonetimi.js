import React, { useEffect, useState } from 'react';
import axios from 'axios';

const KategoriYonetimi = () => {
  const [aktifKategoriler, setAktifKategoriler] = useState([]);
  const [pasifKategoriler, setPasifKategoriler] = useState([]);

  useEffect(() => {
    axios.get('https://localhost:7264/api/admin/aktifkategoriler')
      .then(res => setAktifKategoriler(res.data))
      .catch(err => console.error("Aktif kategori çekilemedi:", err));

    axios.get('https://localhost:7264/api/admin/pasifkategoriler')
      .then(res => setPasifKategoriler(res.data))
      .catch(err => console.error("Pasif kategori çekilemedi:", err));
  }, []);

  return (
    <div style={sayfaStil}>
      <h2 style={baslikStil}>Kategori Yönetimi</h2>

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
    </div>
  );
};

// 🎨 STİL NESNELERİ
const sayfaStil = {
  padding: '2rem',
  backgroundColor: '#f4f6f8',
  fontFamily: 'Segoe UI, sans-serif',
};

const baslikStil = {
  fontSize: '2rem',
  textAlign: 'center',
  color: '#2a2f5b',
  marginBottom: '2rem',
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
