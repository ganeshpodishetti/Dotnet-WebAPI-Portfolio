import React, { useEffect, useState } from 'react';
import { experienceService } from '../../services/api';
import { ExperienceRequestDto } from '../../types';

const AdminExperience = () => {
  const [experiences, setExperiences] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentExperience, setCurrentExperience] = useState<ExperienceRequestDto>({
    title: '',
    companyName: '',
    location: '',
    startDate: '',
    endDate: null,
    description: ''
  });
  const [editingId, setEditingId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    fetchExperiences();
  }, []);

  const fetchExperiences = async () => {
    try {
      const data = await experienceService.getAll();
      setExperiences(data);
    } catch (error) {
      console.error('Error fetching experiences:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setCurrentExperience({ ...currentExperience, [name]: value });
  };

  const resetForm = () => {
    setCurrentExperience({
      title: '',
      companyName: '',
      location: '',
      startDate: '',
      endDate: null,
      description: ''
    });
    setEditingId(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await experienceService.update(editingId, currentExperience);
      } else {
        await experienceService.create(currentExperience);
      }
      resetForm();
      setShowForm(false);
      fetchExperiences();
    } catch (error) {
      console.error('Error saving experience:', error);
    }
  };

  const handleEdit = (exp: any) => {
    setCurrentExperience({
      title: exp.title,
      companyName: exp.companyName,
      location: exp.location || '',
      startDate: exp.startDate ? exp.startDate.split('T')[0] : '',
      endDate: exp.endDate ? exp.endDate.split('T')[0] : null,
      description: exp.description || ''
    });
    setEditingId(exp.id);
    setShowForm(true);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this experience entry?')) {
      try {
        await experienceService.delete(id);
        fetchExperiences();
      } catch (error) {
        console.error('Error deleting experience:', error);
      }
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Manage Work Experience</h1>
        <button 
          className="btn btn-primary" 
          onClick={() => {
            resetForm();
            setShowForm(!showForm);
          }}
        >
          {showForm ? 'Cancel' : 'Add New Experience'}
        </button>
      </div>

      {showForm && (
        <div className="card mb-4">
          <div className="card-body">
            <h3>{editingId ? 'Edit Experience' : 'Add New Experience'}</h3>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label htmlFor="title" className="form-label">Job Title*</label>
                <input
                  type="text"
                  className="form-control"
                  id="title"
                  name="title"
                  value={currentExperience.title}
                  onChange={handleInputChange}
                  required
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="companyName" className="form-label">Company Name*</label>
                <input
                  type="text"
                  className="form-control"
                  id="companyName"
                  name="companyName"
                  value={currentExperience.companyName}
                  onChange={handleInputChange}
                  required
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="location" className="form-label">Location</label>
                <input
                  type="text"
                  className="form-control"
                  id="location"
                  name="location"
                  value={currentExperience.location}
                  onChange={handleInputChange}
                />
              </div>
              
              <div className="row mb-3">
                <div className="col">
                  <label htmlFor="startDate" className="form-label">Start Date</label>
                  <input
                    type="date"
                    className="form-control"
                    id="startDate"
                    name="startDate"
                    value={currentExperience.startDate}
                    onChange={handleInputChange}
                  />
                </div>
                <div className="col">
                  <label htmlFor="endDate" className="form-label">End Date</label>
                  <input
                    type="date"
                    className="form-control"
                    id="endDate"
                    name="endDate"
                    value={currentExperience.endDate || ''}
                    onChange={handleInputChange}
                    placeholder="Leave blank if current"
                  />
                </div>
              </div>
              
              <div className="mb-3">
                <label htmlFor="description" className="form-label">Description</label>
                <textarea
                  className="form-control"
                  id="description"
                  name="description"
                  rows={4}
                  value={currentExperience.description}
                  onChange={handleInputChange}
                ></textarea>
              </div>
              
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Update Experience' : 'Add Experience'}
              </button>
            </form>
          </div>
        </div>
      )}

      {experiences.length === 0 ? (
        <div className="alert alert-info">No experience entries found. Add your first work experience.</div>
      ) : (
        <div className="table-responsive">
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Title</th>
                <th>Company</th>
                <th>Location</th>
                <th>Period</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {experiences.map((exp) => (
                <tr key={exp.id}>
                  <td>{exp.title}</td>
                  <td>{exp.companyName}</td>
                  <td>{exp.location || '-'}</td>
                  <td>
                    {exp.startDate ? new Date(exp.startDate).getFullYear() : '-'}
                    {exp.startDate && exp.endDate ? ' - ' : ''}
                    {exp.endDate ? new Date(exp.endDate).getFullYear() : exp.startDate ? 'Present' : ''}
                  </td>
                  <td>
                    <button 
                      className="btn btn-sm btn-outline-primary me-2" 
                      onClick={() => handleEdit(exp)}
                    >
                      Edit
                    </button>
                    <button 
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(exp.id)}
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

export default AdminExperience;
