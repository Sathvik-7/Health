import axios from "axios"

const api = axios.create(
    {
        baseURL : "https://localhost:44344/api/HealthCareAuth"
    });


export const postLoginData = (email, password) => 
    {
        return api.post(
            "/Login",
            { email, password },
            { headers: { "Content-Type": "application/json" } }
          );
    }

export const postRegisterData = (username,password,email,phonenumber,role) =>
    {
        return api.post("/Register",
                {username,password,email,phonenumber,role},
                { headers: { "Content-Type": "application/json" } }
            )
    }