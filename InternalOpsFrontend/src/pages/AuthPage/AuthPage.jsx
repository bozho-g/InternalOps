import { useState } from "react";
import styles from './AuthPage.module.css';
import { Button } from '@/components/shared/Button/Button';
import { useNavigate } from "react-router-dom";
import { useLoginMutation } from "@/hooks/auth/useLoginMutation";
import { useRegisterMutation } from "@/hooks/auth/useRegisterMutation";

export function AuthPage({ isSigningIn = true }) {
    const [mode, setMode] = useState(isSigningIn);
    const [errors, setErrors] = useState([]);

    const loginMutation = useLoginMutation();
    const registerMutation = useRegisterMutation();

    const isLoading = loginMutation.isPending || registerMutation.isPending;

    const navigate = useNavigate();

    async function handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData(e.target);
        const email = formData.get("email")?.toString() ?? "";
        const password = formData.get("password")?.toString() ?? "";
        const confirmPassword = formData.get("confirmPassword")?.toString() ?? "";

        const errorsLocal = [];
        if (email.length < 1) {
            errorsLocal.push("Email must be at least 1 character long.");
        }

        if (password.length < 6) {
            errorsLocal.push("Password must be at least 6 characters long.");
        }

        if (!mode && password !== confirmPassword) {
            errorsLocal.push("Passwords do not match.");
        }

        setErrors(errorsLocal);
        if (errorsLocal.length > 0) {
            return;
        }

        if (mode) {
            await loginMutation.mutateAsync({ email, password });
            navigate("/");
            return;
        }

        await registerMutation.mutateAsync({ email, password, confirmPassword });
        navigate("/");
    }

    return (
        <div className={styles.authPage}>
            <h1>InternalOps</h1>

            <div className={styles.formContainer}>
                <h2 className={styles.formHeader}>{mode ? "Sign in to your account" : "Create an account"}</h2>
                <form onSubmit={handleSubmit}>
                    <div className="inputBox">
                        <label htmlFor="email">Email</label>
                        <input type="email" name="email" id="email" required placeholder="m@example.com" />
                    </div>

                    <div className="inputBox">
                        <label htmlFor="password">Password</label>
                        <input type="password" name="password" id="password" required placeholder="••••••••" />
                    </div>
                    {!mode && (
                        <>
                            <div className="inputBox">
                                <label htmlFor="confirmPassword">Confirm Password</label>
                                <input type="password" name="confirmPassword" id="confirmPassword" required />
                            </div>
                        </>
                    )}
                    {
                        errors.length > 0 && (
                            <div className="errorBox">
                                {errors.map((error, index) => (
                                    <p key={index} className={styles.errorMessage}>{error}</p>
                                ))}
                            </div>
                        )
                    }

                    <Button variant="secondary" disabled={isLoading} type="submit">{isLoading ? "Loading..." : mode ? "Sign In" : "Sign Up"}</Button>

                    <div className={styles.formFooter}>
                        {mode ? "Don't have an account? " : "Already have an account? "}
                        <a className={styles.link} onClick={() => { setMode(!mode); setErrors([]); }}>{mode ? "Sign Up" : "Sign In"}</a>
                    </div>

                    <div className={styles.divider}>
                        <span>or</span>
                    </div>

                    <div className={styles.demoLogin}>
                        <p className={styles.demoLabel}>Demo access:</p>
                        <div className={styles.demoButtons}>
                            <button type="button" onClick={() => loginMutation.mutateAsync({ email: "user@example.com", password: "123456" })}>
                                User
                            </button>
                            <button type="button" onClick={() => loginMutation.mutateAsync({ email: "manager@example.com", password: "123456" })}>
                                Manager
                            </button>
                            <button type="button" onClick={() => loginMutation.mutateAsync({ email: "admin@example.com", password: "123456" })}>
                                Admin
                            </button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );
}