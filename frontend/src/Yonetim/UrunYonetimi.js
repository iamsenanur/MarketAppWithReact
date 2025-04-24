import React, { useState, useEffect } from 'react'
import axios from 'axios'
import {
  MDBContainer,
  MDBCard,
  MDBCardBody,
  MDBInput,
  MDBBtn,
  MDBCheckbox
} from 'mdb-react-ui-kit'

const UrunEkle = () => {
  const [form, setForm] = useState({
    urunAdi: '',
    urunBarkod: '',
    urunKategoriID: '',
    isActive: true
  })

  const [mesaj, setMesaj] = useState('')

  // MDB stil dosyasını yükle
  useEffect(() => {
    const link = document.createElement('link')
    link.rel = 'stylesheet'
    link.href = 'https://cdnjs.cloudflare.com/ajax/libs/mdb-ui-kit/6.4.1/mdb.min.css'
    link.id = 'mdb-css'
    document.head.appendChild(link)

    return () => {
      const existing = document.getElementById('mdb-css')
      if (existing) document.head.removeChild(existing)
    }
  }, [])

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target
    setForm((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value
    }))
  }

  const handleSubmit = async (e) => {
    e.preventDefault()

    try {
      const token = localStorage.getItem('token')
      if (!token) {
        setMesaj("Giriş yapmadığınız için işlem yapılamıyor.")
        return
      }

      const formData = {
        urunAdi: form.urunAdi,
        urunBarkod: form.urunBarkod,
        urunKategoriID: parseInt(form.urunKategoriID) || 0,
        isActive: form.isActive
      }

      const response = await axios.post('https://localhost:7264/api/product/add', formData, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      })

      setMesaj(response.data || "Ürün başarıyla eklendi.")
      setForm({
        urunAdi: '',
        urunBarkod: '',
        urunKategoriID: '',
        isActive: true
      })
    } catch (err) {
      console.error(err)
      setMesaj(err.response?.data || 'Bir hata oluştu!')
    }
  }

  return (
    <MDBContainer
      className="d-flex justify-content-center align-items-center"
      style={{ minHeight: '100vh' }}
    >
      <MDBCard style={{ maxWidth: '500px', width: '100%' }}>
        <MDBCardBody className="p-4">
          <h3 className="text-center mb-4">Ürün Ekle</h3>

          {mesaj && (
            <div className="alert alert-info text-center" role="alert">
              {mesaj}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <MDBInput
              label="Ürün Adı"
              name="urunAdi"
              type="text"
              value={form.urunAdi}
              onChange={handleChange}
              wrapperClass="mb-4"
              required
            />

            <MDBInput
              label="Ürün Barkodu"
              name="urunBarkod"
              type="text"
              value={form.urunBarkod}
              onChange={handleChange}
              wrapperClass="mb-4"
              required
            />

            <MDBInput
              label="Kategori ID"
              name="urunKategoriID"
              type="number"
              value={form.urunKategoriID}
              onChange={handleChange}
              wrapperClass="mb-4"
              required
            />

<div className="form-check mb-4 d-flex align-items-center">
  <input
    className="form-check-input"
    type="checkbox"
    id="isActive"
    name="isActive"
    checked={form.isActive}
    onChange={handleChange}
    style={{ marginRight: '0.5rem' }}
  />
  <label className="form-check-label" htmlFor="isActive" style={{ marginBottom: 0 }}>
    Ürün Aktif mi?
  </label>
</div>


            <MDBBtn type="submit" color="success" className="w-100">
              Ürünü Ekle
            </MDBBtn>
          </form>
        </MDBCardBody>
      </MDBCard>
    </MDBContainer>
  )
}

export default UrunEkle
