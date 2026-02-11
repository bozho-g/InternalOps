import { useMutation } from "@tanstack/react-query";
import { useAuthStore } from "../../stores/authStore";
import axiosInstance from "../../api/axiosInstance";
import { toast } from "sonner";

export function useLoginMutation() {
    const setAuth = useAuthStore((state) => state.setAuth);

    return useMutation({
        mutationFn: async (credentials) => {
            const res = await axiosInstance.post('/auth/login', credentials);
            return res.data;
        },
        onSuccess: (data) => {
            setAuth(data);
        },
        onError: (error) => {
            const detail =
                error.response?.data?.detail ??
                "Invalid Credentials";

            toast.error(detail);
        }
    });
};