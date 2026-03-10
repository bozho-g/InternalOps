import { useDeleteRequest } from '../../hooks/requests/useDeleteRequest';
import { useModalStore } from '../../stores/modalStore';
import { Modal } from '../modals/Modal/Modal';
import { Button } from '../Button/Button';
import styles from './Modal/Modal.module.css';
import { useNavigate } from 'react-router-dom';

export function DeleteRequestModal({ request }) {
    const closeModal = useModalStore((s) => s.closeModal);
    const { mutateAsync: deleteAsync, isPending: isDeletePending } = useDeleteRequest();
    const navigate = useNavigate();

    async function handleDelete() {
        await deleteAsync(request.id);
        closeModal();
        navigate('/requests');
    }

    return (
        <Modal title={`Delete Request "${request.title}"?`} isOpen onClose={closeModal}>
            <div className={styles.actions}>
                <Button disabled={isDeletePending} onClick={handleDelete}>Confirm</Button>
                <Button variant='secondary' onClick={closeModal}>
                    Cancel
                </Button>
            </div>
        </Modal>
    );
}