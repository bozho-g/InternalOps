import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { toast } from "sonner";

export function useRequestAction() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async ({ requestId, action }) => {
            const res = await axiosInstance.post(`/requests/${requestId}/${action}`);
            return res.data;
        },
        onSuccess: (_, { requestId }) => {
            queryClient.invalidateQueries({ queryKey: ['requests'] });
            queryClient.invalidateQueries({ queryKey: ["request", String(requestId)] });
        },
        onError: (error) => {
            const detail =
                error.response?.data?.detail ??
                "Request failed";

            toast.error(detail);
        }
    });
}