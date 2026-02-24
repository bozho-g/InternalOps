import axiosInstance from "../api/axiosInstance";

export function validatefiles(files) {
    const errors = [];
    const maxSize = 10 * 1024 * 1024;
    const allowed = ["application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "image/jpeg", "image/png", "image/jpg"];

    for (const file of files) {
        if (file.size > maxSize) {
            errors.push(`File "${file.name}" exceeds the maximum size of 10MB.`);
        }
        if (!allowed.includes(file.type)) {
            errors.push(`File "${file.name}" has an unsupported file type.`);
        }
    }
    return errors;
}

export function uploadAttachment(requestId, file) {
    const formData = new FormData();
    formData.append("requestId", requestId);
    formData.append("file", file);

    return axiosInstance.post("/attachments", formData);
}