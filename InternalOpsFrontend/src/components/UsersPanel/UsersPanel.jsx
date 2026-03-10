import { useListFilters } from '../../hooks/useListFilters';
import { useUsers } from '../../hooks/users/useUsers';
import { useUsersFilterConfig } from '../../hooks/users/useUsersFilterConfig';
import ListPanel from '../List/ListPanel';
import styles from './UsersPanel.module.css';
import UserCard from '../UserCard/UserCard';

export default function UsersPanel({ showFilterBar = false, showPagination = false, defaultFilters = {} }) {
    const filterConfig = useUsersFilterConfig(defaultFilters);
    const { filters, inputValues, updateFilter } = useListFilters(filterConfig);

    return (
        <ListPanel
            useDataHook={useUsers}
            ListComponent={UsersList}
            showFilterBar={showFilterBar}
            showPagination={showPagination}
            filterProps={{ filters, inputValues, updateFilter, searchPlaceholder: "Search users...", config: filterConfig }}
        />
    );
}

function UsersList({ data }) {
    return (
        <div className={styles.usersPanel}>
            {!data || data.length === 0 ? (
                <div className={styles.emptyState}>
                    <p>No users found</p>
                </div>
            ) : (
                <div className={styles.usersList}>
                    {data.map((user) => (
                        <UserCard key={user.id} user={user} />
                    ))}
                </div>
            )}
        </div>
    );
}