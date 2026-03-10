import { useState } from "react";
import { Button } from "../Button/Button";
import { Modal } from "./Modal/Modal";
import styles from "./Modal/Modal.module.css";
import { toast } from "sonner";
import { uploadAttachment, validatefiles } from "../../utils/attachments";
import { useCreateRequest } from "../../hooks/requests/useCreateRequest";
import { useNavigate } from "react-router-dom";
import { useRequestTypes } from "../../hooks/useEnums";
import { useModalStore } from "../../stores/modalStore";

export function NewRequestModal() {
    const [errors, setErrors] = useState([]);
    const { data: requestTypes = [], isLoading } = useRequestTypes();
    const { mutateAsync: createRequest, isPending } = useCreateRequest();
    let navigate = useNavigate();
    const closeModal = useModalStore((s) => s.closeModal);

    function handleClose() {
        setErrors([]);
        closeModal();
    }

    async function handleSubmit(e) {
        e.preventDefault();

        const formData = new FormData(e.target);
        const title = formData.get("title");
        const description = formData.get("description");
        const requestType = formData.get("type");
        const attachments = Array.from(e.target.attachments.files);

        const newErrors = [];
        const fileErrors = validatefiles(attachments);

        if (fileErrors.length > 0) {
            newErrors.push(...fileErrors);
        }

        if (!title || title.length < 5 || title.length > 100) {
            newErrors.push("Title must be between 5 and 100 characters long.");
        }

        if (description && description.length > 500) {
            newErrors.push("Description must be at most 500 characters long.");
        }

        if (!requestType || !requestTypes.some(type => type.value === requestType)) {
            newErrors.push("Please select a valid request type.");
        }

        setErrors(newErrors);
        if (newErrors.length > 0) {
            return;
        }

        const payload = {
            title,
            description,
            requestType
        };

        try {
            const { data: request } = await createRequest(payload);

            if (attachments.length > 0) {
                await Promise.all(
                    attachments.map(file =>
                        uploadAttachment(request.id, file)
                    )
                );
            }

            toast.success("Request created successfully!");
            handleClose();
            navigate(`/requests/${request.id}`);
        } catch (err) {
            if (err.response?.data?.detail) {
                setErrors([err.response.data.detail]);
            } else {
                setErrors([err.message || "Something went wrong."]);
            }
        }
    }

    return (
        <Modal title="New Request" isOpen onClose={handleClose}>
            <form onSubmit={handleSubmit}>
                <div className="inputBox">
                    <label htmlFor="title">Request Title *</label>
                    <input type="text" name="title" id="title" required />
                </div>

                <div className="inputBox">
                    <label htmlFor="description">Request Description</label>
                    <textarea className={styles.description} name="description" id="description"></textarea>
                </div>

                <div className="inputBox">
                    <label htmlFor="type">Request Type</label>
                    <select name="type" id="type" required>
                        <option value="">Select a type</option>
                        {isLoading && <option disabled>Loading...</option>}
                        {!isLoading && requestTypes.map((type, index) => (
                            <option key={index} value={type.value}>{type.label}</option>
                        ))}
                    </select>
                </div>

                <div className="inputBox">
                    <label htmlFor="attachments">Attachments</label>
                    <input type="file" name="attachments" id="attachments" accept=".pdf,.doc,.docx,.jpg,.jpeg,.png" multiple />
                </div>

                <Button disabled={isLoading || isPending} type="submit">{isLoading ? "Loading..." : isPending ? "Creating..." : "Create Request"}</Button>
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
