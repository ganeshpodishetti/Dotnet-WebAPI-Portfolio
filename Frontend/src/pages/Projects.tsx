import React, { useEffect, useState } from 'react';
import { projectService } from '../services/api';
import { ProjectRequestDto } from '../types';

const Projects = () => {
  const [projects, setProjects] = useState<ProjectRequestDto[]>([]);

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const data = await projectService.getAll();
        setProjects(data);
      } catch (error) {
        console.error('Error fetching projects:', error);
      }
    };

    fetchProjects();
  }, []);

  return (
    <div className="container">
      <h2>My Projects</h2>
      <div className="row">
        {projects.map((project, index) => (
          <div key={index} className="col-md-4 mb-4">
            <div className="card">
              <div className="card-body">
                <h5 className="card-title">{project.name}</h5>
                <p className="card-text">{project.description}</p>
                {project.url && (
                  <a href={project.url} className="btn btn-primary me-2" target="_blank" rel="noopener noreferrer">
                    View Project
                  </a>
                )}
                {project.githubUrl && (
                  <a href={project.githubUrl} className="btn btn-secondary" target="_blank" rel="noopener noreferrer">
                    GitHub
                  </a>
                )}
                {project.skills && (
                  <div className="mt-2">
                    {project.skills.map((skill, i) => (
                      <span key={i} className="badge bg-info me-1">{skill}</span>
                    ))}
                  </div>
                )}
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Projects;
