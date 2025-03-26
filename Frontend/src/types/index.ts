export interface Project {
  id: number;
  title: string;
  description: string;
  imageUrl?: string;
  projectUrl?: string;
  technologies: string[];
  createdAt: string;
}

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface AuthResponse {
  token: string;
  user: {
    username: string;
    role: string;
  };
}

export interface RegisterRequestDto {
  userName: string;
  email: string;
  password: string;
}

export interface LoginRequestDto {
  email: string;
  password: string;
}

export interface ChangePasswordDto {
  currentPassword: string;
  newPassword: string;
}

export interface ProjectRequestDto {
  name: string;
  description: string;
  url?: string;
  githubUrl?: string;
  skills?: string[];
}

export interface EducationRequestDto {
  school: string;
  degree?: string;
  location?: string;
  fieldOfStudy?: string;
  startDate?: string;
  endDate?: string | null;
  description?: string;
}

export interface ExperienceRequestDto {
  title: string;
  companyName: string;
  location?: string;
  startDate?: string;
  endDate?: string | null;
  description?: string;
}

export interface SkillRequestDto {
  skillCategory: string;
  skillsTypes: string[];
}

export interface MessageRequestDto {
  name: string;
  email: string;
  subject?: string;
  content: string;
}

export interface UpdateMessageDto {
  isRead: boolean;
}

export interface UserRequestDto {
  firstName?: string;
  lastName?: string;
  profilePicture?: string;
  bio?: string;
  headline?: string;
  country?: string;
  city?: string;
}

export interface SocialLinkRequestDto {
  platform: string;
  url: string;
  icon?: string;
}
