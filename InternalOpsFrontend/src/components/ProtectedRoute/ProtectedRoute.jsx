import { AuthPage } from "../../pages/AuthPage/AuthPage";
import { useAuthStore } from "../../stores/authStore";

export function ProtectedRoute({ allowedRoles, children }) {
    const { user } = useAuthStore();

    if (!user) {
        return <AuthPage />;
    }

    return children;
}