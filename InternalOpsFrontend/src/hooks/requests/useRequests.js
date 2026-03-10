import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useRequests(filters = {}) {
    return useQuery({
        queryKey: ["requests", filters],
        queryFn: () => axiosInstance
            .get("/requests", { params: filters })
            .then(res => res.data),
        placeholderData: (prev) => prev,
        retry: (failureCount, error) => {
            if (error?.response?.status === 400) {
                return false;
            }

            return failureCount < 2;
        }
    });
}