import axios, { AxiosError, AxiosRequestConfig } from 'axios';
import {
    ChangePasswordDto,
    EducationRequestDto,
    ExperienceRequestDto,
    LoginRequestDto,
    MessageRequestDto,
    ProjectRequestDto,
    RegisterRequestDto,
    SkillRequestDto,
    SocialLinkRequestDto,
    UpdateMessageDto,
    UserRequestDto
} from '../types';
import { isAuthenticated, logout, refreshTokenExpiry } from './authUtils';

// Use environment variable for API base URL
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5067';

const api = axios.create({
  baseURL: API_BASE_URL,
});

// Request interceptor to add auth token
api.interceptors.request.use(async (config: AxiosRequestConfig) => {
  const token = localStorage.getItem('token');
  
  if (token) {
    // Check if token is still valid
    if (await isAuthenticated()) {
      config.headers = config.headers || {};
      config.headers.Authorization = `Bearer ${token}`;
      
      // Refresh token expiry on successful request
      refreshTokenExpiry();
    } else {
      // Token is invalid or expired, redirect to login
      logout();
    }
  }
  
  return config;
});

// Response interceptor to handle errors
api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      // Unauthorized - clear token and redirect to login
      logout();
    }
    return Promise.reject(error);
  }
);

export const authService = {
  login: async (credentials: LoginRequestDto) => {
    try {
      const response = await api.post('/api/authentication/login', credentials);
      return response.data;
    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        throw new Error(error.response.data.message || 'Invalid response from server. Please try again.');
      }
      throw new Error('Invalid response from server. Please try again.');
    }
  },
  register: async (data: RegisterRequestDto) => {
    const response = await api.post('/api/authentication/register', data);
    return response.data;
  },
  changePassword: async (data: ChangePasswordDto) => {
    const response = await api.post('/api/authentication/changePassword', data);
    return response.data;
  },
  deleteUser: async () => {
    const response = await api.delete('/api/authentication/deleteUser');
    return response.data;
  }
};

export const projectService = {
  getAll: async () => {
    const response = await api.get('/api/projects');
    return response.data;
  },
  create: async (data: ProjectRequestDto) => {
    const response = await api.post('/api/projects', data);
    return response.data;
  },
  update: async (id: string, data: ProjectRequestDto) => {
    const response = await api.patch(`/api/projects/${id}`, data);
    return response.data;
  },
  delete: async (id: string) => {
    const response = await api.delete(`/api/projects/${id}`);
    return response.data;
  },
};

export const educationService = {
  getAll: async () => {
    const response = await api.get('/api/education');
    return response.data;
  },
  create: async (data: EducationRequestDto) => {
    const response = await api.post('/api/education', data);
    return response.data;
  },
  update: async (id: string, data: EducationRequestDto) => {
    const response = await api.patch(`/api/education/${id}`, data);
    return response.data;
  },
  delete: async (id: string) => {
    const response = await api.delete(`/api/education/${id}`);
    return response.data;
  },
};

export const experienceService = {
  getAll: async () => {
    const response = await api.get('/api/experience');
    return response.data;
  },
  create: async (data: ExperienceRequestDto) => {
    const response = await api.post('/api/experience', data);
    return response.data;
  },
  update: async (id: string, data: ExperienceRequestDto) => {
    const response = await api.patch(`/api/experience/${id}`, data);
    return response.data;
  },
  delete: async (id: string) => {
    const response = await api.delete(`/api/experience/${id}`);
    return response.data;
  },
};

export const skillService = {
  getAll: async () => {
    const response = await api.get('/api/skills');
    return response.data;
  },
  create: async (data: SkillRequestDto) => {
    const response = await api.post('/api/skills', data);
    return response.data;
  },
  update: async (id: string, data: SkillRequestDto) => {
    const response = await api.patch(`/api/skills/${id}`, data);
    return response.data;
  },
  delete: async (id: string) => {
    const response = await api.delete(`/api/skills/${id}`);
    return response.data;
  },
};

export const messageService = {
  getAll: async () => {
    const response = await api.get('/api/messages');
    return response.data;
  },
  create: async (data: MessageRequestDto) => {
    const response = await api.post('/api/messages', data);
    return response.data;
  },
  getUnread: async () => {
    const response = await api.get('/api/messages/unreadMessages');
    return response.data;
  },
  markAsRead: async (id: string, data: UpdateMessageDto) => {
    const response = await api.patch(`/api/messages/${id}`, data);
    return response.data;
  },
  delete: async (id: string) => {
    const response = await api.delete(`/api/messages/${id}`);
    return response.data;
  },
};

export const userService = {
  getProfile: async () => {
    const response = await api.get('/api/user');
    return response.data;
  },
  updateProfile: async (data: UserRequestDto) => {
    const response = await api.patch('/api/user', data);
    return response.data;
  },
};

export const socialLinkService = {
  getAll: async () => {
    const response = await api.get('/api/socialLink');
    return response.data;
  },
  create: async (data: SocialLinkRequestDto) => {
    const response = await api.post('/api/socialLink', data);
    return response.data;
  },
  update: async (id: string, data: SocialLinkRequestDto) => {
    const response = await api.patch(`/api/socialLink/${id}`, data);
    return response.data;
  },
  delete: async (id: string) => {
    const response = await api.delete(`/api/socialLink/${id}`);
    return response.data;
  },
};

export default api;
