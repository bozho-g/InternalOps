import { useState } from "react";
import { Button } from "./shared/Button/Button";
import { Modal } from "./shared/Modal/Modal";
import { useChangePasswordMutation } from "../hooks/auth/useChangePasswordMutation";
import { toast } from "sonner";

export function ChangePasswordModal({ isOpen, onClose }) {
    const [errors, setErrors] = useState([]);
    const { mutateAsync: changePassword, isPending } = useChangePasswordMutation();

    function handleClose() {
        setErrors([]);
        onClose();
    }

    async function handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData(e.target);
        const currentPassword = formData.get("currentPassword");
        const newPassword = formData.get("newPassword");
        const confirmPassword = formData.get("confirmPassword");

        const errorsLocal = [];
        if (newPassword.length < 6) {
            errorsLocal.push("New password must be at least 6 characters long.");
        }

        if (newPassword !== confirmPassword) {
            errorsLocal.push("New passwords do not match.");
        }

        setErrors(errorsLocal);
        if (errorsLocal.length > 0) {
            return;
        }

        const payload = {
            currentPassword,
            newPassword,
            confirmPassword
        };

        try {
            await changePassword(payload);

            handleClose();
            toast.success("Password changed successfully!");
        } catch (err) {
            if (err.response?.data?.detail) {
                setErrors([err.response.data.detail]);
            } else {
                setErrors([err.message || "Something went wrong."]);
            }
        }
    }

    return (
        <Modal title="Change Password" isOpen={isOpen} onClose={handleClose}>
            <form onSubmit={handleSubmit}>
                <div className="inputBox">
                    <label htmlFor="currentPassword">Current Password</label>
                    <input type="password" name="currentPassword" id="currentPassword" autoComplete="current-password" required />
                </div>

                <div className="inputBox">
                    <label htmlFor="newPassword">New Password</label>
                    <input type="password" name="newPassword" id="newPassword" autoComplete="new-password" required />
                </div>

                <div className="inputBox">
                    <label htmlFor="confirmPassword">Confirm New Password</label>
                    <input type="password" name="confirmPassword" id="confirmPassword" autoComplete="new-password" required />
                </div>

                <Button disabled={isPending} type="submit">{isPending ? "Changing..." : "Change Password"}</Button>
            </form>

            {errors.length > 0 && (
                <div className="errorBox">
                    {errors.map((error, index) => (
                        <p key={index}>{error}</p>
                    ))}
                </div>
            )}
        </Modal>
    );
}
