import { NavLink } from 'react-router-dom';
import { statusIconsRegistry } from '../../utils/statusIconsRegistry';
import styles from './TypesDash.module.css';

export default function TypesDash({ data = [], title, showTotal = true, filter }) {
    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <h3>{title}</h3>
                {showTotal && <p>Total: <strong>{data.reduce((sum, { count }) => sum + count, 0)}</strong></p>}
            </div>

            <div className={styles.body}>
                {data.map(({ name, count }) => {
                    const Icon = statusIconsRegistry[name.toLowerCase()];

                    return (
                        <NavLink key={name} className={styles.typeCard} to={`/requests?${filter}=${name.toLowerCase()}`}>
                            {<Icon />}
                            <div className={styles.content}>
                                <h3>{name}</h3>
                                <p>{count}</p>
                            </div>
                        </NavLink>
                    );
                })}
            </div>
        </div>
    );
}