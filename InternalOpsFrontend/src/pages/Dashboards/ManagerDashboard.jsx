import styles from './Dashboard.module.css';

export function ManagerDashboard({ data }) {
    return (
        <div className={styles.managerDashboard}>
            <h1>Manager Dashboard</h1>
            <p>Welcome, Manager!</p>
            {data && <pre>{JSON.stringify(data, null, 2)}</pre>}
        </div>
    );
}