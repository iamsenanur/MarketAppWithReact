import React, { useEffect, useState } from 'react'
import axios from 'axios'

const MusteriYonetimi = ({ searchTerm }) => {
  const [kullanicilar, setKullanicilar] = useState([])

  useEffect(() => {
    axios.get('https://localhost:7264/api/admin/kullanicilarilistele')
      .then(res => setKullanicilar(res.data))
      .catch(err => console.error("Veri çekme hatası:", err))
  }, [])

  const filteredKullanicilar = kullanicilar.filter(u =>
    u.userName?.toLowerCase().includes(searchTerm.toLowerCase())
  )

  return (
    <div style={{ textAlign: 'center', marginTop: '1rem' }}>
      <h2 style={{ color: "#2a2f5b", textAlign: "left", marginLeft: "50px" }}>Müşteriler</h2>

      <div style={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'center' }}>
        {filteredKullanicilar.map((kullanici) => (
          <div
            key={kullanici.id}
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
            <h3>{kullanici.userName}</h3>
            <p>Email: {kullanici.email}</p>
            <p>ID: {kullanici.id}</p>
          </div>
        ))}
      </div>
    </div>
  )
}

export default MusteriYonetimi
