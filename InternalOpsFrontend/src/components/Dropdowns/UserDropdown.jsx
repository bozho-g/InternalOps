import { useAuthStore } from "../../stores/authStore";
import { useModalStore } from "../../stores/modalStore";
import { Dropdown } from "./Dropdown";
import { useLogout } from '../../hooks/auth/useLogout';
import styles from './Dropdown.module.css';

export function UserDropdown({ isOpen, onClose, triggerRef }) {
    const { user } = useAuthStore();
    const logout = useLogout();

    const openModal = useModalStore((s) => s.openModal);

    return (
        <Dropdown isOpen={isOpen} onClose={onClose} className={styles.userDropdown} triggerRef={triggerRef}>
            <div className={styles.userHeader}>
                <span className={styles.userEmail}>{user?.email}</span>
            </div>
            <button className={styles.dropdownItem} onClick={() => { onClose(); openModal('CHANGE_PASSWORD'); }}>Change Password</button>
            <button className={`${styles.dropdownItem} ${styles.destructiveItem}`} onClick={logout}>Logout</button>
        </Dropdown >
    );
}