import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";
import { toast } from "sonner";

export function useAddComment() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async ({ requestId, content }) => {
            const res = await axiosInstance.post(`/comments/${requestId}`, { content });
            return res.data;
        },
        onSuccess: (_, { requestId }) => {
            queryClient.invalidateQueries({ queryKey: ["request", String(requestId)] });
        },
        onError: (error) => {
            toast.error(error.response?.data?.detail ?? "Failed to add comment");
        }
    });
}

export function useUpdateComment() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async ({ commentId, content }) => {
            const res = await axiosInstance.put(`/comments/${commentId}`, { content });
            return res.data;
        },
        onSuccess: (_, { requestId }) => {
            queryClient.invalidateQueries({ queryKey: ["request", String(requestId)] });
        },
        onError: (error) => {
            toast.error(error.response?.data?.detail ?? "Failed to update comment");
        }
    });
}

export function useDeleteComment() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async ({ commentId }) => {
            await axiosInstance.delete(`/comments/${commentId}`);
        },
        onSuccess: (_, { requestId }) => {
            toast.success("Comment deleted");
            queryClient.invalidateQueries({ queryKey: ["request", String(requestId)] });
        },
        onError: (error) => {
            toast.error(error.response?.data?.detail ?? "Failed to delete comment");
        }
    });
}
