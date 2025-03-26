import React, { useEffect, useState } from 'react';
import { educationService } from '../services/api';
import { EducationRequestDto } from '../types';

const Education = () => {
  const [education, setEducation] = useState<EducationRequestDto[]>([]);
  const [loading, setLoading] = useState<boolean>(true);

  useEffect(() => {
    const fetchEducation = async () => {
      try {
        const data = await educationService.getAll();
        setEducation(data);
      } catch (error) {
        console.error('Error fetching education data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchEducation();
  }, []);

  if (loading) {
    return <div className="text-center mt-5"><div className="spinner-border"></div></div>;
  }

  return (
    <div className="container">
      <h2 className="mb-4">Education</h2>
      
      {education.length === 0 && (
        <div className="alert alert-info">No education information available.</div>
      )}

      <div className="timeline">
        {education.map((edu, index) => (
          <div key={index} className="card mb-4">
            <div className="card-body">
              <h3 className="card-title">{edu.school}</h3>
              {edu.degree && <h4>{edu.degree}</h4>}
              {edu.fieldOfStudy && <div className="text-muted">{edu.fieldOfStudy}</div>}
              {edu.location && <div>{edu.location}</div>}
              
              <div className="text-muted mb-2">
                {edu.startDate && new Date(edu.startDate).getFullYear()}
                {edu.startDate && edu.endDate ? ' - ' : ''}
                {edu.endDate ? new Date(edu.endDate).getFullYear() : edu.startDate ? 'Present' : ''}
              </div>
              
              {edu.description && (
                <p className="card-text">{edu.description}</p>
              )}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Education;
