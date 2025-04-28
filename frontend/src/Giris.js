import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  FaShoppingBasket,
  FaTruck,
  FaShoppingCart,
  FaUserShield,
  FaBoxes,
  FaClipboardCheck,
  FaCreditCard
} from "react-icons/fa";
import { FiTarget } from "react-icons/fi";

function Giris() {
  const navigate = useNavigate();
  const [hovered, setHovered] = useState(false);

  const bugun = new Date().toLocaleDateString("tr-TR", {
    weekday: "long",
    year: "numeric",
    month: "long",
    day: "numeric"
  });

  const features = [
    { icon: <FaUserShield size={32} />, label: "JWT ile Güvenli Giriş" },
    { icon: <FaBoxes size={32} />, label: "Ürün Yönetimi ve Stok Takibi" },
    { icon: <FaShoppingCart size={32} />, label: "Akıllı Sepet Sistemi" },
    { icon: <FaClipboardCheck size={32} />, label: "Sipariş ve Teslimat Takibi" },
    { icon: <FaCreditCard size={32} />, label: "Çok Yönlü Ödeme Altyapısı" }
  ];

  return (
    <div style={{ padding: "40px" }}>
      {/* Üst Kutu */}
      <div
        style={{
          background: "linear-gradient(135deg, #98d6a5, #66bb6a)",
          borderRadius: "25px",
          padding: "30px",
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          flexWrap: "wrap",
          boxShadow: "0 6px 18px rgba(0, 0, 0, 0.08)"
        }}
      >
        <h2 style={{ margin: 0, color: "#495057" }}>
          Market Uygulamasına Hoş Geldiniz!
        </h2>

        <div
          style={{
            backgroundColor: "#ffffff",
            padding: "10px 20px",
            borderRadius: "10px",
            marginTop: "10px",
            color: "#2a2f5b",
            fontWeight: "bold",
            boxShadow: "0 2px 6px rgba(0,0,0,0.1)"
          }}
        >
          {bugun}
        </div>
      </div>

      {/* Orta Kısım */}
      <div
        style={{
          marginTop: "40px",
          display: "flex",
          justifyContent: "space-between",
          flexWrap: "wrap",
          gap: "40px"
        }}
      >
        {/* Sol */}
        <div style={{ flex: "1 1 350px" }}>
          <h3
            style={{
              fontSize: "22px",
              color: "#4f5d75",
              marginBottom: "20px",
              textAlign: "center"
            }}
          >
            Hazırsanız alışverişe başlayalım!
          </h3>

          {/* Buton */}
          <div style={{ textAlign: "center", marginBottom: "40px" }}>
            <button
              onClick={() => navigate("/login")}
              onMouseEnter={() => setHovered(true)}
              onMouseLeave={() => setHovered(false)}
              style={{
                padding: "20px 60px",
                backgroundColor: "#388e3c",
                color: "#fff",
                border: "none",
                borderRadius: "15px",
                fontSize: "22px",
                fontWeight: "bold",
                cursor: "pointer",
                display: "inline-flex",
                alignItems: "center",
                gap: "12px",
                boxShadow: "0 4px 12px rgba(0,0,0,0.15)",
                transform: hovered ? "scale(1.05)" : "scale(1)",
                transition: "all 0.3s ease"
              }}
            >
              <FaShoppingCart size={24} />
              Alışverişe Başla
            </button>
          </div>

          {/* Kartlar */}
          <div
            style={{
              marginTop: "50px",
              display: "flex",
              flexWrap: "wrap",
              gap: "20px",
              justifyContent: "center"
            }}
          >
            {features.map((item, i) => (
              <div
                key={i}
                style={{
                  backgroundColor: "#f1f8e9",
                  padding: "20px 30px",
                  borderRadius: "15px",
                  minWidth: "200px",
                  textAlign: "center",
                  color: "#2a2f5b",
                  boxShadow: "0 2px 8px rgba(0,0,0,0.05)"
                }}
              >
                <div style={{ marginBottom: "10px" }}>{item.icon}</div>
                <div style={{ fontSize: "16px" }}>{item.label}</div>
              </div>
            ))}
          </div>
        </div>

        {/* Sağ Görsel */}
        <div style={{ flex: "1 1 350px", textAlign: "center" }}>
          <img
            src="/sagAileFotosu.png"
            alt="Anne baba cocuk raf"
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
    </div>
  );
}

export default Giris;
