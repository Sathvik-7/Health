import React, { useState } from 'react'

const Login = () => {
    const [email,setEmail] = useState("")
    const [password,setPassword] = useState("")
    const [role, setRole] = useState("");

    const [errorEmail,setErrorEmail] = useState("")
    const [errorPassword,setErrorPassword] = useState("")
    const [errorRole,setErrorRole] = useState("")
    
    const [emailColor,setEmailColor] = useState("")
    const [passwordColor,setPasswordColor] = useState("")
    const [roleColor,setRoleColor] = useState("")
    

    const [Message,setMessage] = useState("")
    
    const validateForm = async (e) => 
      {
        e.preventDefault()

        try 
        {
          if(email.includes("@gmail.com"))
        {
          setErrorEmail("")
          setEmailColor("green")
          if(password.length >= 8)
            {
              setErrorPassword("")
              setPasswordColor("green")
              if (role) 
                {
                    setErrorRole("")
                    setRoleColor("green")

                    const response = await postLoginData(email, password);
                    setMessage(`✅ Success: ${response.data.message}`);

                    setEmail("")
                    setPassword("")
                }
              else
                {
                    setErrorRole("Please select one option")
                    setRoleColor("red")
                }
            }
          else 
            {
              setErrorPassword("Password must 8 characters long")
              setPasswordColor("red")
            }
          }
          else
          {
            setErrorEmail("Email should have @gmail.com in it")
            setEmailColor("red")
          }
        } 
        catch (error) 
      {
        if (error.response) 
          {
              // Server responded with an error status (400, 401, 500, etc.)
              setMessage(`❌ Error: ${error.response.data.message}`);
          } 
        else if (error.request) 
          {
              // No response from server
              setMessage("❌ No response from server. Please check API URL.");
          } 
        else {
              // Other errors
              setMessage("❌ Something went wrong.");
            }
        }
      }
   
  return (
    <>
    <form>
        <h2>Sign In!</h2>
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
        <select value={role} onChange={e =>{setRole(e.target.value)}}
            style = {{borderColor:roleColor}} >
        <option value="">-- Select --</option>
        <option value="Admin">Admin</option>
        <option value="HealthProfessional">Health Professional</option>
        </select>
        <p className="error">{errorRole}</p>
        <button className="submit-btn" onClick={validateForm}>Submit</button>
        <p>{Message}</p>
      </form>
      </>
  )
}

export default Login