import { useQuery } from "@tanstack/react-query";
import { useAuthStore } from "../stores/authStore";
import axiosInstance from "../api/axiosInstance";

export function useDashboard() {
    const user = useAuthStore((state) => state.user);

    const endpoint = user.role === 'admin'
        ? '/dashboards/admin'
        : user.role === 'manager'
            ? '/dashboards/manager'
            : '/dashboards/user';

    return useQuery({
        queryKey: ['dashboard'],
        queryFn: async () => {
            return (await axiosInstance.get(endpoint)).data;
        },
    });
}