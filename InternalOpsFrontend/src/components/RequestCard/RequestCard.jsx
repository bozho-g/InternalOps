import styles from './RequestCard.module.css';
import { formatDate } from '../../utils/formatDate';
import { NavLink } from 'react-router-dom';
import { StatusBadge } from '../StatusBadge';
import RequestActions from '../RequestActions/RequestActions';
import { useModalStore } from '../../stores/modalStore';

export function RequestCard({ request }) {
    const isDeleted = request?.deletedInfo?.isDeleted ?? false;
    const openModal = useModalStore((s) => s.openModal);

    return (
        <>
            <div className={`${styles.requestCardContainer} ${isDeleted ? styles.deleted : ''}`}>
                <NavLink className={styles.requestCard} to={`/requests/${request.id}`}>
                    <div className={styles.cardHeader}>
                        <StatusBadge status={isDeleted ? "Deleted" : request.status} />
                        <div className={styles.requestMeta}>
                            <span className={styles.requestType}>{request.requestType}</span>
                            <span className={styles.requestId}>#{request.id}</span>
                        </div>
                    </div>

                    <div className={styles.cardBody}>
                        <h3 className={`${styles.requestTitle} ${styles.cardText}`}>{request.title || 'Untitled Request'}</h3>
                        <p className={`${styles.requestDescription} ${styles.cardText}`}>
                            {request.description || 'No description provided'}
                        </p>

                        <div className={styles.requestInfo}>
                            <div className={styles.requestUsersInfo}>
                                <span>Requested by: <strong>{request.requestedBy.email || 'Unknown'}</strong></span>
                                {request.handledBy && <span>Handled by: <strong>{request?.handledBy?.email || 'Unknown'}</strong></span>}
                            </div>
                            <span className={styles.requestDate}>{formatDate(request.createdAt)}</span>
                        </div>
                    </div>

                </NavLink>
                <RequestActions request={request} isDeleted={isDeleted} onDelete={() => openModal('DELETE_REQUEST', { request })} />
            </div>
        </>
    );
};
