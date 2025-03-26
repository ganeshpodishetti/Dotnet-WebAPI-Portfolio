import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { authService } from '../../services/api';
import { isAuthenticated } from '../../services/authUtils';
import { LoginRequestDto } from '../../types';

const Login = () => {
  const [credentials, setCredentials] = useState<LoginRequestDto>({
    email: '',
    password: ''
  });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [checkingAuth, setCheckingAuth] = useState(true);
  const navigate = useNavigate();

  // Check if already logged in
  useEffect(() => {
    const checkAuth = async () => {
      if (await isAuthenticated()) {
        navigate('/admin');
      }
      setCheckingAuth(false);
    };
    
    checkAuth();
  }, [navigate]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);
    
    try {
      const response = await authService.login(credentials);
      
      if (response && response.token) {
        // Store token with expiry time (1 day)
        const expiryTime = new Date();
        expiryTime.setHours(expiryTime.getHours() + 24);
        
        localStorage.setItem('token', response.token);
        localStorage.setItem('tokenExpiry', expiryTime.toISOString());
        
        // Use navigate instead of direct URL change for proper routing
        navigate('/admin');
      } else {
        setError('Invalid response from server. Please try again.');
      }
    } catch (error: any) {
      console.error('Login failed:', error);
      setError(error.message || 'Invalid response from server. Please try again.');
    } finally {
      setLoading(false);
    }
  };
  
  if (checkingAuth) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="row justify-content-center">
        <div className="col-md-6">
          <div className="card">
            <div className="card-body">
              <h3 className="card-title text-center">Admin Login</h3>
              {error && (
                <div className="alert alert-danger" role="alert">
                  {error}
                </div>
              )}
              <form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label htmlFor="email" className="form-label">Email</label>
                  <input
                    type="email"
                    className="form-control"
                    id="email"
                    value={credentials.email}
                    onChange={(e) => setCredentials({ ...credentials, email: e.target.value })}
                    required
                    disabled={loading}
                  />
                </div>
                <div className="mb-3">
                  <label htmlFor="password" className="form-label">Password</label>
                  <input
                    type="password"
                    className="form-control"
                    id="password"
                    value={credentials.password}
                    onChange={(e) => setCredentials({ ...credentials, password: e.target.value })}
                    required
                    disabled={loading}
                  />
                </div>
                <button 
                  type="submit" 
                  className="btn btn-primary w-100" 
                  disabled={loading}
                >
                  {loading ? (
                    <>
                      <span className="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
                      Logging in...
                    </>
                  ) : 'Login'}
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
