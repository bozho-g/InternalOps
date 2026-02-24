import { NavLink } from 'react-router-dom';
import styles from './StatCard.module.css';

export function StatCard({ title, value, icon, toLink }) {
    return (
        <NavLink className={styles.statCard} to={toLink}>
            <div className={styles.statContent}>
                <h3 className={styles.statLabel}>{title}</h3>
                <p className={styles.statValue}>{value}</p>
            </div>
            {icon}
        </NavLink>
    );
}