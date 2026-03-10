import { useMutation } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useChangePasswordMutation() {
    return useMutation({
        mutationFn: async (passwordData) => {
            return await axiosInstance.post("/auth/change-password", passwordData);
        },
    });
}