import React, { useEffect, useState } from 'react';
import { authService, userService } from '../../services/api';
import { ChangePasswordDto, UserRequestDto } from '../../types';

const AdminUserProfile = () => {
  const [userProfile, setUserProfile] = useState<UserRequestDto>({
    firstName: '',
    lastName: '',
    profilePicture: '',
    bio: '',
    headline: '',
    country: '',
    city: ''
  });
  const [changePassword, setChangePassword] = useState<ChangePasswordDto>({
    currentPassword: '',
    newPassword: ''
  });
  const [confirmPassword, setConfirmPassword] = useState('');
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState<{ type: 'success' | 'danger', text: string } | null>(null);

  useEffect(() => {
    fetchUserProfile();
  }, []);

  const fetchUserProfile = async () => {
    try {
      const data = await userService.getProfile();
      setUserProfile(data);
    } catch (error) {
      console.error('Error fetching user profile:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleProfileChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setUserProfile({ ...userProfile, [name]: value });
  };

  const handlePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setChangePassword({ ...changePassword, [name]: value });
  };

  const handleProfileSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setMessage(null);
    try {
      await userService.updateProfile(userProfile);
      setMessage({ type: 'success', text: 'Profile updated successfully!' });
    } catch (error) {
      console.error('Error updating profile:', error);
      setMessage({ type: 'danger', text: 'Failed to update profile. Please try again.' });
    }
  };

  const handlePasswordSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setMessage(null);
    
    if (changePassword.newPassword !== confirmPassword) {
      setMessage({ type: 'danger', text: 'New password and confirm password do not match.' });
      return;
    }
    
    try {
      await authService.changePassword(changePassword);
      setChangePassword({
        currentPassword: '',
        newPassword: ''
      });
      setConfirmPassword('');
      setMessage({ type: 'success', text: 'Password changed successfully!' });
    } catch (error) {
      console.error('Error changing password:', error);
      setMessage({ type: 'danger', text: 'Failed to change password. Please check your current password.' });
    }
  };

  const handleDeleteAccount = async () => {
    if (window.confirm('Are you sure you want to delete your account? This action cannot be undone.')) {
      try {
        await authService.deleteUser();
        localStorage.removeItem('token');
        window.location.href = '/';
      } catch (error) {
        console.error('Error deleting account:', error);
        setMessage({ type: 'danger', text: 'Failed to delete account. Please try again.' });
      }
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <h1 className="mb-4">Profile Settings</h1>
      
      {message && (
        <div className={`alert alert-${message.type} alert-dismissible fade show`} role="alert">
          {message.text}
          <button 
            type="button" 
            className="btn-close" 
            onClick={() => setMessage(null)}
          ></button>
        </div>
      )}
      
      <div className="row">
        <div className="col-lg-8">
          <div className="card mb-4">
            <div className="card-header">
              <h3>Personal Information</h3>
            </div>
            <div className="card-body">
              <form onSubmit={handleProfileSubmit}>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="firstName" className="form-label">First Name</label>
                    <input
                      type="text"
                      className="form-control"
                      id="firstName"
                      name="firstName"
                      value={userProfile.firstName || ''}
                      onChange={handleProfileChange}
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="lastName" className="form-label">Last Name</label>
                    <input
                      type="text"
                      className="form-control"
                      id="lastName"
                      name="lastName"
                      value={userProfile.lastName || ''}
                      onChange={handleProfileChange}
                    />
                  </div>
                </div>
                
                <div className="mb-3">
                  <label htmlFor="headline" className="form-label">Headline/Title</label>
                  <input
                    type="text"
                    className="form-control"
                    id="headline"
                    name="headline"
                    value={userProfile.headline || ''}
                    onChange={handleProfileChange}
                    placeholder="e.g., Full Stack Developer"
                  />
                </div>
                
                <div className="mb-3">
                  <label htmlFor="profilePicture" className="form-label">Profile Picture URL</label>
                  <input
                    type="url"
                    className="form-control"
                    id="profilePicture"
                    name="profilePicture"
                    value={userProfile.profilePicture || ''}
                    onChange={handleProfileChange}
                  />
                </div>
                
                <div className="mb-3">
                  <label htmlFor="bio" className="form-label">Bio</label>
                  <textarea
                    className="form-control"
                    id="bio"
                    name="bio"
                    rows={4}
                    value={userProfile.bio || ''}
                    onChange={handleProfileChange}
                  ></textarea>
                </div>
                
                <div className="row mb-3">
                  <div className="col-md-6">
                    <label htmlFor="city" className="form-label">City</label>
                    <input
                      type="text"
                      className="form-control"
                      id="city"
                      name="city"
                      value={userProfile.city || ''}
                      onChange={handleProfileChange}
                    />
                  </div>
                  <div className="col-md-6">
                    <label htmlFor="country" className="form-label">Country</label>
                    <input
                      type="text"
                      className="form-control"
                      id="country"
                      name="country"
                      value={userProfile.country || ''}
                      onChange={handleProfileChange}
                    />
                  </div>
                </div>
                
                <button type="submit" className="btn btn-primary">
                  Save Changes
                </button>
              </form>
            </div>
          </div>
          
          <div className="card mb-4">
            <div className="card-header">
              <h3>Change Password</h3>
            </div>
            <div className="card-body">
              <form onSubmit={handlePasswordSubmit}>
                <div className="mb-3">
                  <label htmlFor="currentPassword" className="form-label">Current Password</label>
                  <input
                    type="password"
                    className="form-control"
                    id="currentPassword"
                    name="currentPassword"
                    value={changePassword.currentPassword}
                    onChange={handlePasswordChange}
                    required
                  />
                </div>
                
                <div className="mb-3">
                  <label htmlFor="newPassword" className="form-label">New Password</label>
                  <input
                    type="password"
                    className="form-control"
                    id="newPassword"
                    name="newPassword"
                    value={changePassword.newPassword}
                    onChange={handlePasswordChange}
                    required
                  />
                </div>
                
                <div className="mb-3">
                  <label htmlFor="confirmPassword" className="form-label">Confirm New Password</label>
                  <input
                    type="password"
                    className="form-control"
                    id="confirmPassword"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                  />
                </div>
                
                <button type="submit" className="btn btn-warning">
                  Change Password
                </button>
              </form>
            </div>
          </div>
          
          <div className="card">
            <div className="card-header bg-danger text-white">
              <h3>Danger Zone</h3>
            </div>
            <div className="card-body">
              <h5>Delete Account</h5>
              <p>This action is permanent and cannot be undone. All your data will be permanently deleted.</p>
              <button 
                type="button" 
                className="btn btn-outline-danger"
                onClick={handleDeleteAccount}
              >
                Delete My Account
              </button>
            </div>
          </div>
        </div>
        
        <div className="col-lg-4">
          <div className="card">
            <div className="card-header">
              <h3>Profile Preview</h3>
            </div>
            <div className="card-body text-center">
              {userProfile.profilePicture ? (
                <img 
                  src={userProfile.profilePicture} 
                  alt="Profile" 
                  className="rounded-circle img-thumbnail" 
                  style={{ width: '150px', height: '150px', objectFit: 'cover' }}
                />
              ) : (
                <div 
                  className="bg-light rounded-circle mx-auto d-flex align-items-center justify-content-center mb-3" 
                  style={{ width: '150px', height: '150px' }}
                >
                  <span className="display-4 text-secondary">
                    {userProfile.firstName?.charAt(0) || ''}
                    {userProfile.lastName?.charAt(0) || ''}
                  </span>
                </div>
              )}
              <h5 className="mt-3">
                {userProfile.firstName || ''} {userProfile.lastName || ''}
              </h5>
              <p className="text-secondary">{userProfile.headline || ''}</p>
              <p className="small">
                {userProfile.city && userProfile.country 
                  ? `${userProfile.city}, ${userProfile.country}` 
                  : userProfile.city || userProfile.country || ''
                }
              </p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminUserProfile;
