import { AuditLogsTable } from '../../components/AuditLogsTable/AuditLogsTable';
import { StatCard } from '../../components/StatCard/StatCard';
import TypesDash from '../../components/TypesDash/TypesDash';
import { useRequestStatuses, useRequestTypes } from '../../hooks/useEnums';
import { mapData } from '../../utils/mapDataToArrayOfObjects';
import { statusIconsRegistry } from '../../utils/statusIconsRegistry';
import styles from './Dashboard.module.css';

export function AdminDashboard({ data }) {
    const { data: statuses = [] } = useRequestStatuses();
    const { data: requestTypes = [] } = useRequestTypes();

    return (
        <div className={styles.dashboard}>
            <div className={styles.statCardsContainer}>
                <StatCard
                    title="Total Requests"
                    value={data?.totalRequests}
                    icon={<statusIconsRegistry.total />}
                    toLink={"/requests"}
                />

                <StatCard
                    title="Total Users"
                    value={data?.totalUsers}
                    icon={<statusIconsRegistry.users />}
                    toLink={`/users`}
                />

                <StatCard
                    title="Pending Requests"
                    value={data?.byStatus?.Pending || 0}
                    icon={<statusIconsRegistry.pending />}
                    toLink={`/requests?status=pending`}
                />
            </div>

            <div className={styles.typesContainer}>
                <TypesDash title="Request Types" data={mapData(requestTypes, data?.byType)} showTotal={false} filter={"type"} />
                <TypesDash title="Request Statuses" data={mapData(statuses, data?.byStatus)} showTotal={false} filter={"status"} />
            </div>

            <div className={styles.requestsWrapper}>
                <h2>Recent Audit Logs</h2>
                <AuditLogsTable title='Recent Audit Logs' defaultFilters={{ pageSize: 5 }} />
            </div>
        </div>
    );
}