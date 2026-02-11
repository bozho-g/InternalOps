import { useMutation } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { useAuthStore } from "../../stores/authStore";
import { toast } from "sonner";

export function useRegisterMutation() {
    const setAuth = useAuthStore((state) => state.setAuth);

    return useMutation({
        mutationFn: async (credentials) => {
            const res = await axiosInstance.post('/auth/register', credentials);
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