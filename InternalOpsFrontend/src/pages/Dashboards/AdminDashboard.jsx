import styles from './Dashboard.module.css';

export function AdminDashboard({ data }) {
    return (
        <div className={styles.adminDashboard}>
            <h1>Admin Dashboard</h1>
            <p>Welcome, Admin!</p>
            {data && <pre>{JSON.stringify(data, null, 2)}</pre>}
        </div>
    );
}