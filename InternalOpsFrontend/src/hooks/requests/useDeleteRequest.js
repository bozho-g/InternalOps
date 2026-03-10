import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { toast } from "sonner";

export function useDeleteRequest() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (requestId) => {
            await axiosInstance.delete(`/requests/${requestId}`);
        },

        onSuccess: () => {
            toast.success("Request deleted successfully");
            queryClient.invalidateQueries({ queryKey: ["requests"] });
        },

        onError: (error) => {
            const detail =
                error.response?.data?.detail ??
                "Delete failed";

            toast.error(detail);
        }
    });
}