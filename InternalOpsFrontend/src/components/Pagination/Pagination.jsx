import styles from './Pagination.module.css';
import filterStyles from '../FilterBar/FilterBar.module.css';
import { ChevronsRight, ChevronsLeft, ChevronLeft, ChevronRight } from 'lucide-react';

const ALLOWED_PAGE_SIZES = [10, 20, 30, 40, 50];

export default function Pagination({ pageNumber, pageSize, totalPages, hasNextPage, hasPreviousPage, updateFilter }) {
    return (
        <div className={styles.pagination}>
            <div className={styles.pageSizeContainer}>
                <label htmlFor="pageSizeSelect">
                    Rows per page
                </label>
                <select name='pageSizeSelect' id="pageSizeSelect" value={pageSize} onChange={(e) => {
                    updateFilter('pageSize', e.target.value);
                }} className={filterStyles.filterSelect}>
                    {ALLOWED_PAGE_SIZES.map(size => (
                        <option key={size} value={size}>{size}</option>
                    ))}
                </select>
            </div>

            <div className={styles.pageInfo}>
                Page {pageNumber} of {totalPages}
            </div>

            <div className={styles.pageButtons}>
                <button
                    onClick={() => updateFilter("pageNumber", 1)}
                    disabled={pageNumber === 1}
                    className={styles.pageButton}
                >
                    <ChevronsLeft />
                </button>

                <button
                    onClick={() => hasPreviousPage && updateFilter("pageNumber", pageNumber - 1)}
                    disabled={!hasPreviousPage}
                    className={styles.pageButton}
                >
                    <ChevronLeft />
                </button>

                <button
                    onClick={() => hasNextPage && updateFilter("pageNumber", pageNumber + 1)}
                    disabled={!hasNextPage}
                    className={styles.pageButton}
                >
                    <ChevronRight />
                </button>

                <button
                    onClick={() => updateFilter("pageNumber", totalPages)}
                    disabled={pageNumber === totalPages}
                    className={styles.pageButton}
                >
                    <ChevronsRight />
                </button>
            </div>
        </div>
    );
}