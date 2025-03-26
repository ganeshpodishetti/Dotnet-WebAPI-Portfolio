import React, { useEffect, useState } from 'react';
import { educationService } from '../../services/api';
import { EducationRequestDto } from '../../types';

const AdminEducation = () => {
  const [education, setEducation] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentEducation, setCurrentEducation] = useState<EducationRequestDto>({
    school: '',
    degree: '',
    location: '',
    fieldOfStudy: '',
    startDate: '',
    endDate: null,
    description: ''
  });
  const [editingId, setEditingId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);

  useEffect(() => {
    fetchEducation();
  }, []);

  const fetchEducation = async () => {
    try {
      const data = await educationService.getAll();
      setEducation(data);
    } catch (error) {
      console.error('Error fetching education:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setCurrentEducation({ ...currentEducation, [name]: value });
  };

  const resetForm = () => {
    setCurrentEducation({
      school: '',
      degree: '',
      location: '',
      fieldOfStudy: '',
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
        await educationService.update(editingId, currentEducation);
      } else {
        await educationService.create(currentEducation);
      }
      resetForm();
      setShowForm(false);
      fetchEducation();
    } catch (error) {
      console.error('Error saving education:', error);
    }
  };

  const handleEdit = (edu: any) => {
    setCurrentEducation({
      school: edu.school,
      degree: edu.degree || '',
      location: edu.location || '',
      fieldOfStudy: edu.fieldOfStudy || '',
      startDate: edu.startDate ? edu.startDate.split('T')[0] : '',  // Format date for input
      endDate: edu.endDate ? edu.endDate.split('T')[0] : null,
      description: edu.description || ''
    });
    setEditingId(edu.id);
    setShowForm(true);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this education entry?')) {
      try {
        await educationService.delete(id);
        fetchEducation();
      } catch (error) {
        console.error('Error deleting education:', error);
      }
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Manage Education</h1>
        <button 
          className="btn btn-primary" 
          onClick={() => {
            resetForm();
            setShowForm(!showForm);
          }}
        >
          {showForm ? 'Cancel' : 'Add New Education'}
        </button>
      </div>

      {showForm && (
        <div className="card mb-4">
          <div className="card-body">
            <h3>{editingId ? 'Edit Education' : 'Add New Education'}</h3>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label htmlFor="school" className="form-label">School/University*</label>
                <input
                  type="text"
                  className="form-control"
                  id="school"
                  name="school"
                  value={currentEducation.school}
                  onChange={handleInputChange}
                  required
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="degree" className="form-label">Degree</label>
                <input
                  type="text"
                  className="form-control"
                  id="degree"
                  name="degree"
                  value={currentEducation.degree}
                  onChange={handleInputChange}
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="fieldOfStudy" className="form-label">Field of Study</label>
                <input
                  type="text"
                  className="form-control"
                  id="fieldOfStudy"
                  name="fieldOfStudy"
                  value={currentEducation.fieldOfStudy}
                  onChange={handleInputChange}
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="location" className="form-label">Location</label>
                <input
                  type="text"
                  className="form-control"
                  id="location"
                  name="location"
                  value={currentEducation.location}
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
                    value={currentEducation.startDate}
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
                    value={currentEducation.endDate || ''}
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
                  rows={3}
                  value={currentEducation.description}
                  onChange={handleInputChange}
                ></textarea>
              </div>
              
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Update Education' : 'Add Education'}
              </button>
            </form>
          </div>
        </div>
      )}

      {education.length === 0 ? (
        <div className="alert alert-info">No education entries found. Add your first education entry.</div>
      ) : (
        <div className="table-responsive">
          <table className="table table-hover">
            <thead>
              <tr>
                <th>School</th>
                <th>Degree</th>
                <th>Field of Study</th>
                <th>Period</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {education.map((edu) => (
                <tr key={edu.id}>
                  <td>{edu.school}</td>
                  <td>{edu.degree || '-'}</td>
                  <td>{edu.fieldOfStudy || '-'}</td>
                  <td>
                    {edu.startDate ? new Date(edu.startDate).getFullYear() : '-'}
                    {edu.startDate && edu.endDate ? ' - ' : ''}
                    {edu.endDate ? new Date(edu.endDate).getFullYear() : edu.startDate ? 'Present' : ''}
                  </td>
                  <td>
                    <button 
                      className="btn btn-sm btn-outline-primary me-2" 
                      onClick={() => handleEdit(edu)}
                    >
                      Edit
                    </button>
                    <button 
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(edu.id)}
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

export default AdminEducation;
