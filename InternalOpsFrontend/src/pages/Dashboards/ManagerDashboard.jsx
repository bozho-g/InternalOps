import { RequestsPanel } from '../../components/RequestsPanel/RequestsPanel';
import { StatCard } from '../../components/StatCard/StatCard';
import { statusIconsRegistry } from '../../utils/statusIconsRegistry';
import styles from './Dashboard.module.css';
import { mapData } from '../../utils/mapDataToArrayOfObjects';
import TypesDash from '../../components/TypesDash/TypesDash';
import { useRequestTypes } from '../../hooks/useEnums';

export function ManagerDashboard({ data }) {
    const today = new Date().toISOString().split('T')[0];
    const { data: requestTypes = [] } = useRequestTypes();

    return (
        <div className={styles.dashboard}>
            <div className={styles.statCardsContainer}>
                <StatCard
                    title="Pending Requests"
                    value={data?.pendingCount}
                    icon={<statusIconsRegistry.pending />}
                    toLink={"/requests?status=pending"}
                />
                <StatCard
                    title="Approved Today"
                    value={data?.approvedToday}
                    icon={<statusIconsRegistry.approved />}
                    toLink={`/requests?status=approved&handledFrom=${today}`}
                />
                <TypesDash data={mapData(requestTypes, data?.byType)} title={"Requests By Type"} filter={"type"} />
            </div>
            <div className={styles.requestsWrapper}>
                <h2>Pending Requests</h2>
                <RequestsPanel defaultFilters={{ status: 'pending', pageSize: 8 }} />
            </div>
        </div>
    );
}