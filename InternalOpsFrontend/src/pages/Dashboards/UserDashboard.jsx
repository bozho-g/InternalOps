import { RequestsPanel } from '../../components/RequestsPanel/RequestsPanel';
import { StatCard } from '../../components/StatCard/StatCard';
import { useRequestStatuses } from '../../hooks/useEnums';
import { statusIconsRegistry } from '../../utils/statusIconsRegistry';
import styles from './Dashboard.module.css';

export function UserDashboard({ data }) {
    const { data: statuses } = useRequestStatuses();

    return (
        <div className={styles.dashboard}>
            <div className={styles.statCardsContainer}>
                {statuses?.map((status, index) => {
                    const Icon = statusIconsRegistry[status.value];

                    return (
                        <StatCard
                            key={index}
                            title={`${status.label} Requests`}
                            value={data?.[`my${status.label}`]}
                            icon={<Icon />}
                            toLink={`/requests?status=${status.value}`}
                        />
                    );
                })}
            </div>

            <div className={styles.requestsWrapper}>
                <h2>Latest Submitted Requests</h2>
                <RequestsPanel defaultFilters={{ pageSize: 8 }} />
            </div>
        </div>
    );
};