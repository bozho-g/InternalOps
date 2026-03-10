import { useState } from 'react';
import { MessageSquare, Pencil, Trash2, Send } from 'lucide-react';
import { Button } from '../Button/Button';
import { useAddComment, useUpdateComment, useDeleteComment } from '../../hooks/comments/useComments';
import { useAuthStore } from '../../stores/authStore';
import { formatDate } from '../../utils/formatDate';
import styles from './Comments.module.css';

export default function Comments({ requestId, comments = [], canComment }) {
    const [newComment, setNewComment] = useState('');
    const [editingId, setEditingId] = useState(null);
    const [editContent, setEditContent] = useState('');

    const user = useAuthStore((s) => s.user);
    const addComment = useAddComment();
    const updateComment = useUpdateComment();
    const deleteComment = useDeleteComment();

    const handleSubmit = (e) => {
        e.preventDefault();
        const trimmed = newComment.trim();
        if (!trimmed) return;
        addComment.mutate(
            { requestId, content: trimmed },
            { onSuccess: () => setNewComment('') }
        );
    };

    const handleEdit = (comment) => {
        setEditingId(comment.id);
        setEditContent(comment.content);
    };

    const handleSaveEdit = (commentId) => {
        const trimmed = editContent.trim();
        if (!trimmed) return;
        updateComment.mutate(
            { commentId, content: trimmed, requestId },
            { onSuccess: () => { setEditingId(null); setEditContent(''); } }
        );
    };

    const handleDelete = (commentId) => {
        deleteComment.mutate({ commentId, requestId });
    };

    const isOwnerOrAdmin = (commentUserId) => {
        return user?.email === commentUserId || user?.role === 'admin' || user?.role === 'manager';
    };

    return (
        <div className={styles.comments}>
            <h2 className={styles.title}><MessageSquare size={16} /> Comments ({comments.length})</h2>

            {comments.length > 0 ? (
                <div className={styles.list}>
                    {comments.map(comment => (
                        <div key={comment.id} className={styles.comment}>
                            <div className={styles.commentHeader}>
                                <span className={styles.author}>{comment.user?.email ?? 'Unknown'}</span>
                                <span className={styles.date}>{formatDate(comment.createdAt, true)}</span>
                            </div>

                            {editingId === comment.id ? (
                                <div className={styles.editRow}>
                                    <textarea
                                        className={styles.editInput}
                                        value={editContent}
                                        onChange={(e) => setEditContent(e.target.value)}
                                        rows={2}
                                    />
                                    <div className={styles.editActions}>
                                        <Button onClick={() => handleSaveEdit(comment.id)} disabled={updateComment.isPending}>
                                            Save
                                        </Button>
                                        <Button variant="secondary" onClick={() => setEditingId(null)}>Cancel</Button>
                                    </div>
                                </div>
                            ) : (
                                <p className={styles.body}>{comment.content}</p>
                            )}

                            {editingId !== comment.id && (
                                <div className={styles.actions}>
                                    {user?.email === comment.user?.email && (
                                        <button className={styles.actionBtn} onClick={() => handleEdit(comment)}>
                                            <Pencil size={14} />
                                        </button>
                                    )}
                                    {isOwnerOrAdmin(comment.user?.email) && (
                                        <button
                                            className={`${styles.actionBtn} ${styles.deleteBtn}`}
                                            onClick={() => handleDelete(comment.id)}
                                            disabled={deleteComment.isPending}
                                        >
                                            <Trash2 size={14} />
                                        </button>
                                    )}
                                </div>
                            )}
                        </div>
                    ))}
                </div>
            ) : (
                <p className={styles.empty}>No comments yet.</p>
            )}

            {canComment && (
                <form className={styles.form} onSubmit={handleSubmit}>
                    <textarea
                        className={styles.input}
                        value={newComment}
                        onChange={(e) => setNewComment(e.target.value)}
                        placeholder="Write a comment..."
                        rows={2}
                    />
                    <Button type="submit" disabled={!newComment.trim() || addComment.isPending}>
                        <Send size={14} />
                        {addComment.isPending ? 'Posting...' : 'Post'}
                    </Button>
                </form>
            )}
        </div>
    );
}
