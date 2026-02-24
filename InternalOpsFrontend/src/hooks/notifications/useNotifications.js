import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useNotifications() {
    return useQuery({
        queryKey: ["notifications"],
        queryFn: async () => {
            return (await axiosInstance.get("/notifications")).data;
        },
    });
}