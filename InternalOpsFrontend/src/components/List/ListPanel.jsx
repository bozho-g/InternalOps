import { toast } from 'sonner';
import { FilterBar } from '../FilterBar/FilterBar';
import Pagination from '../Pagination/Pagination';

export default function ListPanel({ useDataHook, ListComponent, showFilterBar, filterProps, showPagination = true }) {
    const { data, isLoading, error } = useDataHook(filterProps?.filters);

    if (isLoading) return <span className='loader'></span>;

    if (error) {
        toast.error(error?.response?.data?.detail || 'Failed to load data');
    }

    const { data: paginationData, ...pagination } = data;

    return (
        <div>
            {showFilterBar && filterProps && (
                <FilterBar {...filterProps} filters={filterProps.inputValues} />
            )}

            {ListComponent && <ListComponent data={paginationData} />}

            {showPagination && <Pagination {...pagination} updateFilter={filterProps?.updateFilter} />}
        </div>
    );
}