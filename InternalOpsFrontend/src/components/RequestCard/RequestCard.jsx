import { Clock, CheckCircle, XCircle, Package } from 'lucide-react';
import styles from './RequestCard.module.css';
import { Button } from '../shared/Button/Button';
import { formatDate } from '../../utils/formatDate';
import { NavLink } from 'react-router-dom';

const statusConfig = {
    "Pending": { label: 'Pending', icon: Clock, color: 'pending' },
    "Approved": { label: 'Approved', icon: CheckCircle, color: 'approved' },
    "Rejected": { label: 'Rejected', icon: XCircle, color: 'rejected' },
    "Completed": { label: 'Completed', icon: Package, color: 'completed' }
};

export function RequestCard({ request, onApprove, onReject, showActions = false }) {
    const statusInfo = statusConfig[request.status];
    const StatusIcon = statusInfo.icon;

    return (
        <NavLink className={styles.requestCard} to={`/requests/${request.id}`}>
            <div className={styles.cardHeader}>
                <div className={styles.statusBadge} data-status={statusInfo.color}>
                    <StatusIcon size={16} />
                    <span>{statusInfo.label}</span>
                </div>
                <div className={styles.requestMeta}>
                    <span className={styles.requestType}>{request.requestType}</span>
                    <span className={styles.requestId}>#{request.id}</span>
                </div>
            </div>

            <div className={styles.cardBody}>
                <h3 className={styles.requestTitle}>{request.title || 'Untitled Request'}</h3>
                <p className={styles.requestDescription}>
                    {request.description || 'No description provided'}
                </p>

                <div className={styles.requestInfo}>
                    <span>Requested by: <strong>{request.requestedBy.email || 'Unknown'}</strong></span>
                    <span className={styles.requestDate}>{formatDate(request.createdAt)}</span>
                </div>
            </div>

            {showActions && request.status === "Pending" && (
                <div className={styles.cardActions}>
                    <Button onClick={() => onApprove?.(request.id)} variant="secondary">
                        Approve
                    </Button>
                    <Button onClick={() => onReject?.(request.id)} variant="destructive">
                        Reject
                    </Button>
                </div>
            )}
        </NavLink>
    );
}