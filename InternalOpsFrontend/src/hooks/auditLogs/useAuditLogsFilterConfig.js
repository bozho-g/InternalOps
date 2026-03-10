import { useAuditActions } from "../useEnums";
import { useUsers } from "../users/useUsers";

export const useAuditLogsFilterConfig = (defaultFilters) => {
    const { data: actions = [] } = useAuditActions();
    const { data: users = [] } = useUsers();

    const usersOptions = users?.data?.map(u => ({
        value: u.id,
        label: u.email
    }));

    return [
        {
            key: 'pageNumber',
            type: 'pagination',
            defaultValue: defaultFilters.pageNumber || 1,
        },
        {
            key: 'pageSize',
            type: 'pagination',
            defaultValue: defaultFilters.pageSize || 10,
        },
        {
            key: 'search',
            type: 'text',
            label: 'Search',
            defaultValue: defaultFilters.search || '',
            debounced: true
        },
        {
            key: 'requestId',
            type: 'number',
            label: 'Request ID',
            defaultValue: defaultFilters.requestId || '',
            debounced: true
        },
        {
            key: 'action',
            type: 'select',
            label: 'Actions',
            defaultValue: defaultFilters.action || '',
            options: actions,
        },
        {
            key: 'userId',
            type: 'select',
            label: 'Users',
            defaultValue: defaultFilters.userId || '',
            options: usersOptions,
        },
        {
            key: 'fromDate',
            type: 'date',
            label: 'From',
            defaultValue: defaultFilters.fromDate || '',
        },
        {
            key: 'toDate',
            type: 'date',
            label: 'To',
            defaultValue: defaultFilters.toDate || '',
        },
    ];
};