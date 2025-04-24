import './App.css'
import { useState } from 'react'
import { Routes, Route } from 'react-router-dom' //  yönlendirme için
import { useLocation } from 'react-router-dom'

import Footer from './Footer'
import Giris from './Giris'
import Navbar from './Navbar'
import SideBar from './SideBar'
import UrunListesi from './Yonetim/UrunListesi'
import MusteriYonetimi from './Yonetim/MusteriYonetimi'
import AdminYonetimi from './Yonetim/AdminYonetimi'
import UrunYonetimi from './Yonetim/UrunYonetimi'
import StokRaporları from './Raporlar/StokRaporları'
import SatisRaporlari from './Raporlar/SatisRaporlari'
import Satislar from './Satislar/Satislar'
import Login from './Login'
import KategoriYonetimi from './Yonetim/KategoriYonetimi'
import SepetYonetimi from './SepetYonetimi'
import SiparisYonetimi from './SiparisYonetimi'
import UrunListesiMusteri from './UrunlerForMusteri'


function App() {
  const [searchTerm, setSearchTerm] = useState('')
  const location = useLocation()
  const isLoginPage = location.pathname === '/login'


  return (
    <div className="App">
      <Navbar onSearch={setSearchTerm} />
      <div style={{ display: 'flex' }}>
      {!isLoginPage && <SideBar />}
        <div style={{ flex: 1, padding: '1rem', backgroundColor: '#e9ecef', height: "100vh" }}>
          {/*  Sayfa yönlendirmeleri burada */}
          <Routes>
            <Route path="/" element={<Giris />} />
            <Route path="/urun-listesi" element={ <UrunListesi searchTerm={searchTerm} />} />
            <Route path="/musteri-yonetimi" element={<MusteriYonetimi />} />
            <Route path="/admin-yonetimi" element={<AdminYonetimi />} /> 
            <Route path="/urun-yonetimi" element={<UrunYonetimi/>} />
            <Route path="/stok-raporlari" element={<StokRaporları />} />
            <Route path="/satis-raporlari" element={<SatisRaporlari />} />
            <Route path="/satislar" element={<Satislar />} />
            <Route path="/login" element={<Login />} />
            <Route path="/kategori-yonetimi" element={<KategoriYonetimi />} />
            <Route path="/sepet" element={<SepetYonetimi />}/>
            <Route path="/siparis" element={<SiparisYonetimi />}/>
            <Route path="/urun-listesiMusteri" element={ <UrunListesiMusteri searchTerm={searchTerm} />} />


          </Routes>
        </div>
      </div>
      {!isLoginPage && <Footer />}

    </div>
  )
}

export default App
