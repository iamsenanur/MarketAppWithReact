import React, { useEffect, useState } from 'react'
import axios from 'axios'

const UrunListesi = () => {
  const [urunler, setUrunler] = useState([])
  const [searchTerm, setSearchTerm] = useState('')

  useEffect(() => {
    axios.get('https://localhost:7264/api/product/getall')
      .then(res => setUrunler(res.data))
      .catch(err => console.error('Veri çekme hatası:', err))
  }, [])

  const filteredUrunler = urunler.filter(u =>
    u.urunAdi.toLowerCase().includes(searchTerm.toLowerCase())
  )

  return (
    <div style={{ textAlign: 'center', marginTop: '1rem' }}>
      {/* Başlık ve Arama inputunu aynı satıra aldık */}
      <div style={{
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        margin: '0 50px',
        marginBottom: '1rem'
      }}>
        <h2 style={{ color: "#2a2f5b" }}>Ürünler</h2>

        <input
          type="text"
          placeholder="Ara..."
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

      {/* Ürün kartları */}
      <div style={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'center' }}>
        {filteredUrunler.map((urun) => (
          <div
            key={urun.urunID}
            style={{
              borderRadius: "10px",
              margin: "1rem",
              padding: "1rem",
              width: "220px",
              backgroundColor: "white",
              color: "#333",
              boxShadow: "0 2px 6px rgba(0,0,0,0.1)"
            }}
          >
            <h3>{urun.urunAdi}</h3>
            <p>ID: {urun.urunID}</p>
            <p>Kategori ID: {urun.urunKategoriID}</p>
          </div>
        ))}
      </div>
    </div>
  )
}

export default UrunListesi
