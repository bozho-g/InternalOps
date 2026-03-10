import { Outlet } from "react-router-dom";
import styles from './PageOutlet.module.css';

export function PageOutlet() {
    return (
        <div className={styles.wrapper}>
            <Outlet />
        </div>
    );
}