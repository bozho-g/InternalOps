import { useMutation } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useChangePasswordMutation() {
    return useMutation({
        mutationFn: (passwordData) => {
            return axiosInstance.post("/auth/change-password", passwordData);
        },
    });
}