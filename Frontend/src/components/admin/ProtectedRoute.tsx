import React, { useEffect, useState } from 'react';
import { Navigate } from 'react-router-dom';
import { isAuthenticated } from '../../services/authUtils';

interface ProtectedRouteProps {
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  const [authChecked, setAuthChecked] = useState(false);
  const [isAuthed, setIsAuthed] = useState(false);

  useEffect(() => {
    const checkAuth = async () => {
      const authed = await isAuthenticated();
      setIsAuthed(authed);
      setAuthChecked(true);
    };
    
    checkAuth();
  }, []);

  if (!authChecked) {
    return <div className="container mt-5 text-center"><div className="spinner-border"></div></div>;
  }

  if (!isAuthed) {
    return <Navigate to="/admin/login" />;
  }

  return <>{children}</>;
};

export default ProtectedRoute;
