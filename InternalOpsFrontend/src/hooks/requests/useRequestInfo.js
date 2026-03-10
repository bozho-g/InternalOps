import { useAuthStore } from "../../stores/authStore";

export function useRequestInfo(request) {
    const user = useAuthStore((state) => state.user);

    return {
        isAdmin: user?.role === "admin",
        isManager: user?.role === "manager",
        isOwner: user?.email === request?.requestedBy?.email,
        isPending: request?.status === "Pending",
        isApproved: request?.status === "Approved",
        isAdminOrManager: user?.role === "admin" || user?.role === "manager"
    };
}