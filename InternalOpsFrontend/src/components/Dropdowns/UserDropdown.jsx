import { useAuthStore } from "../../stores/authStore";
import { Dropdown } from "./Dropdown";
import { useLogout } from '../../hooks/auth/useLogout';
import styles from './Dropdown.module.css';
import { useState } from "react";
import { ChangePasswordModal } from "../ChangePasswordModal";

export function UserDropdown({ isOpen, onClose, triggerRef }) {
    const { user } = useAuthStore();
    const logout = useLogout();

    const [isChangingPassword, setIsChangingPassword] = useState(false);

    return (
        <>
            <Dropdown isOpen={isOpen} onClose={onClose} className={styles.userDropdown} triggerRef={triggerRef}>
                <div className={styles.userHeader}>
                    <span className={styles.userEmail}>{user?.email}</span>
                </div>
                <button className={styles.dropdownItem} onClick={() => setIsChangingPassword(true)}>Change Password</button>
                <button className={`${styles.dropdownItem} ${styles.destructiveItem}`} onClick={logout}>Logout</button>
            </Dropdown >
            <ChangePasswordModal isOpen={isChangingPassword} onClose={() => setIsChangingPassword(false)} />
        </>
    );
}