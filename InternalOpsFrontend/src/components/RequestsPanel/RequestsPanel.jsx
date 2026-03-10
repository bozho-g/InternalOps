import { RequestCard } from '../RequestCard/RequestCard';
import styles from './RequestsPanel.module.css';
import { useRequests } from '../../hooks/requests/useRequests';
import { useListFilters } from '../../hooks/useListFilters';
import { useRequestsFilterConfig } from '../../hooks/requests/useRequestsFilterConfig';
import ListPanel from '../List/ListPanel';

export function RequestsPanel({ showFilterBar = false, showPagination = false, defaultFilters = {} }) {
    const filterConfig = useRequestsFilterConfig(defaultFilters);
    const { filters, inputValues, updateFilter } = useListFilters(filterConfig);

    return (
        <ListPanel
            useDataHook={useRequests}
            ListComponent={RequestsList}
            showFilterBar={showFilterBar}
            showPagination={showPagination}
            filterProps={{ filters, inputValues, updateFilter, searchPlaceholder: "Search requests...", config: filterConfig }}
        />
    );
}

function RequestsList({ data }) {
    return (
        <div className={styles.requestsPanel}>
            {!data || data.length === 0 ? (
                <div className={styles.emptyState}>
                    <p>No requests found</p>
                </div>
            ) : (
                <div className={styles.requestsList}>
                    {data.map((request) => (
                        <RequestCard
                            key={request.id}
                            request={request}
                        />
                    ))}
                </div>
            )}
        </div>
    );
}