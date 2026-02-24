import { useMutation, useQueryClient } from "@tanstack/react-query";
import axiosInstance from "../../api/axiosInstance";

export function useCreateRequest() {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: async (payload) => {
            const data = await axiosInstance.post("/requests", payload);
            return data;
        },
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["requests"] });
            queryClient.invalidateQueries({ queryKey: ["dashboard"] });
        }
    });
}