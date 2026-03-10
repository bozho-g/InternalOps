import { statusIconsRegistry } from '../utils/statusIconsRegistry';
import styles from './RequestCard/RequestCard.module.css';

export function StatusBadge({ status }) {
    const StatusIcon = statusIconsRegistry[status.toLowerCase()];

    return (
        <div className={styles.statusBadge} data-status={status.toLowerCase()}>
            <StatusIcon size={16} />
            <span>{status}</span>
        </div>
    );
}