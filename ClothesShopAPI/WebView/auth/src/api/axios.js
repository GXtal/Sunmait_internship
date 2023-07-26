import axios from "axios";

export const API_URL = 'http://localhost:5233/api/'

const api = axios.create(
    {
        withCredentials: true,
        baseURL: API_URL,
        headers: { 'Content-Type': 'application/json' },
    }
)

api.interceptors.request.use((config) => {
    if (localStorage.getItem('accessToken')) {
        config.headers.Authorization = 'Bearer ' + localStorage.getItem('accessToken')
    }
    return config;
}, (err) => {
    console.log(err);
    return Promise.reject(err);
})

api.interceptors.response.use((config) => {
    return config
}, async (error) => {
    return Promise.reject(error.response.data);
})

export default api