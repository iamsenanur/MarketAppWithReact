import React from 'react'
const UrunKarti=({onEdit, onDelete,urun})=>{
    return(
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
            <h3>{urun.urunAdi}</h3>
            <p>ID: {urun.urunID}</p>
            <p>Kategori ID: {urun.urunKategoriID}</p>
        </div>
    );
};
export default UrunKarti;