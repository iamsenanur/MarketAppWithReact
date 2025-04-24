import React from "react";
import { useNavigate } from "react-router-dom";

function Giris() {
  const navigate = useNavigate();

  const bugun = new Date().toLocaleDateString("tr-TR", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric"
  });

  return (
    <div style={{ padding: "40px" }}>
      {/* Üstteki Hoş Geldin Kutusu */}
      <div
        style={{
          background: "linear-gradient(135deg, #98d6a5, #66bb6a)",
          borderRadius: "25px",
          padding: "30px",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          boxShadow: "0 6px 18px rgba(0, 0, 0, 0.08)"
        }}
      >
        <div>
          <h2 style={{ margin: 0, color: "#2a2f5b" }}>Market Uygulamasına Hoş Geldiniz!</h2>
        </div>

        <div
          style={{
            backgroundColor: "#ffffff",
            padding: "10px 20px",
            borderRadius: "10px",
            textAlign: "center",
            color: "#2a2f5b",
            fontWeight: "bold",
            boxShadow: "0 2px 6px rgba(0,0,0,0.1)"
          }}
        >
          {bugun}
        </div>
      </div>

      {/* Orta Kısım: Buton ve Görsel */}
      <div
        style={{
          marginTop: "40px",
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
          flexWrap: "wrap"
        }}
      >
        {/* Sol: Büyük Buton */}
        <div style={{ flex: 1 }}>
          <h3 style={{ fontSize: "22px", color: "#2a2f5b", marginBottom: "20px" }}>
             Hazırsanız alışverişe başlayalım!
          </h3>
          <button
            onClick={() => navigate("/login")}
            style={{
              padding: "20px 40px",
              backgroundColor: "#388e3c",
              color: "#fff",
              border: "none",
              borderRadius: "15px",
              fontSize: "20px",
              cursor: "pointer",
              boxShadow: "0 4px 12px rgba(0,0,0,0.15)"
            }}
          >
            Alışverişe Başla
          </button>
           {/* Alt Kısım: Ekstra İçerik */}
      <div style={{
        marginTop: "50px",
        display: "flex",
        gap: "20px",
        flexWrap: "wrap",
        justifyContent: "center"
      }}>
        {/* Bilgi Kutuları */}
        {[
          { icon: "🧺", label: "150+ ürün çeşidi" },
          { icon: "🚚", label: "Hızlı sipariş yönetimi" },
          { icon: "🎯", label: "Kolay sepet takibi" }
        ].map((item, i) => (
          <div key={i} style={{
            backgroundColor: "#f1f8e9",
            padding: "20px 30px",
            borderRadius: "15px",
            minWidth: "220px",
            textAlign: "center",
            color: "#2a2f5b",
            boxShadow: "0 2px 8px rgba(0,0,0,0.05)"
          }}>
            <div style={{ fontSize: "30px" }}>{item.icon}</div>
            <div style={{ marginTop: "10px", fontSize: "16px" }}>{item.label}</div>
          </div>
        ))}
      </div>
        </div>

        {/* Sağ: Market Rafı Görseli */}
        <img
          src="/marketRafi.png"
          alt="Market Rafı"
          style={{
            maxWidth: "400px",
            width: "100%",
            borderRadius: "20px",
            boxShadow: "0 4px 10px rgba(0,0,0,0.1)",
            marginTop: "20px"
          }}
        />
      </div>

     
    </div>
  );
}

export default Giris;
