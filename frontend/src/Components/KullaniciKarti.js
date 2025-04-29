import React from 'react';
import { FaEdit, FaTrash } from "react-icons/fa";
const KullaniciKarti = ({ kullanici, onEdit, onDelete, kartTipi = "musteri" }) => {
    return (
        <div
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
            <p style={{ fontSize: "0.85rem", color: "#777" }}>Rol: {kullanici.role}</p>
            <div style={{ marginTop: "1rem", display: "flex", justifyContent: "space-between" }}>
                {onEdit && (
                    <button
                        onClick={() => onEdit(kullanici)}
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
                )}
                {onDelete && (
                    <button
                        onClick={() => onDelete(kullanici.id)}
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
                )}
            </div>
        </div>
    );
};
export default KullaniciKarti;