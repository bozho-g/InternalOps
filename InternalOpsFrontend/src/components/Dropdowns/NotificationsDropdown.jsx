import { useNotifications } from "../../hooks/notifications/useNotifications";
import { Dropdown } from "./Dropdown";
import styles from './Dropdown.module.css';

export function NotificationsDropdown({ isOpen, onClose, triggerRef }) {
    const { data: notifications, isLoading } = useNotifications();

    return (
        <Dropdown isOpen={isOpen} onClose={onClose} className={styles.notificationsDropdown} triggerRef={triggerRef}>
            {isLoading ? (
                <div>Loading...</div>
            ) : notifications && notifications.length > 0 ? (
                notifications.map((notification) => (
                    <div key={notification.id}>{notification.message}</div>
                ))
            ) : (
                <div>No notifications.</div>
            )}
        </Dropdown>
    );
};