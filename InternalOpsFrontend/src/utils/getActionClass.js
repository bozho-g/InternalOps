export function getActionClass(action) {
    return getActionVariant(action);
};

const positiveActions = [
    "Created",
    "Restored",
    "AttachmentAdded",
    "CommentAdded"
];

const destructiveActions = [
    "Deleted",
    "AttachmentRemoved",
    "CommentRemoved"
];

const getActionVariant = (action) => {
    if (positiveActions.includes(action)) return "positive";
    if (destructiveActions.includes(action)) return "destructive";
    return "neutral";
};
