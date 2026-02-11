import { useMutation } from "@tanstack/react-query";
import axiosInstance from "@/api/axiosInstance";
import { useAuthStore } from "@/stores/authStore";

export function useLogoutMutation() {
    const logout = useAuthStore((s) => s.logout);

    return useMutation({
        mutationFn: () =>
            axiosInstance.post("/auth/logout", { withCredentials: true }),
        onSettled: () => {
            logout();
        },
    });
}