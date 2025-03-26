import React, { useEffect, useState } from 'react';
import { skillService } from '../../services/api';
import { SkillRequestDto } from '../../types';

const AdminSkills = () => {
  const [skills, setSkills] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentSkill, setCurrentSkill] = useState<SkillRequestDto>({
    skillCategory: '',
    skillsTypes: []
  });
  const [editingId, setEditingId] = useState<string | null>(null);
  const [showForm, setShowForm] = useState(false);
  const [skillTypeInput, setSkillTypeInput] = useState('');

  useEffect(() => {
    fetchSkills();
  }, []);

  const fetchSkills = async () => {
    try {
      const data = await skillService.getAll();
      setSkills(data);
    } catch (error) {
      console.error('Error fetching skills:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setCurrentSkill({ ...currentSkill, [name]: value });
  };

  const handleSkillKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && skillTypeInput.trim()) {
      e.preventDefault();
      setCurrentSkill({
        ...currentSkill,
        skillsTypes: [...currentSkill.skillsTypes, skillTypeInput.trim()]
      });
      setSkillTypeInput('');
    }
  };

  const removeSkillType = (index: number) => {
    const skillsTypes = [...currentSkill.skillsTypes];
    skillsTypes.splice(index, 1);
    setCurrentSkill({ ...currentSkill, skillsTypes });
  };

  const resetForm = () => {
    setCurrentSkill({
      skillCategory: '',
      skillsTypes: []
    });
    setEditingId(null);
    setSkillTypeInput('');
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingId) {
        await skillService.update(editingId, currentSkill);
      } else {
        await skillService.create(currentSkill);
      }
      resetForm();
      setShowForm(false);
      fetchSkills();
    } catch (error) {
      console.error('Error saving skill:', error);
    }
  };

  const handleEdit = (skill: any) => {
    setCurrentSkill({
      skillCategory: skill.skillCategory,
      skillsTypes: skill.skillsTypes || []
    });
    setEditingId(skill.id);
    setShowForm(true);
  };

  const handleDelete = async (id: string) => {
    if (window.confirm('Are you sure you want to delete this skill category?')) {
      try {
        await skillService.delete(id);
        fetchSkills();
      } catch (error) {
        console.error('Error deleting skill:', error);
      }
    }
  };

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <div className="d-flex justify-content-between align-items-center mb-4">
        <h1>Manage Skills</h1>
        <button 
          className="btn btn-primary" 
          onClick={() => {
            resetForm();
            setShowForm(!showForm);
          }}
        >
          {showForm ? 'Cancel' : 'Add New Skill Category'}
        </button>
      </div>

      {showForm && (
        <div className="card mb-4">
          <div className="card-body">
            <h3>{editingId ? 'Edit Skill Category' : 'Add New Skill Category'}</h3>
            <form onSubmit={handleSubmit}>
              <div className="mb-3">
                <label htmlFor="skillCategory" className="form-label">Skill Category*</label>
                <input
                  type="text"
                  className="form-control"
                  id="skillCategory"
                  name="skillCategory"
                  value={currentSkill.skillCategory}
                  onChange={handleInputChange}
                  placeholder="e.g., Programming Languages, Frameworks, Tools, etc."
                  required
                />
              </div>
              
              <div className="mb-3">
                <label className="form-label">Skills (Press Enter to add)</label>
                <input
                  type="text"
                  className="form-control"
                  value={skillTypeInput}
                  onChange={(e) => setSkillTypeInput(e.target.value)}
                  onKeyDown={handleSkillKeyDown}
                  placeholder="e.g., JavaScript, React, Docker, etc."
                />
                
                <div className="mt-2">
                  {currentSkill.skillsTypes.map((skill, index) => (
                    <span key={index} className="badge bg-secondary me-2 mb-2">
                      {skill} 
                      <button 
                        type="button" 
                        className="btn-close btn-close-white ms-1" 
                        style={{ fontSize: '0.5rem' }} 
                        onClick={() => removeSkillType(index)}
                      ></button>
                    </span>
                  ))}
                </div>
                
                {currentSkill.skillsTypes.length === 0 && (
                  <div className="text-muted small mt-2">
                    Add at least one skill to this category
                  </div>
                )}
              </div>
              
              <button 
                type="submit" 
                className="btn btn-primary"
                disabled={currentSkill.skillsTypes.length === 0}
              >
                {editingId ? 'Update Skill Category' : 'Add Skill Category'}
              </button>
            </form>
          </div>
        </div>
      )}

      {skills.length === 0 ? (
        <div className="alert alert-info">No skills found. Add your first skill category.</div>
      ) : (
        <div className="row">
          {skills.map((skill) => (
            <div key={skill.id} className="col-md-6 mb-4">
              <div className="card">
                <div className="card-body">
                  <div className="d-flex justify-content-between">
                    <h4 className="card-title">{skill.skillCategory}</h4>
                    <div>
                      <button 
                        className="btn btn-sm btn-outline-primary me-2" 
                        onClick={() => handleEdit(skill)}
                      >
                        Edit
                      </button>
                      <button 
                        className="btn btn-sm btn-outline-danger"
                        onClick={() => handleDelete(skill.id)}
                      >
                        Delete
                      </button>
                    </div>
                  </div>
                  
                  <div className="mt-3">
                    {skill.skillsTypes.map((skillType: string, i: number) => (
                      <span key={i} className="badge bg-primary me-2 mb-2 p-2">{skillType}</span>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default AdminSkills;
