import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useToggleManager(userId) {
    const queryClient = useQueryClient();

    return useMutation({
        mutationKey: ["toggleManager", userId],
        mutationFn: async () => {
            await axiosInstance.post(`/users/${userId}/toggle-manager`);
        },
        onSettled: () => {
            queryClient.invalidateQueries(["users"]);
            queryClient.invalidateQueries(["user", userId]);
        },
    });
}