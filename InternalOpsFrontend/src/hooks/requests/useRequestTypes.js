import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useRequestTypes() {
    return useQuery({
        queryKey: ["request-types"],
        queryFn: async () => {
            const { data } = await axiosInstance.get("/requests/request-types");
            return data;
        },
    });
}