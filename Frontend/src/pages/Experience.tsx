import React, { useEffect, useState } from 'react';
import { experienceService } from '../services/api';
import { ExperienceRequestDto } from '../types';

const Experience = () => {
  const [experiences, setExperiences] = useState<ExperienceRequestDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchExperiences = async () => {
      try {
        const data = await experienceService.getAll();
        setExperiences(data);
      } catch (error) {
        console.error('Error fetching experience data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchExperiences();
  }, []);

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <h2 className="mb-4">Professional Experience</h2>
      
      {experiences.length === 0 && (
        <div className="alert alert-info">No experience information available.</div>
      )}

      <div className="timeline">
        {experiences.map((exp, index) => (
          <div key={index} className="card mb-4">
            <div className="card-body">
              <h3 className="card-title">{exp.title}</h3>
              <h4>{exp.companyName}</h4>
              {exp.location && <div>{exp.location}</div>}
              
              <div className="text-muted mb-2">
                {exp.startDate && new Date(exp.startDate).getFullYear()}
                {exp.startDate && exp.endDate ? ' - ' : ''}
                {exp.endDate ? new Date(exp.endDate).getFullYear() : exp.startDate ? 'Present' : ''}
              </div>
              
              {exp.description && (
                <p className="card-text">{exp.description}</p>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Experience;
