import axios from "axios";

export const BASE_URL = "http://localhost:5248"; // Ensure this matches your backend URL
export const myAxios = axios.create({
  baseURL: BASE_URL,
  headers: {
    "Content-Type": "application/json",
    Authorization: `Bearer ${localStorage.getItem("token")}`, // Add this line
  },
});
