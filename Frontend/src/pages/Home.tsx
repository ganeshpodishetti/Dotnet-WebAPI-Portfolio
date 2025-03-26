import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { socialLinkService, userService } from '../services/api';
import { SocialLinkRequestDto, UserRequestDto } from '../types';

const Home = () => {
  const [user, setUser] = useState<UserRequestDto | null>(null);
  const [socialLinks, setSocialLinks] = useState<SocialLinkRequestDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        // Try to get user profile
        try {
          const userData = await userService.getProfile();
          setUser(userData);
        } catch (error) {
          console.log('No user profile available');
        }

        // Get social links
        try {
          const socialData = await socialLinkService.getAll();
          setSocialLinks(socialData);
        } catch (error) {
          console.log('No social links available');
        }
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="row align-items-center mb-5">
        <div className="col-md-4 text-center">
          {user?.profilePicture ? (
            <img 
              src={user.profilePicture} 
              alt="Profile" 
              className="img-fluid rounded-circle mb-3" 
              style={{ maxWidth: '200px' }} 
            />
          ) : (
            <div 
              className="bg-light rounded-circle d-inline-flex align-items-center justify-content-center mb-3" 
              style={{ width: '200px', height: '200px' }}
            >
              <span className="display-4 text-secondary">
                {user?.firstName?.charAt(0) || ''}
                {user?.lastName?.charAt(0) || ''}
              </span>
            </div>
          )}
        </div>
        <div className="col-md-8">
          <h1 className="display-4 mb-1">
            {user?.firstName || ''} {user?.lastName || 'Welcome to my Portfolio'}
          </h1>
          <h2 className="fs-4 text-secondary mb-3">{user?.headline || 'Software Developer'}</h2>
          
          {user?.bio && <p className="lead">{user.bio}</p>}
          
          <div className="mb-3">
            {user?.city && user?.country ? `${user.city}, ${user.country}` : user?.country || user?.city || ''}
          </div>
          
          <div className="d-flex gap-3 mb-3">
            {socialLinks.map((link, index) => (
              <a 
                key={index} 
                href={link.url} 
                target="_blank" 
                rel="noopener noreferrer" 
                className="btn btn-outline-dark"
              >
                {link.icon ? <i className={link.icon}></i> : link.platform}
              </a>
            ))}
          </div>
          
          <div className="d-flex gap-2">
            <Link to="/contact" className="btn btn-primary">Contact Me</Link>
            <Link to="/projects" className="btn btn-outline-primary">View Projects</Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
