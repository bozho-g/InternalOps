import { useQuery } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useGetRequestById(id) {
    return useQuery({
        queryKey: ['request', id],
        queryFn: async () => {
            const res = await axiosInstance.get(`/requests/${id}`);
            return res.data;
        },
        retry: (failureCount, error) => {
            if (error?.response?.status === 400) {
                return false;
            }

            return failureCount < 2;
        }
    });
}