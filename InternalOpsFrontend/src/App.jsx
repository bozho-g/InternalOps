import { Route, Routes } from 'react-router-dom';
import './App.css';
import { ProtectedRoute } from './components/ProtectedRoute/ProtectedRoute';
import { Dashboard } from './pages/Dashboards/Dashboard';
import { Toaster } from 'sonner';
import { Navbar } from './components/Navbar/Navbar';
import { useAuthStore } from './stores/authStore';
import { PageOutlet } from './pages/PageOutlet/PageOutlet';
import { RequestsPanel } from './components/RequestsPanel/RequestsPanel';
import { NotFound } from './components/NotFound/NotFound';
import { AuditLogsTable } from './components/AuditLogsTable/AuditLogsTable';
import UsersPanel from './components/UsersPanel/UsersPanel';
import RequestDetail from './components/RequestDetail/RequestDetail';
import { ModalRoot } from './components/ModalRoot';
import { useSignalRNotifications } from './hooks/notifications/useSignalRNotifications';

function App() {
  const { user } = useAuthStore();

  useSignalRNotifications();

  return (
    <>
      <Toaster position="top-right" richColors closeButton toastOptions={{
        style: {
          background: "var(--background)",
          color: "var(--foreground)"
        }
      }} />
      <ModalRoot />

      {user && <Navbar />}

      <Routes>
        <Route path='/' element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
        <Route path='/requests' element={<ProtectedRoute><PageOutlet /></ProtectedRoute>} >
          <Route index element={<RequestsPanel showFilterBar={true} showPagination={true} />} />
          <Route path=":id" element={<RequestDetail />} />
        </Route>

        <Route path="/audit-logs" element={<ProtectedRoute allowedRoles={["admin"]}><PageOutlet /></ProtectedRoute>} >
          <Route index element={<AuditLogsTable showFilterBar={true} showPagination={true} />} />
        </Route>

        <Route path="/users" element={<ProtectedRoute allowedRoles={["admin"]}><PageOutlet /></ProtectedRoute>} >
          <Route index element={<UsersPanel showFilterBar={true} showPagination={true} />} />
        </Route>

        <Route path='*' element={<NotFound />} />
      </Routes>
    </>
  );
}

export default App;
