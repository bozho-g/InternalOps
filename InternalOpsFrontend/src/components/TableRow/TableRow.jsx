import { NavLink } from 'react-router-dom';
import { formatDate } from '../../utils/formatDate';
import { getActionClass } from '../../utils/getActionClass';
import styles from './TableRow.module.css';
import { useState } from 'react';

export default function TableRow({ log }) {
    const [expanded, setExpanded] = useState(false);

    const hasStatusChange = log.oldStatus || log.newStatus;
    const hasValueChange = log.oldValue || log.newValue;
    const hasChanges = hasStatusChange || hasValueChange;

    return (
        <>
            <tr key={log.id} onClick={() => hasChanges && setExpanded(prev => !prev)} className={hasChanges ? styles.expand : ""}>
                <td>{log.id}</td>
                <td className={styles.requestId}><NavLink onClick={(e) => e.stopPropagation()} className={styles.link} to={`/requests/${log.requestId}`}>{log.requestId}</NavLink></td>
                <td>
                    <span className={`${styles.badge} ${styles[getActionClass(log.action)]}`} >
                        {log.action}
                    </span>
                </td>
                <td>{log.summary}</td>
                <td>
                    <div><NavLink onClick={(e) => e.stopPropagation()} className={styles.link} to={`/users?search=${log.changedBy.email}`}>{log.changedBy.email}</NavLink></div>
                    <div className={styles.userId}>
                        {log.changedBy.id}
                    </div>
                </td>
                <td className={styles.timestamp}>{formatDate(log.timestamp, true)}</td>
            </tr>

            {expanded && (
                <tr className={styles.expandedRow}>
                    <td colSpan="6">
                        <div className={styles.changesBox}>
                            {hasStatusChange && (
                                <div>
                                    <strong>Status Change:</strong>
                                    <span className={styles.old}>
                                        {log.oldStatus ?? "—"}
                                    </span>
                                    →
                                    <span className={styles.new}>
                                        {log.newStatus ?? "—"}
                                    </span>
                                </div>
                            )}

                            {hasValueChange && (
                                <div>
                                    <strong>Value Change:</strong>
                                    <span className={styles.old}>
                                        {log.oldValue ?? "—"}
                                    </span>
                                    →
                                    <span className={styles.new}>
                                        {log.newValue ?? "—"}
                                    </span>
                                </div>
                            )}
                        </div>
                    </td>
                </tr>
            )}
        </>
    );
}