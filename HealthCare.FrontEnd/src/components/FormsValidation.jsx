
import React, { useState } from 'react'
import '../style.css'
import axios from "axios";

const FormsValidation = () => {

    const [email,setEmail] = useState("")
    const [password,setPassword] = useState("")

    const [errorEmail,setErrorEmail] = useState("")
    const [errorPassword,setErrorPassword] = useState("")
    
    const [emailColor,setEmailColor] = useState("")
    const [passwordColor,setPasswordColor] = useState("")

    const [Message,setMessage] = useState("")
    
    const validateForm = async (e) => 
      {
        e.preventDefault()

        if(email.includes("@gmail.com"))
        {
          setErrorEmail("")
          setEmailColor("green")
        }
        else
        {
          setErrorEmail("Email should have @gmail.com in it")
          setEmailColor("red")
        }

        if(password.length >= 8)
          {
            setErrorPassword("")
            setPasswordColor("green")
          }
          else 
          {
            setErrorPassword("Password must 8 characters long")
            setPasswordColor("red")
          }

          try {
            const response = await axios.post(
              "https://localhost:44344/api/HealthCareAuth/Login",
              { email, password },
              { headers: { "Content-Type": "application/json" } }
            );
      
            setMessage(`✅ Success: ${response.data.message}`);
      
          } catch (error) {
            if (error.response) {
              // Server responded with an error status (400, 401, 500, etc.)
              setMessage(`❌ Error: ${error.response.data.message}`);
            } else if (error.request) {
              // No response from server
              setMessage("❌ No response from server. Please check API URL.");
            } else {
              // Other errors
              setMessage("❌ Something went wrong.");
            }
          }
      }
    
  return (
    <>
    <div className="card">
      <div className="card-image"></div>
      <form>
        <input type="text" 
                style = {{borderColor:emailColor}}
                placeholder='Enter email'
                value={email} 
                onChange={e => setEmail(e.target.value)}/>
        <p className="error">{errorEmail}</p>
        <input type="password" 
                style = {{borderColor:passwordColor}}
                placeholder='Enter password'
                value={password} 
                onChange={e => setPassword(e.target.value)}/>
        <p className="error">{errorPassword}</p>
        <button className="submit-btn" onClick={validateForm}>Submit</button>
        <p>{Message}</p>
      </form>
    </div>
    </>
  )
}

export default FormsValidation
