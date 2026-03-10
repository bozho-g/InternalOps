import { NavLink, useNavigate } from 'react-router-dom';
import styles from './UserCard.module.css';
import { useToggleManager } from '../../hooks/users/useToggleManager';

export default function UserCard({ user }) {
    const isAdmin = user.roles.includes("Admin");
    const navigate = useNavigate();
    const isManager = user.isManager;

    const toggleManagerMutation = useToggleManager(user.id);

    return (
        <div className={styles.card} onClick={() => navigate(`/requests?userId=${user.id}`)}>
            <p className={styles.email}>{user.email}</p>
            <p className={styles.meta}>
                Requests: {user.totalRequests}
            </p>
            <p className={styles.role}>
                {isAdmin ? "Admin" : isManager ? "Manager" : "User"}
            </p>

            {!isAdmin && (
                <button
                    className={styles.managerButton}
                    onClick={(e) => { e.stopPropagation(); toggleManagerMutation.mutate(); }}
                    disabled={toggleManagerMutation.isPending}
                >
                    {toggleManagerMutation.isPending ? "Updating..." : isManager ? "Remove Manager" : "Make Manager"}
                </button>
            )}
        </div>
    );
}