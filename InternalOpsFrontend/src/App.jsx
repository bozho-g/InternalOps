import { Route, Routes } from 'react-router-dom';
import './App.css';
import { ProtectedRoute } from './components/ProtectedRoute/ProtectedRoute';
import { Dashboard } from './pages/Dashboards/Dashboard';
import { Toaster } from 'sonner';
import { Navbar } from './components/Navbar/Navbar';
import { useAuthStore } from './stores/authStore';

function App() {
  const { user } = useAuthStore();

  return (
    <>
      <Toaster position="top-right" richColors closeButton toastOptions={{
        style: {
          background: "var(--background)",
          color: "var(--foreground)"
        }
      }} />

      {user && <Navbar />}

      <Routes>
        <Route path='/' element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
        {/* <Route path="/requests" element={<ProtectedRoute><RequestsList /></ProtectedRoute>} />
        <Route path='/requests/:id' element={<ProtectedRoute><RequestDetails /></ProtectedRoute>} /> */}
      </Routes>
    </>
  );
}

export default App;
