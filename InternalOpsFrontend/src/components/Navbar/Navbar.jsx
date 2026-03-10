import { NavLink } from 'react-router-dom';
import styles from './Navbar.module.css';
import { Bell, CircleUserRound, Plus } from 'lucide-react';
import { UserDropdown } from '../Dropdowns/UserDropdown';
import { useRef, useState } from 'react';
import { NotificationsDropdown } from '../Dropdowns/NotificationsDropdown';
import { Button } from '../Button/Button';
import { useAuthStore } from '../../stores/authStore';
import { useModalStore } from '../../stores/modalStore';
import { useNotificationStore } from '../../stores/notificationStore';
import { useNotifications } from '../../hooks/notifications/useNotifications';

export function Navbar() {
    const { user } = useAuthStore();
    const [isUserDropdownOpen, setIsUserDropdownOpen] = useState(false);
    const { isOpen: isNotificationsOpen, toggle: toggleNotifications, close: closeNotifications } = useNotificationStore();
    const userIconRef = useRef(null);
    const notifIconRef = useRef(null);
    const openModal = useModalStore((s) => s.openModal);
    const { data: notifications = [] } = useNotifications();
    const unreadCount = notifications.filter(n => !n.isRead).length;

    return (
        <nav className={styles.navbar}>
            <img src="/logo-32x32.png" alt="Site Logo" />
            <div className={styles.links}>
                <NavLink to="/" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>Dashboard</NavLink>
                <NavLink to="/requests" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>{user?.role === 'admin' || user?.role === 'manager' ? 'All' : 'My'} Requests</NavLink>

                {user?.role === 'admin' &&
                    <>
                        <NavLink to="/users" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>Manage Users</NavLink>
                        <NavLink to="/audit-logs" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>Audit Logs</NavLink>
                    </>
                }
            </div>

            <div className={styles.actions}>
                <Button onClick={() => openModal('NEW_REQUEST')}><Plus />New Request</Button>

                <div className={styles.iconWrapper}>
                    <Bell className={styles.bellIcon} onClick={toggleNotifications} ref={notifIconRef} />
                    {unreadCount > 0 && <span className={styles.unreadDot} />}
                    <NotificationsDropdown isOpen={isNotificationsOpen} onClose={closeNotifications} triggerRef={notifIconRef} />
                </div>

                <div className={styles.iconWrapper}>
                    <CircleUserRound className={styles.userIcon} onClick={() => setIsUserDropdownOpen(prev => !prev)} ref={userIconRef} />
                    <UserDropdown isOpen={isUserDropdownOpen} onClose={() => setIsUserDropdownOpen(false)} triggerRef={userIconRef} />
                </div>
            </div>
        </nav>
    );
}