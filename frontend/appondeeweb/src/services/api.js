import axios from "axios";

const api = axios.create({
    baseURL: 'http://localhost:5127/'
});

export default api;