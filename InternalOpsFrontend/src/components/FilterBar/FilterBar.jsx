import { Search, ChevronDown, ChevronUp } from 'lucide-react';
import styles from './FilterBar.module.css';
import { useState } from 'react';
import { Button } from '../Button/Button';
import { useAuthStore } from '../../stores/authStore';

export function FilterBar({ filters, updateFilter, searchPlaceholder, config }) {
    const [showAdvanced, setShowAdvanced] = useState(false);

    const { user } = useAuthStore();
    const isAdminOrManager = ["admin", "manager"].includes(user?.role);

    const basicFilters = config.filter(f => f.group !== "advanced");
    const advancedFilters = config.filter(f => f.group === "advanced");

    function renderFilter(filter) {
        switch (filter.type) {
            case "select":
                return (
                    <select
                        key={filter.key}
                        value={filters[filter.key]}
                        onChange={(e) => updateFilter(filter.key, e.target.value)}
                        className={styles.filterSelect}
                    >
                        <option value="">All {filter.label}</option>
                        {filter.options?.map((option, index) => (
                            <option key={index} value={option.value}>{option.label}</option>
                        ))}
                    </select>
                );
            case "checkbox":
                return (
                    <label key={filter.key} className={styles.checkbox}>
                        <input
                            type="checkbox"
                            checked={filters[filter.key]}
                            onChange={(e) => updateFilter(filter.key, e.target.checked)}
                        />
                        {filter.label}
                    </label>
                );
            case "date":
                return (
                    <div key={filter.key} className={styles.dateGroup}>
                        <label>{filter.label}</label>
                        <input
                            type="date"
                            value={filters[filter.key] || ''}
                            onChange={(e) => updateFilter(filter.key, e.target.value)}
                            className={styles.dateInput}
                        />
                    </div>
                );
            case "number":
                return (
                    <div key={filter.key} className={styles.inputGroup}>
                        <label>{filter.label}</label>
                        <input
                            type="number"
                            value={filters[filter.key] || ''}
                            onChange={(e) => updateFilter(filter.key, e.target.value)}
                            className={styles.numberInput}
                            placeholder={filter.placeholder}
                            min={1}
                        />
                    </div>
                );
            default:
                return null;
        }
    }

    return (
        <div className={styles.filterBar}>
            <div className={styles.searchBox}>
                <Search size={18} />
                <input
                    type="text"
                    placeholder={searchPlaceholder}
                    value={filters.search || ''}
                    onChange={(e) => updateFilter('search', e.target.value)}
                    className={styles.searchInput}
                />
            </div>

            <div className={styles.filters}>
                {basicFilters.map(renderFilter)}

                {isAdminOrManager && advancedFilters.length > 0 && (
                    <button
                        className={styles.advancedToggle}
                        onClick={() => setShowAdvanced(!showAdvanced)}
                        type="button"
                    >
                        Advanced {showAdvanced ? <ChevronUp size={16} /> : <ChevronDown size={16} />}
                    </button>
                )}

                <Button variant='secondary' className={styles.resetFiltersBtn} onClick={() => updateFilter('reset')}>Reset Filters</Button>
            </div>

            {isAdminOrManager && showAdvanced && advancedFilters.length > 0 && (
                <div className={styles.advancedFilters}>
                    {advancedFilters.map(renderFilter)}
                </div>
            )}
        </div>
    );
}

