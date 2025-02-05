
import React, { useState } from 'react'
import '../style.css'
import Login from './Login/Login';
import Register from './Register/Register';

const FormsValidation = () => {

  const [isActive , setActive] = useState(true)

  return (
    <>
    <div className="card">
      <div className="card-image"></div>
      {isActive ? <Register/> : <Login/>}
    </div>
    </>
  )
}

export default FormsValidation
