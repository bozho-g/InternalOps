import { useState } from 'react';
import { RequestCard } from '../RequestCard/RequestCard';
import { FilterBar } from '../FilterBar/FilterBar';
import styles from './RequestsPanel.module.css';
import { useRequests } from '../../hooks/requests/useRequests';
import { useDebounce } from '../../hooks/useDebounce';

export function RequestsPanel({
    showFilterBar = true,
    filters = {}
}) {
    const [typeFilter, setTypeFilter] = useState('');
    const [statusFilter, setStatusFilter] = useState('');
    const [searchTerm, setSearchTerm] = useState('');
    const debouncedSearch = useDebounce(searchTerm, 400);

    const { data: requests, isLoading } = useRequests({
        ...filters,
        status: statusFilter,
        type: typeFilter,
        search: debouncedSearch
    });

    return (
        <div className={styles.requestsPanel}>
            {showFilterBar && <FilterBar
                searchTerm={searchTerm}
                onSearchChange={setSearchTerm}
                status={statusFilter}
                onStatusChange={setStatusFilter}
                type={typeFilter}
                onTypeChange={setTypeFilter}
            />}

            {isLoading && <span className='loader'></span>}

            {!requests || requests.length === 0 ? (
                <div className={styles.emptyState}>
                    <p>No requests found</p>
                </div>
            ) : (
                <div className={styles.requestsList}>
                    {requests.map((request) => (
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