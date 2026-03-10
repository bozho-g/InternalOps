import { useRequestStatuses, useRequestTypes } from "../useEnums";
import { useUsers } from "../users/useUsers";

export const useRequestsFilterConfig = (defaultFilters) => {
    const { data: requestTypes = [] } = useRequestTypes();
    const { data: requestStatuses = [] } = useRequestStatuses();
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
            key: 'status',
            type: 'select',
            label: 'Statuses',
            defaultValue: defaultFilters.status || '',
            options: requestStatuses,
            group: "basic"
        },
        {
            key: 'type',
            type: 'select',
            label: 'Types',
            defaultValue: defaultFilters.type || '',
            options: requestTypes,
            group: "basic"
        },
        {
            key: 'userId',
            type: 'select',
            label: 'Users',
            defaultValue: defaultFilters.userId || '',
            options: usersOptions,
            group: "advanced"
        },
        {
            key: 'createdFrom',
            type: 'date',
            label: 'Created From',
            defaultValue: defaultFilters.createdFrom || '',
            group: "advanced"
        },
        {
            key: 'createdTo',
            type: 'date',
            label: 'Created To',
            defaultValue: defaultFilters.createdTo || '',
            group: "advanced"
        },
        {
            key: 'handledFrom',
            type: 'date',
            label: 'Handled From',
            defaultValue: defaultFilters.handledFrom || '',
            group: "advanced"
        },
        {
            key: 'handledTo',
            type: 'date',
            label: 'Handled To',
            defaultValue: defaultFilters.handledTo || '',
            group: "advanced"
        },
        {
            key: 'includeDeleted',
            type: 'checkbox',
            label: 'Include Deleted',
            defaultValue: false,
            group: "advanced"
        },
    ];
};