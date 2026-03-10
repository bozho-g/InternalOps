import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { toast } from "sonner";

export function useUpdateRequest() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async ({ requestId, patchDoc }) => {
            const res = await axiosInstance.patch(`/requests/${requestId}`, patchDoc, {
                headers: { 'Content-Type': 'application/json-patch+json' }
            });
            return res.data;
        },
        onSuccess: (_, { requestId }) => {
            toast.success("Request updated successfully");
            queryClient.invalidateQueries({ queryKey: ["request", String(requestId)] });
            queryClient.invalidateQueries({ queryKey: ["requests"] });
        },
        onError: (error) => {
            toast.error(error.response?.data?.detail ?? "Update failed");
        }
    });
}
