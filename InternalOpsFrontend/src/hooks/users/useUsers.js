import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { useAuthStore } from "../../stores/authStore";

export function useUsers(filters = {}) {
    const { user } = useAuthStore();

    const isAdminOrManager = user?.role === "admin" || user?.role === "manager";

    return useQuery({
        queryKey: ["users", filters],
        queryFn: async () => {
            const res = await axiosInstance.get("/users", { params: { pageSize: 50, ...filters } });
            return res.data;
        },
        placeholderData: (prev) => prev,
        staleTime: 1000 * 30,
        enabled: isAdminOrManager
    });
}