import { useRequestAction } from '../../hooks/requests/useRequestAction';
import { useRequestInfo } from '../../hooks/requests/useRequestInfo';
import { Button } from '../Button/Button';
import styles from './RequestActions.module.css';

export default function RequestActions({ request, isDeleted, onDelete }) {
    const { mutateAsync, isPending: isActionPending } = useRequestAction();
    const { isAdmin, isOwner, isPending, isApproved, isAdminOrManager } = useRequestInfo(request);

    return (
        <div className={styles.cardActions}>
            {isAdmin && isDeleted && (
                <Button variant="outline" onClick={() => mutateAsync({ requestId: request.id, action: "restore" })}>
                    Restore
                </Button>
            )}

            {isAdminOrManager && !isDeleted && isPending && (
                <>
                    <Button disabled={isActionPending} onClick={() => mutateAsync({ requestId: request.id, action: "approve" })} variant="secondary">
                        Approve
                    </Button>
                    <Button disabled={isActionPending} onClick={() => mutateAsync({ requestId: request.id, action: "reject" })} variant="destructive">
                        Reject
                    </Button>
                </>
            )}

            {isAdminOrManager && !isDeleted && isApproved && (
                <Button disabled={isActionPending} onClick={() => mutateAsync({ requestId: request.id, action: "complete" })} variant="primary">
                    Complete
                </Button>
            )}

            {(isOwner || isAdmin) && isPending && !isDeleted && (
                <>
                    <Button variant="outline" onClick={() => onDelete()}>
                        Delete
                    </Button>
                </>
            )}
        </div>
    );
}

