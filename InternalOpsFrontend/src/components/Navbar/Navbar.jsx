import { useLogoutMutation } from '../../hooks/auth/useLogoutMutation';
import styles from './Navbar.module.css';
import { useAuthStore } from "@/stores/authStore";

export function Navbar() {
    const { user } = useAuthStore();
    const logoutMutation = useLogoutMutation();

    return (
        <nav className={styles.navbar}>
            <div>
                {user.email}
            </div>
            <div className={styles.links}>
                {/* <NavLink to="/" className={({ isActive }) => isActive ? styles.activeLink : styles.link}><svg viewBox="0 0 16 16" aria-hidden="true" focusable="false"><use href={`${iconsUrl}#icon-home`} /></svg>
                    Home</NavLink>

                <NavLink to="/users" className={({ isActive }) => isActive ? styles.activeLink : styles.link}><svg viewBox="0 0 16 16" aria-hidden="true" focusable="false"><use href={`${iconsUrl}#icon-search`} /></svg>Users</NavLink>

                <NavLink to={`/profile/${user?.username}`} className={({ isActive }) => isActive ? styles.activeLink : styles.link}><svg viewBox="0 0 20 20" aria-hidden="true" focusable="false"><use href={`${iconsUrl}#icon-profile`} /></svg>Profile</NavLink> */}

                <a href="/logout" onClick={(e) => { e.preventDefault(); logoutMutation.mutate(); }} className={styles.link}>Logout</a>
            </div>
        </nav>
    );
}