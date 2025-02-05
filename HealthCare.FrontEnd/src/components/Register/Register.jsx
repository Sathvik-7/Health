import React from 'react'
import { useState } from 'react'
import { postRegisterData } from '../api/Api'
import axios from "axios"

const Register = () => {
    
    const [userName,setUsername] = useState("")
    const [email,setEmail] = useState("")
    const [password,setPassword] = useState("")
    const [confirmPassword,setConfirmPassword] = useState("")
    const [phoneNumber,setPhoneNumber] = useState("")
    const [role, setRole] = useState("");

    const [errorUsername,setErrorUsername] = useState("")
    const [errorEmail,setErrorEmail] = useState("")
    const [errorPassword,setErrorPassword] = useState("")
    const [errorConfirmPassword,setErrorConfirmPassword] = useState("")
    const [errorPhoneNumber,setErrorPhoneNumber] = useState("")
    const [errorRole,setErrorRole] = useState("")

    const [userColor,setUserColor] = useState("")
    const [emailColor,setEmailColor] = useState("")
    const [passwordColor,setPasswordColor] = useState("")
    const [cofirmPasswordColor,setCofirmPasswordColor] = useState("")
    const [phoneNumberColor,setPhoneNumberColor] = useState("")
    const [roleColor,setRoleColor] = useState("")
    
    const [Message,setMessage] = useState("")
        
    const validateForm = async (e) => 
        {
            e.preventDefault()
  
            try 
            {
              const regex = /^[A-Za-z0-9]+$/;
              const phoneRegex = /^\+?[1-9]\d{1,14}$/;

              if(regex.test(userName))
              {
                setErrorUsername("")
                setUserColor("green")
                if(email.includes("@gmail.com"))
                  {
                    setErrorEmail("")
                    setEmailColor("green")
                    if(password.length >= 8)
                      {
                        setErrorPassword("")
                        setPasswordColor("green")
                        if(password == confirmPassword)
                          {
                              setErrorConfirmPassword("")
                              setCofirmPasswordColor("green")
                              if(phoneRegex.test(phoneNumber))
                                {
                                    setErrorPhoneNumber("")
                                    setPhoneNumberColor("green")
                                    if (role) 
                                      {
                                          setErrorRole("")
                                          setRoleColor("green")

                                          const response = await postRegisterData(userName,password,email,phoneNumber,role);

                                          setMessage(`✅ Success: ${response.data.message}`);

                                          setUsername("")
                                          setPassword("")
                                          setEmail("")
                                          setCofirmPasswordColor("")
                                          setPhoneNumber("")
                                          setRole("")

                                      }
                                    else
                                      {
                                          setErrorRole("Please select one option")
                                          setRoleColor("red")
                                      }
                                }
                            else
                                {
                                    setErrorPhoneNumber("PhoneNumber should be of 10 digits. Ex-0123456789")
                                    setPhoneNumberColor("red")
                                }
                          }
                          else 
                          {
                              setErrorConfirmPassword("Password and ConfirmPassword should be same")
                              setCofirmPasswordColor("red")
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
              else
              {
                  setErrorUsername("UserName should not have special characters")
                  setUserColor("red")
              }
            }
            catch(error)
            {
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
        <form>
        <h2>Register !</h2>
        <input type="text" 
                style = {{borderColor:userColor}}
                placeholder='Enter username'
                value={userName} 
                onChange={e => setUsername(e.target.value)}/>
        <p className="error">{errorUsername}</p>
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
        <input type="password" 
                style = {{borderColor:cofirmPasswordColor}}
                placeholder='Re-enter password'
                value={confirmPassword} 
                onChange={e => setConfirmPassword(e.target.value)}/>
        <p className="error">{errorConfirmPassword}</p>
        <input type="text" 
                style = {{borderColor:phoneNumberColor}}
                placeholder='Enter PhoneNumber'
                value={phoneNumber} 
                onChange={e => setPhoneNumber(e.target.value)}/>
        <p className="error">{errorPhoneNumber}</p>
        <select value={role} onChange={e =>{setRole(e.target.value)}}
            style = {{borderColor:roleColor}} >
        <option value="">-- Select --</option>
        <option value="Admin">Admin</option>
        <option value="HealthProfessional">Health Professional</option>
      </select>
      <p className="error">{errorRole}</p>
        <button className="submit-btn" onClick={validateForm}>Register</button>
        <p>{Message}</p>
      </form>
      </>
  );
}

export default Register