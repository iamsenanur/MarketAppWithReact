
import { FaShoppingBasket } from "react-icons/fa";

function Navbar() {
  return (
    <nav
      style={{
        position: "sticky",
        top: 0,
        zIndex: 1000,
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
        backgroundColor: "#4CAF50",
        padding: "1rem 2rem",
        color: "white"
      }}
    >
      <div style={{ fontSize: "1.5rem", fontWeight: "bold" }}>
        <FaShoppingBasket style={{ marginRight: 8 }} /> Market Uygulaması
      </div>
    </nav>
  )
}


export default Navbar
