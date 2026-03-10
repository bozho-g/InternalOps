export function useUsersFilterConfig(defaultFilters) {
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
            key: 'role',
            type: 'select',
            label: 'Roles',
            defaultValue: defaultFilters.role || '',
            options: [
                { value: 'admin', label: 'Admin' },
                { value: 'manager', label: 'Manager' },
                { value: 'user', label: 'User' }
            ],
            group: "basic"
        },
        {
            key: 'desc',
            type: 'checkbox',
            label: 'Order By Requests Count Desc',
            defaultValue: false,
            group: "basic"
        },
    ];
};