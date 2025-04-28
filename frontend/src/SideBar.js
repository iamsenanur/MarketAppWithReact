import { useState } from "react";
import { useLocation, useNavigate, Link } from "react-router-dom";
import { jwtDecode } from 'jwt-decode'

import {
  FaHome, FaClipboardList, FaShoppingCart, FaChartBar,
  FaChevronDown, FaChevronUp, FaAngleRight, FaSignOutAlt, FaShoppingBasket, FaCog, FaClipboardCheck 
} from "react-icons/fa";

function SideBar() {
  const [openYonetim, setOpenYonetim] = useState(false);
  const [openRapor, setOpenRapor] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();

  const isActive = (path) => location.pathname === path;

  // Token çözümleme
  const token = localStorage.getItem("token");
  const isAuthenticated = !!token;

  let decoded = null;
  try {
    decoded = token ? jwtDecode(token) : null;
  } catch {
    decoded = null;
  }

  // Role ve claim'lere göre kontrol
  const userRole = decoded?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  const isAdmin = userRole === "Admin";
  const isMusteri = userRole === "Müşteri";


  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/login");
  };

  return (
    <div
    style={{
      width: "220px",
      backgroundColor: "#388E3C",
      color: "white",
      padding: "1.5rem 1rem",
      boxSizing: "border-box",
      fontSize: "1.1rem",
      position: "relative",
      display: "flex",
      flexDirection: "column"
    }}
  >
  
      <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
        <SideBarItem icon={<FaHome />} text="AnaSayfa" to="/" />

        {/* Yönetim Açılır Menü */}
        {isAdmin && (
          <li>
            <div
              onClick={() => setOpenYonetim(!openYonetim)}
              style={{
                ...linkStyle,
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
                padding: "0.5rem 1rem",
                borderRadius: "8px",
                cursor: "pointer",
                marginBottom: "0.5rem"
              }}
              onMouseOver={(e) => (e.currentTarget.style.backgroundColor = "#2E7D32")}
              onMouseOut={(e) => (e.currentTarget.style.backgroundColor = "transparent")}
            >
              <div style={{ display: "flex", alignItems: "center", gap: "0.8rem" }}>
                <FaCog  />
                <span>Yönetim</span>
              </div>
              {openYonetim ? <FaChevronUp /> : <FaChevronDown />}
            </div>

            {openYonetim && (
              <ul style={{ listStyle: "none", paddingLeft: "1.8rem", marginBottom: "1rem" }}>
                {isAdmin && (
                  <li>
                    <Link to="/admin-yonetimi" style={{
                      ...subLinkStyle,
                      backgroundColor: isActive("/admin-yonetimi") ? "#2E7D32" : "transparent",
                      borderRadius: "6px",
                      padding: "0.4rem 0.6rem"
                    }}>
                      <FaAngleRight style={{ marginRight: 6 }} />
                      Admin Yönetimi
                    </Link>
                  </li>
                )}
                <li>
                  <Link to="/musteri-yonetimi" style={{
                    ...subLinkStyle,
                    backgroundColor: isActive("/musteri-yonetimi") ? "#2E7D32" : "transparent",
                    borderRadius: "6px",
                    padding: "0.4rem 0.6rem"
                  }}>
                    <FaAngleRight style={{ marginRight: 6 }} />
                    Müşteri Yönetimi
                  </Link>
                </li>
                <li>
                  <Link to="/kategori-yonetimi" style={{
                    ...subLinkStyle,
                    backgroundColor: isActive("/kategori-yonetimi") ? "#2E7D32" : "transparent",
                    borderRadius: "6px",
                    padding: "0.4rem 0.6rem"
                  }}>
                    <FaAngleRight style={{ marginRight: 6 }} />
                    Kategori Yönetimi
                  </Link>
                </li>
                <li>
                  <Link to="/urun-yonetimi" style={{
                    ...subLinkStyle,
                    backgroundColor: isActive("/urun-yonetimi") ? "#2E7D32" : "transparent",
                    borderRadius: "6px",
                    padding: "0.4rem 0.6rem"
                  }}>
                    <FaAngleRight style={{ marginRight: 6 }} />
                    Ürün Yönetimi
                  </Link>
                </li>
                <li>
                  <Link to="/urun-listesi" style={{
                    ...subLinkStyle,
                    backgroundColor: isActive("/urun-listesi") ? "#2E7D32" : "transparent",
                    borderRadius: "6px",
                    padding: "0.4rem 0.6rem"
                  }}>
                    <FaAngleRight style={{ marginRight: 6 }} />
                    Ürün Listesi
                  </Link>
                </li>
              </ul>
            )}
          </li>
        )}
        {isMusteri && (
          <SideBarItem icon={<FaClipboardList />} text="Ürün Listesi" to="/urun-listesiMusteri" />
        )}
        {isMusteri && (
          <SideBarItem icon={<FaShoppingBasket />} text="Sepet Yönetimi" to="/sepet" />
        )}
        {isMusteri && (
          <SideBarItem icon={<FaClipboardCheck  />} text="Sipariş Yönetimi" to="/siparis" />
        )}

        {isAdmin && (
          <SideBarItem icon={<FaShoppingCart />} text="Satışlar" to="/satislar" />
        )}






        


        {/* Raporlar Açılır Menü */}
        {isAdmin && (
          <li>
            <div
              onClick={() => setOpenRapor(!openRapor)}
              style={{
                ...linkStyle,
                display: "flex",
                alignItems: "center",
                justifyContent: "space-between",
                padding: "0.5rem 1rem",
                borderRadius: "8px",
                cursor: "pointer",
                marginBottom: "0.5rem"
              }}
              onMouseOver={(e) => (e.currentTarget.style.backgroundColor = "#2E7D32")}
              onMouseOut={(e) => (e.currentTarget.style.backgroundColor = "transparent")}
            >
              <div style={{ display: "flex", alignItems: "center", gap: "0.8rem" }}>
                <FaChartBar />
                <span>Raporlar</span>
              </div>
              {openRapor ? <FaChevronUp /> : <FaChevronDown />}
            </div>

            {openRapor && (
              <ul style={{ listStyle: "none", paddingLeft: "1.8rem", marginBottom: "1rem" }}>
                <li>
                  <Link to="/stok-raporlari" style={{
                    ...subLinkStyle,
                    backgroundColor: isActive("/stok-raporlari") ? "#2E7D32" : "transparent",
                    borderRadius: "6px",
                    padding: "0.4rem 0.6rem"
                  }}>
                    <FaAngleRight style={{ marginRight: 6 }} />
                    Stok Raporları
                  </Link>
                </li>
                <li>
                  <Link to="/satis-raporlari" style={{
                    ...subLinkStyle,
                    backgroundColor: isActive("/satis-raporlari") ? "#2E7D32" : "transparent",
                    borderRadius: "6px",
                    padding: "0.4rem 0.6rem"
                  }}>
                    <FaAngleRight style={{ marginRight: 6 }} />
                    Satış Raporları
                  </Link>
                </li>
              </ul>
            )}
          </li>
        )}


        {/* Çıkış */}
        {isAuthenticated && (isAdmin || isMusteri) && (
  <li onClick={handleLogout} style={{ cursor: 'pointer', marginBottom: "1.2rem" }}>
    <div
      style={{
        ...linkStyle,
        display: "flex",
        alignItems: "center",
        gap: "0.8rem",
        padding: "0.5rem 1rem",
        borderRadius: "8px",
        color: "orange"
      }}
      onMouseOver={(e) => (e.currentTarget.style.backgroundColor = "#2E7D32")}
      onMouseOut={(e) => (e.currentTarget.style.backgroundColor = "transparent")}
    >
      <FaSignOutAlt />
      <span>Çıkış Yap</span>
    </div>
  </li>
)}

      </ul>

      <img
        src="/sidebarCocuk.png"
        alt="Çocuk"
        style={{
          position: "absolute",
          bottom: "90px",
          left: "165px",
          width: "170px",
          height: "auto"
        }}
      />
    </div>
  );
}

function SideBarItem({ icon, text, to }) {
  const location = useLocation();
  const isActive = location.pathname === to;

  return (
    <li style={{ marginBottom: "1.2rem" }}>
      <Link
        to={to}
        style={{
          ...linkStyle,
          display: "flex",
          alignItems: "center",
          gap: "0.8rem",
          padding: "0.5rem 1rem",
          borderRadius: "8px",
          transition: "background-color 0.3s ease",
          backgroundColor: isActive ? "#2E7D32" : "transparent"
        }}
        onMouseOver={(e) => (e.currentTarget.style.backgroundColor = "#2E7D32")}
        onMouseOut={(e) => {
          if (!isActive) e.currentTarget.style.backgroundColor = "transparent";
        }}
      >
        <span>{icon}</span>
        <span>{text}</span>
      </Link>
    </li>
  );
}

const linkStyle = {
  color: "#F5F5F5",
  textDecoration: "none"
};

const subLinkStyle = {
  color: "#ffffff",
  textDecoration: "none",
  display: "flex",
  alignItems: "center",
  fontSize: "0.95rem"
};

export default SideBar;
