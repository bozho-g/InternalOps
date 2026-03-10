import { Navigate } from "react-router-dom";
import { AuthPage } from "../../pages/AuthPage/AuthPage";
import { useAuthStore } from "../../stores/authStore";

export function ProtectedRoute({ allowedRoles, children }) {
    const { user } = useAuthStore();

    if (!user) {
        return <AuthPage />;
    }

    if (allowedRoles && !allowedRoles?.includes(user.role)) {
        return <Navigate to="/" replace />;
    }

    return children;
}