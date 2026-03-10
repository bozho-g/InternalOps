import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useAuditLogs(filters = {}) {
    return useQuery({
        queryKey: ["logs", filters],
        queryFn: async () => {
            const res = await axiosInstance.get("/logs", { params: filters });
            return res.data;
        },
        placeholderData: (prev) => prev,
        staleTime: 1000 * 30
    });
}