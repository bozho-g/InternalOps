import { useParams } from 'react-router-dom';
import styles from './RequestDetail.module.css';
import { useGetRequestById } from '../../hooks/requests/useGetRequestById';
import { StatusBadge } from '../StatusBadge';
import { formatDate } from '../../utils/formatDate';
import { getActionClass } from '../../utils/getActionClass';
import { Paperclip, FileText, SquarePen, X, Upload } from 'lucide-react';
import RequestActions from '../RequestActions/RequestActions';
import { useModalStore } from '../../stores/modalStore';
import { useRequestInfo } from '../../hooks/requests/useRequestInfo';
import { useRef, useState } from 'react';
import { useRequestTypes } from '../../hooks/useEnums';
import { Button } from '../Button/Button';
import { useUpdateRequest } from '../../hooks/requests/useUpdateRequest';
import { useDeleteAttachment } from '../../hooks/requests/useDeleteAttachment';
import { uploadAttachment, validatefiles } from '../../utils/attachments';
import { toast } from 'sonner';
import { useQueryClient } from '@tanstack/react-query';
import Comments from '../Comments/Comments';

export default function RequestDetail() {
    const { id } = useParams();
    const { data: request, isLoading } = useGetRequestById(id);
    const { isOwner, isPending, isAdminOrManager } = useRequestInfo(request);
    const openModal = useModalStore((s) => s.openModal);
    const { data: requestTypes, isLoading: isRequestTypesLoading } = useRequestTypes();

    const [isEditing, setIsEditing] = useState(false);
    const [form, setForm] = useState({});
    const [pendingDeletes, setPendingDeletes] = useState([]);
    const [uploading, setUploading] = useState(false);
    const fileInputRef = useRef(null);

    const updateRequest = useUpdateRequest();
    const deleteAttachment = useDeleteAttachment();
    const queryClient = useQueryClient();

    if (isLoading || isRequestTypesLoading) return <span className="loader"></span>;
    if (!request) return <p>Request not found.</p>;

    const isDeleted = request.deletedInfo?.isDeleted;
    const canEdit = isOwner && isPending && !isDeleted;
    const canComment = isAdminOrManager || isOwner;

    const startEditing = () => {
        setForm({
            title: request.title,
            description: request.description ?? '',
            requestType: request.requestType,
        });
        setPendingDeletes([]);
        setIsEditing(true);
    };

    const cancelEditing = () => {
        setForm({});
        setPendingDeletes([]);
        setIsEditing(false);
    };

    const buildPatchDoc = () => {
        const ops = [];
        if (form.title !== request.title) {
            ops.push({ op: 'replace', path: '/title', value: form.title });
        }
        const newDesc = form.description?.trim() || null;
        if (newDesc !== (request.description ?? null)) {
            ops.push({ op: 'replace', path: '/description', value: newDesc });
        }
        if (form.requestType !== request.requestType) {
            ops.push({ op: 'replace', path: '/requestType', value: form.requestType });
        }
        return ops;
    };

    const handleSave = async () => {
        if (!form.title?.trim()) {
            toast.error('Title cannot be empty');
            return;
        }

        for (const attId of pendingDeletes) {
            await deleteAttachment.mutateAsync({ attachmentId: attId, requestId: request.id });
        }

        const patchDoc = buildPatchDoc();
        if (patchDoc.length > 0) {
            updateRequest.mutate(
                { requestId: request.id, patchDoc },
                { onSuccess: () => { setIsEditing(false); setPendingDeletes([]); } }
            );
        } else {
            queryClient.invalidateQueries({ queryKey: ['request', String(request.id)] });
            setIsEditing(false);
            setPendingDeletes([]);
        }
    };

    const handleRemoveAttachment = (att) => {
        setPendingDeletes(prev => [...prev, att.id]);
    };

    const handleUndoRemove = (attId) => {
        setPendingDeletes(prev => prev.filter(id => id !== attId));
    };

    const visibleAttachments = request.attachments?.filter(att => !pendingDeletes.includes(att.id)) ?? [];
    const markedForDelete = request.attachments?.filter(att => pendingDeletes.includes(att.id)) ?? [];

    const handleFileUpload = async (e) => {
        const files = Array.from(e.target.files);
        if (!files.length) return;

        const errors = validatefiles(files);
        if (errors.length) {
            errors.forEach(err => toast.error(err));
            return;
        }

        setUploading(true);
        try {
            for (const file of files) {
                await uploadAttachment(request.id, file);
            }
            toast.success('Attachment uploaded');
            queryClient.invalidateQueries({ queryKey: ['request', String(request.id)] });
        } catch (err) {
            toast.error(err.response?.data?.detail ?? 'Upload failed');
        } finally {
            setUploading(false);
            if (fileInputRef.current) fileInputRef.current.value = '';
        }
    };

    return (
        <div className={styles.page}>
            <div className={styles.content}>
                <div className={`${styles.editableArea} ${isEditing ? styles.editing : ''}`}>
                    <div className={styles.header}>
                        <div className={styles.headerLeft}>
                            <div className={styles.titleContainer}>
                                {isEditing ? (
                                    <input
                                        type="text"
                                        className={styles.editInput}
                                        value={form.title}
                                        onChange={(e) => setForm(f => ({ ...f, title: e.target.value }))}
                                    />
                                ) : (
                                    <h1 className={styles.title}>{request.title}</h1>
                                )}
                                {canEdit && !isEditing && (
                                    <SquarePen onClick={startEditing} />
                                )}
                            </div>
                            <div className={styles.meta}>
                                <span className={styles.id}>#{request.id}</span>
                                {isEditing ? (
                                    <select
                                        className={styles.editSelect}
                                        value={form.requestType.toLowerCase()}
                                        defaultChecked={form.requestType}
                                        onChange={(e) => setForm(f => ({ ...f, requestType: e.target.value }))}
                                    >
                                        {requestTypes.map((type, i) => (
                                            <option key={i} value={type.value}>{type.label}</option>
                                        ))}
                                    </select>
                                ) : (
                                    <span className={styles.type}>{request.requestType}</span>
                                )}
                                <StatusBadge status={isDeleted ? 'Deleted' : request.status} />
                                {!isEditing && (
                                    <RequestActions request={request} isDeleted={isDeleted} onDelete={() => openModal('DELETE_REQUEST', { request })} />
                                )}
                            </div>
                        </div>
                        <span className={styles.date}>{formatDate(request.createdAt)}</span>
                    </div>

                    <div className={styles.section}>
                        {isEditing ? (
                            <textarea
                                className={styles.editTextarea}
                                value={form.description}
                                onChange={(e) => setForm(f => ({ ...f, description: e.target.value }))}
                            />
                        ) : (
                            <p className={styles.description}>{request.description || 'No description provided.'}</p>
                        )}
                    </div>

                    <div className={styles.infoRow}>
                        <div className={styles.infoItem}>
                            <span className={styles.label}>Requested by</span>
                            <span>{request.requestedBy?.email ?? '-'}</span>
                        </div>
                        <div className={styles.infoItem}>
                            <span className={styles.label}>Handled by</span>
                            <span>{request.handledBy?.email ?? '-'}</span>
                        </div>
                        <div className={styles.infoItem}>
                            <span className={styles.label}>Last updated</span>
                            <span>{formatDate(request.updatedAt, true)}</span>
                        </div>
                    </div>

                    <div className={styles.section}>
                        <h2 className={styles.sectionTitle}><Paperclip size={16} /> Attachments</h2>
                        {visibleAttachments.length > 0 ? (
                            <div className={styles.attachments}>
                                {visibleAttachments.map(att => (
                                    <div key={att.id} className={styles.attachmentContainer}>
                                        <a href={att.fileUrl} target="_blank" rel="noopener noreferrer" className={styles.attachment}>
                                            <FileText size={16} />
                                            <span>{att.fileName}</span>
                                            <span className={styles.fileSize}>{(att.fileSize / 1024).toFixed(1)} KB</span>
                                        </a>
                                        {isEditing && (
                                            <button
                                                className={styles.deleteAttachment}
                                                onClick={() => handleRemoveAttachment(att)}
                                            >
                                                <X size={18} />
                                            </button>
                                        )}
                                    </div>
                                ))}
                            </div>
                        ) : (
                            <p className={styles.description}>No attachments provided.</p>
                        )}

                        {isEditing && markedForDelete.length > 0 && (
                            <div className={styles.pendingDeletes}>
                                <span className={styles.pendingLabel}>Will be removed on save:</span>
                                {markedForDelete.map(att => (
                                    <div key={att.id} className={styles.pendingItem}>
                                        <span>{att.fileName}</span>
                                        <button className={styles.undoBtn} onClick={() => handleUndoRemove(att.id)}>Undo</button>
                                    </div>
                                ))}
                            </div>
                        )}

                        {isEditing && (
                            <div className={styles.uploadArea}>
                                <input
                                    type="file"
                                    ref={fileInputRef}
                                    onChange={handleFileUpload}
                                    hidden
                                    multiple
                                    accept=".pdf,.doc,.docx,.jpg,.jpeg,.png"
                                />
                                <Button
                                    variant="outline"
                                    onClick={() => fileInputRef.current?.click()}
                                    disabled={uploading}
                                >
                                    <Upload size={16} />
                                    {uploading ? 'Uploading...' : 'Add attachment'}
                                </Button>
                            </div>
                        )}
                    </div>

                    {isEditing && (
                        <div className={styles.editButtons}>
                            <Button onClick={handleSave} disabled={updateRequest.isPending || deleteAttachment.isPending}>
                                {(updateRequest.isPending || deleteAttachment.isPending) ? 'Saving...' : 'Save changes'}
                            </Button>
                            <Button variant="secondary" onClick={cancelEditing}>Cancel</Button>
                        </div>
                    )}
                </div>

                <Comments
                    requestId={request.id}
                    comments={request.comments}
                    canComment={canComment}
                />
            </div>

            <div className={styles.side}>
                {isDeleted && (
                    <div className={`${styles.deleted} ${styles.section}`}>
                        <div className={styles.logEntry}>
                            <span>Deleted by {request.deletedInfo?.deletedBy?.email ?? '-'}</span>
                            <span className={styles.logDate}>{formatDate(request.deletedInfo?.deletedAt, true)}</span>
                        </div>
                    </div>
                )}

                {request.auditLogs?.length > 0 && (
                    <div className={styles.logs}>
                        <h2 className={styles.sectionTitle}>Activity</h2>
                        <div className={styles.timeline}>
                            {request.auditLogs.map(log => (
                                <div key={log.id} className={styles.logEntry}>
                                    <span className={`${styles.logAction} ${styles[getActionClass(log.action)]}`}>{log.action}</span>
                                    <span className={styles.logSummary}>{log.summary}</span>
                                    <span className={styles.logDate}>{formatDate(log.timestamp, true)}</span>
                                </div>
                            ))}
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}