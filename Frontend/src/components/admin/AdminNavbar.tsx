import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { isAuthenticated, logout } from '../../services/authUtils';

const AdminNavbar = () => {
  const [authenticated, setAuthenticated] = useState(false);
  
  useEffect(() => {
    const checkAuth = async () => {
      const auth = await isAuthenticated();
      setAuthenticated(auth);
    };
    
    checkAuth();
  }, []);
  
  if (!authenticated) {
    return null; // Don't show navbar if not authenticated
  }

  const handleLogout = () => {
    logout();
  };

  return (
    <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
      <div className="container">
        <Link className="navbar-brand" to="/admin">Admin Dashboard</Link>
        <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#adminNavbar">
          <span className="navbar-toggler-icon"></span>
        </button>
        <div className="collapse navbar-collapse" id="adminNavbar">
          <ul className="navbar-nav me-auto">
            <li className="nav-item">
              <Link className="nav-link" to="/admin/projects">Projects</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/admin/education">Education</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/admin/experience">Experience</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/admin/skills">Skills</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/admin/sociallinks">Social Links</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/admin/messages">Messages</Link>
            </li>
            <li className="nav-item">
              <Link className="nav-link" to="/admin/profile">Profile</Link>
            </li>
          </ul>
          <ul className="navbar-nav">
            <li className="nav-item">
              <Link className="nav-link" to="/" target="_blank">View Site</Link>
            </li>
            <li className="nav-item">
              <button className="btn btn-outline-light" onClick={handleLogout}>Logout</button>
            </li>
          </ul>
        </div>
      </div>
    </nav>
  );
};

export default AdminNavbar;
