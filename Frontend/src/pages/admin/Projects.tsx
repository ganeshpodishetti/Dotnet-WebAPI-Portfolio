import React, { useEffect, useState } from 'react';
import { projectService } from '../../services/api';
import { ProjectRequestDto } from '../../types';

const AdminProjects = () => {
  const [projects, setProjects] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentProject, setCurrentProject] = useState<ProjectRequestDto>({
    name: '',
    description: '',
    url: '',
    githubUrl: '',
    skills: []
  });
  const [editingId, setEditingId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [skillInput, setSkillInput] = useState('');

  useEffect(() => {
    fetchProjects();
  }, []);

  const fetchProjects = async () => {
    try {
      const data = await projectService.getAll();
      setProjects(data);
    } catch (error) {
      console.error('Error fetching projects:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setCurrentProject({ ...currentProject, [name]: value });
  };

  const handleSkillKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && skillInput.trim()) {
      e.preventDefault();
      const skills = currentProject.skills || [];
      setCurrentProject({
        ...currentProject,
        skills: [...skills, skillInput.trim()]
      });
      setSkillInput('');
    }
  };

  const removeSkill = (index: number) => {
    const skills = [...(currentProject.skills || [])];
    skills.splice(index, 1);
    setCurrentProject({ ...currentProject, skills });
  };

  const resetForm = () => {
    setCurrentProject({
      name: '',
      description: '',
      url: '',
      githubUrl: '',
      skills: []
    });
    setEditingId(null);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await projectService.update(editingId, currentProject);
      } else {
        await projectService.create(currentProject);
      }
      resetForm();
      setShowForm(false);
      fetchProjects();
    } catch (error) {
      console.error('Error saving project:', error);
    }
  };

  const handleEdit = (project: any) => {
    setCurrentProject({
      name: project.name,
      description: project.description,
      url: project.url || '',
      githubUrl: project.githubUrl || '',
      skills: project.skills || []
    });
    setEditingId(project.id);
    setShowForm(true);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this project?')) {
      try {
        await projectService.delete(id);
        fetchProjects();
      } catch (error) {
        console.error('Error deleting project:', error);
      }
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Manage Projects</h1>
        <button 
          className="btn btn-primary" 
          onClick={() => {
            resetForm();
            setShowForm(!showForm);
          }}
        >
          {showForm ? 'Cancel' : 'Add New Project'}
        </button>
      </div>

      {showForm && (
        <div className="card mb-4">
          <div className="card-body">
            <h3>{editingId ? 'Edit Project' : 'Add New Project'}</h3>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label htmlFor="name" className="form-label">Project Name</label>
                <input
                  type="text"
                  className="form-control"
                  id="name"
                  name="name"
                  value={currentProject.name}
                  onChange={handleInputChange}
                  required
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="description" className="form-label">Description</label>
                <textarea
                  className="form-control"
                  id="description"
                  name="description"
                  rows={4}
                  value={currentProject.description}
                  onChange={handleInputChange}
                  required
                ></textarea>
              </div>
              
              <div className="mb-3">
                <label htmlFor="url" className="form-label">Project URL</label>
                <input
                  type="url"
                  className="form-control"
                  id="url"
                  name="url"
                  value={currentProject.url}
                  onChange={handleInputChange}
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="githubUrl" className="form-label">GitHub URL</label>
                <input
                  type="url"
                  className="form-control"
                  id="githubUrl"
                  name="githubUrl"
                  value={currentProject.githubUrl}
                  onChange={handleInputChange}
                />
              </div>
              
              <div className="mb-3">
                <label htmlFor="skills" className="form-label">Skills (Press Enter to add)</label>
                <input
                  type="text"
                  className="form-control"
                  id="skills"
                  value={skillInput}
                  onChange={(e) => setSkillInput(e.target.value)}
                  onKeyDown={handleSkillKeyDown}
                  placeholder="Add technologies used"
                />
                
                <div className="mt-2">
                  {currentProject.skills?.map((skill, index) => (
                    <span key={index} className="badge bg-secondary me-2 mb-2">
                      {skill} 
                      <button 
                        type="button" 
                        className="btn-close btn-close-white ms-1" 
                        style={{ fontSize: '0.5rem' }} 
                        onClick={() => removeSkill(index)}
                      ></button>
                    </span>
                  ))}
                </div>
              </div>
              
              <button type="submit" className="btn btn-primary">
                {editingId ? 'Update Project' : 'Add Project'}
              </button>
            </form>
          </div>
        </div>
      )}

      {projects.length === 0 ? (
        <div className="alert alert-info">No projects found. Add your first project.</div>
      ) : (
        <div className="table-responsive">
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Name</th>
                <th>Description</th>
                <th>Skills</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {projects.map((project) => (
                <tr key={project.id}>
                  <td>{project.name}</td>
                  <td>{project.description.length > 100 
                    ? `${project.description.substring(0, 100)}...` 
                    : project.description}
                  </td>
                  <td>
                    {project.skills?.slice(0, 3).map((skill: string, i: number) => (
                      <span key={i} className="badge bg-info me-1">{skill}</span>
                    ))}
                    {project.skills?.length > 3 && <span className="badge bg-secondary">+{project.skills.length - 3}</span>}
                  </td>
                  <td>
                    <button 
                      className="btn btn-sm btn-outline-primary me-2" 
                      onClick={() => handleEdit(project)}
                    >
                      Edit
                    </button>
                    <button 
                      className="btn btn-sm btn-outline-danger"
                      onClick={() => handleDelete(project.id)}
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

export default AdminProjects;
