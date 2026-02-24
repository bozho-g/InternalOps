import { RequestsPanel } from '../../components/RequestsPanel/RequestsPanel';
import { StatCard } from '../../components/StatCard/StatCard';
import styles from './Dashboard.module.css';
import { ClipboardCheck, XCircle, Clock, CheckCheck } from 'lucide-react';

export function UserDashboard({ data }) {
    return (
        <div className={styles.dashboard}>
            <div className={styles.statCardsContainer}>
                <StatCard title="Pending Requests" value={data?.myPending} icon={<Clock />} toLink="/requests/pending" />
                <StatCard title="Approved Requests" value={data?.myApproved} icon={<CheckCheck />} toLink="/requests/approved" />
                <StatCard title="Rejected Requests" value={data?.myRejected} icon={<XCircle />} toLink="/requests/rejected" />
                <StatCard title="Completed Requests" value={data?.myCompleted} icon={<ClipboardCheck />} toLink="/requests/completed" />
            </div>

            <div className={styles.requestsWrapper}>
                <h2>Latest Submitted Requests</h2>
                <RequestsPanel filters={{ take: 3 }} showFilterBar={false} />
            </div>
        </div>
    );
}