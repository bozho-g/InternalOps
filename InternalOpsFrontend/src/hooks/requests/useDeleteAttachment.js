import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { toast } from "sonner";

export function useDeleteAttachment() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async ({ attachmentId }) => {
            await axiosInstance.delete(`/attachments/${attachmentId}`);
        },
        onSuccess: (_, { requestId }) => {
            toast.success("Attachment removed");
            queryClient.invalidateQueries({ queryKey: ["request", String(requestId)] });
        },
        onError: (error) => {
            toast.error(error.response?.data?.detail ?? "Failed to remove attachment");
        }
    });
}
