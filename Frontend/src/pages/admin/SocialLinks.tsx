import React, { useEffect, useState } from 'react';
import { socialLinkService } from '../../services/api';
import { SocialLinkRequestDto } from '../../types';

const AdminSocialLinks = () => {
  const [socialLinks, setSocialLinks] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentLink, setCurrentLink] = useState<SocialLinkRequestDto>({
    platform: '',
    url: '',
    icon: ''
  });
  const [editingId, setEditingId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);

  const platformSuggestions = [
    { name: 'LinkedIn', icon: 'bi bi-linkedin' },
    { name: 'GitHub', icon: 'bi bi-github' },
    { name: 'Twitter', icon: 'bi bi-twitter' },
    { name: 'Facebook', icon: 'bi bi-facebook' },
    { name: 'Instagram', icon: 'bi bi-instagram' },
    { name: 'YouTube', icon: 'bi bi-youtube' },
    { name: 'Medium', icon: 'bi bi-medium' },
    { name: 'Slack', icon: 'bi bi-slack' },
    { name: 'Stack Overflow', icon: 'bi bi-stack-overflow' }
  ];

  useEffect(() => {
    fetchSocialLinks();
  }, []);

  const fetchSocialLinks = async () => {
    try {
      const data = await socialLinkService.getAll();
      setSocialLinks(data);
    } catch (error) {
      console.error('Error fetching social links:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    
    if (name === 'platform' && !editingId) {
      // Auto-select icon when platform is selected
      const platform = platformSuggestions.find(p => p.name === value);
      if (platform) {
        setCurrentLink({ 
          ...currentLink, 
          platform: value,
          icon: platform.icon
        });
      } else {
        setCurrentLink({ ...currentLink, platform: value });
      }
    } else {
      setCurrentLink({ ...currentLink, [name]: value });
    }
  };

  const resetForm = () => {
    setCurrentLink({
      platform: '',
      url: '',
      icon: ''
    });
    setEditingId(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await socialLinkService.update(editingId, currentLink);
      } else {
        await socialLinkService.create(currentLink);
      }
      resetForm();
      setShowForm(false);
      fetchSocialLinks();
    } catch (error) {
      console.error('Error saving social link:', error);
    }
  };

  const handleEdit = (link: any) => {
    setCurrentLink({
      platform: link.platform,
      url: link.url,
      icon: link.icon || ''
    });
    setEditingId(link.id);
    setShowForm(true);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this social link?')) {
      try {
        await socialLinkService.delete(id);
        fetchSocialLinks();
      } catch (error) {
        console.error('Error deleting social link:', error);
      }
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Manage Social Links</h1>
        <button 
          className="btn btn-primary" 
          onClick={() => {
            resetForm();
            setShowForm(!showForm);
          }}
        >
          {showForm ? 'Cancel' : 'Add New Social Link'}
        </button>
      </div>

      {showForm && (
        <div className="card mb-4">
          <div className="card-body">
            <h3>{editingId ? 'Edit Social Link' : 'Add New Social Link'}</h3>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label htmlFor="platform" className="form-label">Platform*</label>
                <select
                  className="form-select"
                  id="platform"
                  name="platform"
                  value={currentLink.platform}
                  onChange={handleInputChange}
                  required
                >
                  <option value="">Select Platform</option>
                  {platformSuggestions.map((platform) => (
                    <option key={platform.name} value={platform.name}>
                      {platform.name}
                    </option>
                  ))}
                  <option value="Other">Other</option>
                </select>
              </div>
              
              {currentLink.platform === 'Other' && (
                <div className="mb-3">
                  <label htmlFor="customPlatform" className="form-label">Custom Platform Name*</label>
                  <input
                    type="text"
                    className="form-control"
                    id="customPlatform"
                    name="platform"
                    value={currentLink.platform === 'Other' ? '' : currentLink.platform}
                    onChange={(e) => setCurrentLink({ ...currentLink, platform: e.target.value })}
                    required
                  />
                </div>
              )}
              
              <div className="mb-3">
                <label htmlFor="url" className="form-label">URL*</label>
                <input
                  type="url"
                  className="form-control"
                  id="url"
                  name="url"
                  value={currentLink.url}
                  onChange={handleInputChange}
                  placeholder="https://example.com/yourprofile"
                  required
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="icon" className="form-label">Icon (Optional)</label>
                <input
                  type="text"
                  className="form-control"
                  id="icon"
                  name="icon"
                  value={currentLink.icon || ''}
                  onChange={handleInputChange}
                  placeholder="e.g., bi bi-linkedin"
                />
                <small className="form-text text-muted">
                  Use Bootstrap Icons or other icon libraries. The site must include the necessary CSS for these icons.
                </small>
              </div>
              
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Update Social Link' : 'Add Social Link'}
              </button>
            </form>
          </div>
        </div>
      )}

      {socialLinks.length === 0 ? (
        <div className="alert alert-info">No social links found. Add your first social media profile.</div>
      ) : (
        <div className="table-responsive">
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Platform</th>
                <th>URL</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {socialLinks.map((link) => (
                <tr key={link.id}>
                  <td>
                    {link.icon && <i className={link.icon + " me-2"}></i>}
                    {link.platform}
                  </td>
                  <td>
                    <a href={link.url} target="_blank" rel="noopener noreferrer">
                      {link.url}
                    </a>
                  </td>
                  <td>
                    <button 
                      className="btn btn-sm btn-outline-primary me-2" 
                      onClick={() => handleEdit(link)}
                    >
                      Edit
                    </button>
                    <button 
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(link.id)}
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
};

export default AdminSocialLinks;
