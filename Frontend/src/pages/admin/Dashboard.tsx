import React, { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { educationService, experienceService, messageService, projectService, skillService, socialLinkService } from '../../services/api';

interface CountData {
  projects: number;
  education: number;
  experience: number;
  skills: number;
  socialLinks: number;
  messages: number;
  unreadMessages: number;
}

const AdminDashboard = () => {
  const navigate = useNavigate();
  const [counts, setCounts] = useState<CountData>({
    projects: 0,
    education: 0,
    experience: 0,
    skills: 0,
    socialLinks: 0,
    messages: 0,
    unreadMessages: 0
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Check if user is logged in
    const token = localStorage.getItem('token');
    if (!token) {
      navigate('/admin/login');
      return;
    }

    const fetchCounts = async () => {
      try {
        const [projects, education, experience, skills, socialLinks, messages, unreadMessages] = await Promise.all([
          projectService.getAll(),
          educationService.getAll(),
          experienceService.getAll(),
          skillService.getAll(),
          socialLinkService.getAll(),
          messageService.getAll(),
          messageService.getUnread()
        ]);

        setCounts({
          projects: projects.length,
          education: education.length,
          experience: experience.length,
          skills: skills.length,
          socialLinks: socialLinks.length,
          messages: messages.length,
          unreadMessages: unreadMessages.length
        });
      } catch (error) {
        console.error('Error fetching counts:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchCounts();
  }, [navigate]);

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <h1 className="mb-4">Admin Dashboard</h1>
      
      <div className="row">
        <div className="col-md-4 mb-4">
          <div className="card bg-primary text-white">
            <div className="card-body">
              <h5 className="card-title">Projects</h5>
              <p className="card-text display-4">{counts.projects}</p>
              <Link to="/admin/projects" className="btn btn-light">Manage</Link>
            </div>
          </div>
        </div>
        
        <div className="col-md-4 mb-4">
          <div className="card bg-success text-white">
            <div className="card-body">
              <h5 className="card-title">Education</h5>
              <p className="card-text display-4">{counts.education}</p>
              <Link to="/admin/education" className="btn btn-light">Manage</Link>
            </div>
          </div>
        </div>
        
        <div className="col-md-4 mb-4">
          <div className="card bg-info text-white">
            <div className="card-body">
              <h5 className="card-title">Experience</h5>
              <p className="card-text display-4">{counts.experience}</p>
              <Link to="/admin/experience" className="btn btn-light">Manage</Link>
            </div>
          </div>
        </div>
        
        <div className="col-md-4 mb-4">
          <div className="card bg-warning text-dark">
            <div className="card-body">
              <h5 className="card-title">Skills</h5>
              <p className="card-text display-4">{counts.skills}</p>
              <Link to="/admin/skills" className="btn btn-dark">Manage</Link>
            </div>
          </div>
        </div>
        
        <div className="col-md-4 mb-4">
          <div className="card bg-secondary text-white">
            <div className="card-body">
              <h5 className="card-title">Social Links</h5>
              <p className="card-text display-4">{counts.socialLinks}</p>
              <Link to="/admin/sociallinks" className="btn btn-light">Manage</Link>
            </div>
          </div>
        </div>
        
        <div className="col-md-4 mb-4">
          <div className="card bg-danger text-white">
            <div className="card-body">
              <h5 className="card-title">Messages</h5>
              <p className="card-text display-4">
                {counts.messages}
                {counts.unreadMessages > 0 && (
                  <span className="ms-2 badge bg-light text-danger">{counts.unreadMessages} new</span>
                )}
              </p>
              <Link to="/admin/messages" className="btn btn-light">Manage</Link>
            </div>
          </div>
        </div>
      </div>
      
      <div className="row">
        <div className="col-12">
          <div className="card">
            <div className="card-body">
              <h5 className="card-title">Profile</h5>
              <p className="card-text">Manage your personal information and portfolio settings</p>
              <Link to="/admin/profile" className="btn btn-primary">Edit Profile</Link>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default AdminDashboard;
