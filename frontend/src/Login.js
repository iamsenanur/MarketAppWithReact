import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import axios from 'axios'
import {
  MDBContainer,
  MDBRow,
  MDBCol,
  MDBCard,
  MDBCardBody,
  MDBInput,
  MDBBtn
} from 'mdb-react-ui-kit'

function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const navigate = useNavigate()

  // MDB stilini sadece bu sayfada kullan
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

  const handleLogin = async (e) => {
    e.preventDefault()

    try {
      const res = await axios.post('https://localhost:7264/api/auth/login', {
        username: email,
        password: password
      })

      localStorage.setItem('token', res.data.token)
      navigate('/')
    } catch (err) {
      alert('Kullanıcı adı veya şifre hatalı!')
    }
  }

  return (
    <MDBContainer
      fluid
      style={{ backgroundColor: '#e9ecef', minHeight: '100vh' }}
      className='d-flex justify-content-center align-items-center'
    >
      <MDBRow>
        <MDBCol>
          <MDBCard style={{ borderRadius: '1rem', maxWidth: '400px', margin: 'auto' }}>
            <MDBCardBody className="p-5">
              <h2 className="fw-bold mb-4 text-center">Giriş Yap</h2>
              <form onSubmit={handleLogin}>
                <MDBInput
                  wrapperClass="mb-4"
                  label="Kullanıcı Adı"
                  id="email"
                  type="text"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                />
                <MDBInput
                  wrapperClass="mb-4"
                  label="Şifre"
                  id="password"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                />
                <MDBBtn className="w-100 mb-3" color="success" size="lg" type="submit">
                  Giriş Yap
                </MDBBtn>
              </form>
            </MDBCardBody>
          </MDBCard>
        </MDBCol>
      </MDBRow>
    </MDBContainer>
  )
}

export default Login
