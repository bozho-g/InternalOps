import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useRequests(filters = {}) {
    return useQuery({
        queryKey: ["requests", filters],
        queryFn: () => axiosInstance
            .get("/requests", { params: { take: 10, ...filters } })
            .then(res => res.data),
        placeholderData: (prev) => prev,
        staleTime: 1000 * 30
    });
}