import { Route, Routes } from 'react-router-dom';
import './App.css';
import { ProtectedRoute } from './components/ProtectedRoute/ProtectedRoute';
import { Dashboard } from './pages/Dashboard/Dashboard';
import { Toaster } from 'sonner';

function App() {
  return (
    <>
      <Toaster position="top-right" richColors closeButton toastOptions={{
        style: {
          background: "var(--background)",
          color: "var(--foreground)"
        }
      }} />

      <Routes>
        <Route path='/' element={<ProtectedRoute><Dashboard /></ProtectedRoute>} />
      </Routes>
    </>
  );
}

export default App;
