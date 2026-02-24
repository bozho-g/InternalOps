import { NavLink } from 'react-router-dom';
import styles from './Navbar.module.css';
import { Bell, CircleUserRound, Plus } from 'lucide-react';
import { UserDropdown } from '../Dropdowns/UserDropdown';
import { useRef, useState } from 'react';
import { NotificationsDropdown } from '../Dropdowns/NotificationsDropdown';
import { Button } from '../shared/Button/Button';
import { useAuthStore } from '../../stores/authStore';
import { NewRequestModal } from '../NewRequestModal/NewRequestModal';

export function Navbar() {
    const { user } = useAuthStore();
    const [isUserDropdownOpen, setIsUserDropdownOpen] = useState(false);
    const [isNotificationsOpen, setIsNotificationsOpen] = useState(false);
    const [isNewRequestOpen, setIsNewRequestOpen] = useState(false);
    const userIconRef = useRef(null);
    const notifIconRef = useRef(null);

    return (
        <nav className={styles.navbar}>
            <img src="/logo-32x32.png" alt="Site Logo" />
            <div className={styles.links}>
                <NavLink to="/" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>Dashboard</NavLink>
                {user?.role === 'admin' &&
                    <>
                        <NavLink to="/admin" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>Admin</NavLink>
                        <NavLink to="/users" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>Users</NavLink>
                    </>
                }

                <NavLink to="/requests" className={({ isActive }) => isActive ? styles.activeLink : styles.link}>{user?.role === 'admin' || user?.role === 'manager' ? 'All' : 'My'} Requests</NavLink>
            </div>

            <div className={styles.actions}>
                <Button onClick={() => setIsNewRequestOpen(true)}><Plus />New Request</Button>

                <div className={styles.iconWrapper}>
                    <Bell className={styles.bellIcon} onClick={() => setIsNotificationsOpen(prev => !prev)} ref={notifIconRef} />
                    <NotificationsDropdown isOpen={isNotificationsOpen} onClose={() => setIsNotificationsOpen(false)} triggerRef={notifIconRef} />
                </div>

                <div className={styles.iconWrapper}>
                    <CircleUserRound className={styles.userIcon} onClick={() => setIsUserDropdownOpen(prev => !prev)} ref={userIconRef} />
                    <UserDropdown isOpen={isUserDropdownOpen} onClose={() => setIsUserDropdownOpen(false)} triggerRef={userIconRef} />
                </div>
            </div>
            <NewRequestModal isOpen={isNewRequestOpen} onClose={() => setIsNewRequestOpen(false)} />
        </nav>
    );
}