import { Search } from 'lucide-react';
import styles from './FilterBar.module.css';
import { useRequestTypes } from '../../hooks/requests/useRequestTypes';

export function FilterBar({ searchTerm, onSearchChange, status, onStatusChange, type, onTypeChange }) {
    const { data: requestTypes = [], isLoading } = useRequestTypes();

    return (
        <div className={styles.filterBar}>
            <div className={styles.searchBox}>
                <Search size={18} />
                <input
                    type="text"
                    placeholder="Search requests..."
                    value={searchTerm}
                    onChange={(e) => onSearchChange(e.target.value)}
                    className={styles.searchInput}
                />
            </div>

            <div className={styles.filters}>
                <select
                    value={status}
                    onChange={(e) => onStatusChange(e.target.value)}
                    className={styles.filterSelect}
                >
                    <option value="">All Status</option>
                    <option value="0">Pending</option>
                    <option value="1">Approved</option>
                    <option value="2">Rejected</option>
                    <option value="3">Completed</option>
                </select>

                <select
                    value={type}
                    onChange={(e) => onTypeChange(e.target.value)}
                    className={styles.filterSelect}
                >
                    <option value="">All Types</option>
                    {!isLoading && requestTypes.map((type, index) => (
                        <option key={index} value={type}>{type}</option>
                    ))}
                </select>
            </div>
        </div>
    );
}
