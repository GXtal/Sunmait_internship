import api from "../api/axios"

export default class AuthService{
    static async login(email, password){

        const response = await api.post('/Users/login', {email, password})
        localStorage.setItem('accessToken', response.data.token)
    }
    static async registration(username, email, password){
        const response = await api.post('/Users/register', {username,email, password})
        localStorage.setItem('accessToken', response.data.token)
    }
    static async logout(){
        localStorage.removeItem('accessToken')
    }
}