# InternalOps

A full-stack internal request management system built with ASP\.NET Core and React. Employees can submit requests (equipment, leave, tasks), managers review and action them, and admins have full visibility and control over the entire system.

**[Live Demo](https://agreeable-forest-016df0b03.2.azurestaticapps.net/)**

---

## Features

### Role-Based Access (Employee / Manager / Admin)
- **Employees** - Create, edit, and delete their own requests (only if Pending). Upload file attachments. Track request status.
- **Managers** - Review all pending requests, approve, reject, or mark as complete. Add comments on any request.
- **Admins** - Full access: manage users, promote/demote managers, view audit logs, restore soft-deleted requests.

### Requests
- Create requests with title, description, and type (Equipment / Leave / Task / BUG)
- PATCH-based partial updates using JSON Patch
- Soft delete with restore capability (Admin only)
- Status flow: `Pending → Approved → Completed` or `Pending → Rejected`
- Filtering by status, type, and search term with pagination

### File Attachments
- Upload attachments per request (PDF, JPEG, PNG, DOC/DOCX)
- File signature validation (magic bytes) to prevent MIME spoofing
- Files stored in **Azure Blob Storage**, metadata in SQL
- Attachments deleted from Azure when request is deleted

### Real-Time Notifications (SignalR)
- Managers notified instantly when a new request is submitted
- Requesters notified when their request is approved, rejected, or completed
- Requesters notified when a comment is added to their request
- Notifications persisted to DB + delivered live via WebSocket

### Audit Logs
- Every action tracked: created, updated, approved, rejected, completed, deleted, restored, comment added/removed/updated, file uploaded/removed
- Filterable by action type, date range, and user
- Paginated table for admin review

### Dashboards
- **Employee:** Personal request stats (Pending/Approved/Rejected counts) + recent requests
- **Manager:** Pending review count, approved today, breakdown by type, pending requests list
- **Admin:** Total requests/users, deleted count, breakdown by status and type, recent activity feed

### Auth
- JWT access tokens (30 min expiry) + hashed refresh tokens (7 days)
- Automatic token refresh via Axios interceptor
- Background service cleans up expired refresh tokens
---

## Tech Stack

### Backend
- ASP\.NET Core 10
- Entity Framework Core 10
- ASP\.NET Core Identity
- SignalR
- Azure SQL Database
- Azure Blob Storage
- JWT Bearer Auth
- Mapperly
- FileSignatures

### Frontend
- React 19
- React Router
- TanStack Query
- Axios
- Zustand
- SignalR JS Client
- Sonner
- Lucide React

### Infrastructure
- Azure App Service
- Azure Static Web Apps
- Azure SQL Database 
- Azure Blob Storage

---

## Architecture

```
InternalOps/
├── InternalOpsAPI/
│   └── API/
│       ├── Controllers/        # Auth, Requests, Comments, Attachments,
│       │                       # Notifications, Dashboard, AuditLogs, Users
│       ├── Services/           # Business logic layer (interfaces + implementations)
│       ├── Models/             # EF Core entities
│       ├── DTOs/               # Request/response shapes
│       ├── Mappers/            # Mapperly source-generated mappers
│       ├── Hubs/               # SignalR NotificationHub
│       ├── Data/               # AppDbContext
│       ├── Dependencies/       # DI registration (Application, Identity, Infrastructure, Persistence)
│       ├── Exceptions/         # Custom exceptions + global handler
│       └── Migrations/         # EF Core migrations
│
└── InternalOpsFrontend/
    └── src/
        ├── pages/              # Dashboard, Requests, AuditLogs, Users, Auth
        ├── components/         # RequestsPanel, RequestDetail, Navbar, Dropdowns,
        │                       # AuditLogsTable, UsersPanel, ModalRoot...
        ├── hooks/              # useSignalRNotifications, useDashboard,
        │                       # useNotifications, useDebounce, auth hooks...
        ├── stores/             # authStore, notificationStore, modalStore (Zustand)
        ├── api/                # Axios instance with interceptors
        └── utils/              # formatDate, etc.
```

### Key Design Decisions

**Service Layer Pattern** - Controllers are thin (extract user ID, call service, return result). All business logic, authorization checks, and audit logging live in services injected via DI.

**JSON Patch for Updates** - Requests use `PATCH` with `JsonPatchDocument` allowing partial updates without sending the full object.

**Soft Delete** - Requests are never hard deleted by default (`IsDeleted` flag). Admins can restore them. Attachments are hard deleted from Azure on soft delete to save storage.

**File Signature Validation** - Beyond MIME type checking, the `FileSignatures` library reads magic bytes to prevent content-type spoofing.

**JWT + Hashed Refresh Tokens** - Access tokens are short-lived (30 min). Refresh tokens are hashed with SHA-256 before storage so raw tokens are never persisted.

**SignalR Groups** - Each connected user is added to a group keyed by their user ID. Targeted notifications use `hubContext.Clients.User(userId)` which maps to the group.
