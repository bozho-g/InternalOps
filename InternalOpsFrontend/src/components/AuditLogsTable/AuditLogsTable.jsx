import { useAuditLogsFilterConfig } from "../../hooks/auditLogs/useAuditLogsFilterConfig";
import { useAuditLogs } from "../../hooks/auditLogs/useAuditLogs";
import { useListFilters } from "../../hooks/useListFilters";
import ListPanel from "../List/ListPanel";
import TableRow from "../TableRow/TableRow";

import styles from './AuditLogsTable.module.css';

export function AuditLogsTable({ showFilterBar = false, showPagination = false, defaultFilters = {} }) {
    const filterConfig = useAuditLogsFilterConfig(defaultFilters);
    const { filters, inputValues, updateFilter } = useListFilters(filterConfig);

    return (
        <ListPanel
            useDataHook={useAuditLogs}
            ListComponent={AuditsTable}
            showFilterBar={showFilterBar}
            showPagination={showPagination}
            filterProps={{ filters, inputValues, updateFilter, searchPlaceholder: "Search audit logs...", config: filterConfig }}
        />
    );
}

function AuditsTable({ data }) {
    return (
        <div className={styles.container}>
            <table className={styles.table}>
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Request ID</th>
                        <th>Action</th>
                        <th>Summary</th>
                        <th>Changed By</th>
                        <th>Timestamp</th>
                    </tr>
                </thead>
                <tbody>
                    {!data || data.length === 0 ? (
                        <tr>
                            <td colSpan="6" className={styles.noData}>No audit logs available</td>
                        </tr>
                    ) : (
                        data.map(log => (
                            <TableRow log={log} key={log.id} />
                        ))
                    )}
                </tbody>
            </table>
        </div>
    );
}