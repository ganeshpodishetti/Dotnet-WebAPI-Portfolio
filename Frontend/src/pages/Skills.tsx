import React, { useEffect, useState } from 'react';
import { skillService } from '../services/api';
import { SkillRequestDto } from '../types';

const Skills = () => {
  const [skills, setSkills] = useState<SkillRequestDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchSkills = async () => {
      try {
        const data = await skillService.getAll();
        setSkills(data);
      } catch (error) {
        console.error('Error fetching skills data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchSkills();
  }, []);

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <h2 className="mb-4">Skills</h2>
      
      {skills.length === 0 && (
        <div className="alert alert-info">No skills information available.</div>
      )}

      <div className="row">
        {skills.map((skillCategory, index) => (
          <div key={index} className="col-md-6 mb-4">
            <div className="card">
              <div className="card-body">
                <h3 className="card-title">{skillCategory.skillCategory}</h3>
                <div className="mt-3">
                  {skillCategory.skillsTypes.map((skill, i) => (
                    <span key={i} className="badge bg-primary me-2 mb-2 p-2">{skill}</span>
                  ))}
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Skills;
