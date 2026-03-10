export function formatDate(dateString, showTime = false) {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
        month: 'short',
        day: 'numeric',
        year: 'numeric',
        hour: showTime ? '2-digit' : undefined,
        minute: showTime ? '2-digit' : undefined
    });
};