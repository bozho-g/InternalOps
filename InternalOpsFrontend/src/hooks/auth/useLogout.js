import { useQueryClient } from "@tanstack/react-query";
import axiosInstance from "@/api/axiosInstance";
import { useAuthStore } from "@/stores/authStore";

export function useLogout() {
    const logout = useAuthStore((s) => s.logout);
    const queryClient = useQueryClient();

    return () => {
        logout();
        queryClient.clear();
        axiosInstance.post("/auth/logout");
    };
}