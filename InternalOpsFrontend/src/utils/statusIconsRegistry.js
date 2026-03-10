import { ClipboardCheck, XCircle, Clock, CheckCheck, LockIcon, Bug, Hammer, LayoutList, House, Hash, Users } from 'lucide-react';

export const statusIconsRegistry = {
    "pending": Clock,
    "approved": CheckCheck,
    "rejected": XCircle,
    "completed": ClipboardCheck,
    "deleted": LockIcon,
    "bug": Bug,
    "equipment": Hammer,
    "task": LayoutList,
    "leave": House,
    "total": Hash,
    "users": Users
};