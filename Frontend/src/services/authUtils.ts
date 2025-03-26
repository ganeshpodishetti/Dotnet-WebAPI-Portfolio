/**
 * Checks if the user is authenticated by verifying the token's existence and expiration
 */
export const isAuthenticated = async (): Promise<boolean> => {
  const token = localStorage.getItem('token');
  const tokenExpiry = localStorage.getItem('tokenExpiry');
  
  if (!token || !tokenExpiry) {
    return false;
  }
  
  // Check if token has expired
  const expiryDate = new Date(tokenExpiry);
  if (expiryDate <= new Date()) {
    // Token has expired, clear it
    localStorage.removeItem('token');
    localStorage.removeItem('tokenExpiry');
    return false;
  }
  
  // For enhanced security, we could also validate the token with the server
  // But for simplicity, we'll just check its existence and expiry
  return true;
};

/**
 * Logs out the user by clearing authentication data
 */
export const logout = (): void => {
  localStorage.removeItem('token');
  localStorage.removeItem('tokenExpiry');
  
  // Redirect to login page using the correct URL
  window.location.href = '/admin/login';
};

/**
 * Refreshes token expiry time
 */
export const refreshTokenExpiry = (): void => {
  const token = localStorage.getItem('token');
  
  if (token) {
    // Set new expiry time (extend by 24 hours from now)
    const expiryTime = new Date();
    expiryTime.setHours(expiryTime.getHours() + 24);
    localStorage.setItem('tokenExpiry', expiryTime.toISOString());
  }
};
