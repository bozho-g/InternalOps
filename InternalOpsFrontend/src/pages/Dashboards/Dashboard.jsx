import { useDashboard } from "../../hooks/useDashboard";
import { useAuthStore } from "../../stores/authStore";
import { AdminDashboard } from "./AdminDashboard";
import { ManagerDashboard } from "./ManagerDashboard";
import { UserDashboard } from "./UserDashboard";

import styles from './Dashboard.module.css';

export function Dashboard() {
    const { data, isLoading, error } = useDashboard();
    const { user } = useAuthStore();

    if (isLoading) return <span className="loader"></span>;

    if (error) return <div className={styles.error}>Error loading dashboard.</div>;

    if (user?.role === 'admin') {
        return <AdminDashboard data={data} />;
    } else if (user?.role === 'manager') {
        return <ManagerDashboard data={data} />;
    } else {
        return <UserDashboard data={data} />;
    }
}