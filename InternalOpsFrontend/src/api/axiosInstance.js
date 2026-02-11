import axios from "axios";
import { useAuthStore } from "../stores/authStore";
import { toast } from "sonner";

let isRefreshing = false;
let refreshQueue = [];

const axiosInstance = axios.create({
    baseURL: '/api',
    withCredentials: true
});

axiosInstance.interceptors.request.use((config) => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) {
        config.headers["Authorization"] = `Bearer ${accessToken}`;
    }
    return config;
});

axiosInstance.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status !== 401 || originalRequest._retry) {
            return Promise.reject(error);
        }

        originalRequest._retry = true;
        const authError = error.response.headers["x-auth-error"];

        if (authError !== "token_expired") {
            useAuthStore.getState().logout();
            return Promise.reject(error);
        }

        if (isRefreshing) {
            return new Promise((resolve) => {
                refreshQueue.push((token) => {
                    originalRequest.headers.Authorization = `Bearer ${token}`;
                    resolve(axiosInstance(originalRequest));
                });
            });
        }

        isRefreshing = true;

        try {
            const { data } = await axios.post("/api/auth/refresh", { withCredentials: true });

            localStorage.setItem("accessToken", data.token);

            refreshQueue.forEach((cb) => cb(data.token));
            refreshQueue = [];

            originalRequest.headers.Authorization = `Bearer ${data.token}`;
            return axiosInstance(originalRequest);
        } catch {
            useAuthStore.getState().logout();
            toast.error('Session expired. Please log in again.');
            return Promise.reject(error);
        } finally {
            isRefreshing = false;
        }
    }
);

export default axiosInstance;