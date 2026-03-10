import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../api/axiosInstance";

export function useRequestTypes() {
    return useQuery({
        queryKey: ["request-types"],
        queryFn: async () => {
            const { data } = await axiosInstance.get("/requests/request-types");
            return data;
        },
        staleTime: Infinity
    });
}

export function useRequestStatuses() {
    return useQuery({
        queryKey: ["request-statuses"],
        queryFn: async () => {
            const { data } = await axiosInstance.get("/requests/request-statuses");
            return data;
        },
        staleTime: Infinity,
    });
}

export function useAuditActions() {
    return useQuery({
        queryKey: ["audit-actions"],
        queryFn: async () => {
            const { data } = await axiosInstance.get("/logs/actions");
            return data;
        }
    });
}