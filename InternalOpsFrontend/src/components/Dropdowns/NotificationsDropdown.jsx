import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useNotifications, useMarkAsRead, useMarkAllAsRead } from "../../hooks/notifications/useNotifications";
import { Dropdown } from "./Dropdown";
import { formatDate } from "../../utils/formatDate";
import { CheckCheck, X } from "lucide-react";
import styles from './Dropdown.module.css';

export function NotificationsDropdown({ isOpen, onClose, triggerRef }) {
    const { data: notifications = [], isLoading } = useNotifications();
    const markAsRead = useMarkAsRead();
    const markAllAsRead = useMarkAllAsRead();
    const navigate = useNavigate();
    const [tab, setTab] = useState("unread");

    const unread = notifications.filter(n => !n.isRead);
    const read = notifications.filter(n => n.isRead);
    const list = tab === "unread" ? unread : read;

    const handleClick = (notification) => {
        if (!notification.isRead) {
            markAsRead.mutate(notification.id);
        }
        if (notification.relatedRequestId) {
            navigate(`/requests/${notification.relatedRequestId}`);
        }
        onClose();
    };

    const handleMarkAllRead = () => {
        if (unread.length > 0) {
            markAllAsRead.mutate();
        }
    };

    return (
        <Dropdown isOpen={isOpen} onClose={onClose} className={styles.notificationsDropdown} triggerRef={triggerRef}>
            <div className={styles.notifHeader}>
                <h3 className={styles.notifTitle}>Notifications</h3>
                <div className={styles.notifActions}>
                    {unread.length > 0 && (
                        <button
                            className={styles.markAllBtn}
                            onClick={handleMarkAllRead}
                            disabled={markAllAsRead.isPending}
                        >
                            <CheckCheck size={14} />
                            Mark all read
                        </button>
                    )}
                    <button
                        className={styles.closeBtn}
                        onClick={onClose}
                    >
                        <X size={18} />
                    </button>
                </div>
            </div>

            <div className={styles.notifTabs}>
                <button
                    className={`${styles.notifTab} ${tab === "unread" ? styles.activeTab : ""}`}
                    onClick={() => setTab("unread")}
                >
                    Unread {unread.length > 0 && <span className={styles.tabBadge}>{unread.length}</span>}
                </button>
                <button
                    className={`${styles.notifTab} ${tab === "read" ? styles.activeTab : ""}`}
                    onClick={() => setTab("read")}
                >
                    Read
                </button>
            </div>

            <div className={styles.notifList}>
                {isLoading ? (
                    <div className={styles.notifEmpty}>Loading...</div>
                ) : list.length > 0 ? (
                    list.map((n) => (
                        <button
                            key={n.id}
                            className={`${styles.notifItem}`}
                            onClick={() => handleClick(n)}
                        >
                            <span className={styles.notifMessage}>{n.message}</span>
                            <span className={styles.notifDate}>{formatDate(n.createdAt, true)}</span>
                        </button>
                    ))
                ) : (
                    <div className={styles.notifEmpty}>
                        {tab === "unread" ? "All caught up!" : "No read notifications."}
                    </div>
                )}
            </div>
        </Dropdown>
    );
}