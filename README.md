# Portfolio API

A comprehensive .NET Web API for managing a personal portfolio with features for authentication, project management, education details, skills, and contact messaging.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Configuration](#configuration)
- [API Documentation](#api-documentation)
- [Authentication](#authentication)
- [Logging and Monitoring](#logging-and-monitoring)
- [Error Handling](#error-handling)
- [Project Structure](#project-structure)
- [Additional Improvements](#additional-improvements)
- [Contributing](#contributing)
- [License](#license)

## Overview

This Portfolio API is designed to power a personal portfolio website, enabling you to manage your professional information, projects, education history, skills, and receive messages from visitors, all within a secure, authenticated environment.

## Project Structure

```
src/
├── API/                    # Web API project (entry point)
│   ├── Controllers/        # API controllers
│   ├── Filters/            # Action filters and attributes
│   ├── Middlewares/        # Custom middleware components
│   └── Program.cs          # Application bootstrap
├── Application/            # Application layer
│   ├── Auth/               # Authentication services
│   ├── Common/             # Shared application components
│   ├── DTOs/               # Data transfer objects
│   ├── Interfaces/         # Application service interfaces
│   ├── Mapping/            # AutoMapper profiles
│   ├── Services/           # Implementation of application services
│   └── Validation/         # Validation rules
├── Domain/                 # Domain layer
│   ├── Entities/           # Domain entities
│   ├── Enums/              # Enumeration types
│   ├── Events/             # Domain events
│   ├── Exceptions/         # Custom domain exceptions
│   └── ValueObjects/       # Value objects
├── Infrastructure/         # Infrastructure layer
│   ├── Data/               # Database context and configuration
│   │   ├── Configurations/ # Entity type configurations
│   │   ├── Migrations/     # EF Core migrations
│   │   └── Repositories/   # Repository implementations
│   ├── Email/              # Email service implementation
│   ├── Identity/           # Identity and authentication implementation
│   └── Services/           # External service integrations
└── Shared/                 # Shared components
    ├── Constants/          # Application constants
    ├── Extensions/         # Extension methods
    └── Helpers/            # Helper utilities
```

## Features

- **Authentication System**: JWT-based authentication with refresh tokens
- **User Management**: Create and manage user profiles
- **Project Management**: Add, update, and showcase your projects
- **Education History**: Track and display your educational background
- **Skill Management**: Catalog your technical and professional skills
- **Contact System**: Receive messages from website visitors
- **Social Links**: Manage links to your social media profiles
- **Role-Based Access Control**: Admin-only actions for content management

## Architecture

The project follows the principles of Clean Architecture with:

- **Domain Layer**: Core entities, interfaces, and business rules
- **Application Layer**: Application services, DTOs, and business logic
- **Infrastructure Layer**: External concerns like database access
- **API Layer**: Controllers and presentation logic

## Tech Stack

- **.NET 8**: The core framework for building the API
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Primary database
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **JWT Authentication**: Secure token-based authentication
- **Serilog**: Structured logging
- **OpenTelemetry**: Distributed tracing and monitoring
- **Scalar**: API documentation

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server (or SQL Server Express)
- Visual Studio / VS Code / Rider

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/Dotnet-WebAPI-Portfolio.git
   cd Dotnet-WebAPI-Portfolio
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the solution:
   ```bash
   dotnet build
   ```

4. Apply database migrations:
   ```bash
   cd src/API
   dotnet ef database update
   ```

### Configuration

1. Update the connection string in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=PortfolioDB;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```

2. Configure JWT settings:
   ```json
   "JwtTokenOptions": {
     "Secret": "[Your Secret Key]",
     "Issuer": "PortfolioAPI",
     "Audience": "PortfolioClients",
     "AccessTokenExpirationMinutes": 60,
     "RefreshTokenExpirationDays": 7
   }
   ```

## API Documentation

The API is documented using Scalar, which is available when running the application in Development mode.

Access the API documentation at: `http://localhost:5000/api/docs`

## Authentication

The API uses JWT bearer token authentication. To access protected endpoints:

1. Register a user or log in with an existing user
2. Use the returned JWT token in the Authorization header:
   ```
   Authorization: Bearer [your-token]
   ```
3. Admin privileges are required for most content management endpoints

## Logging and Monitoring

The application uses Serilog for structured logging and OpenTelemetry for distributed tracing.

- **Logs**: Console and configurable outputs
- **Metrics**: Basic HTTP and application metrics
- **Tracing**: Request and database operation tracing

## Error Handling

The API implements a global error handling strategy with:

- **Result Pattern**: Consistent return types with success/failure indicators
- **Standardized Error Responses**: Uniform format for all errors
- **Detailed Logging**: Comprehensive logging of errors with context

## Additional Improvements

This project is continuously evolving. Planned improvements include:

- **Caching**: Implement Redis caching for improved performance
- **Background Jobs**: Add Hangfire for scheduling tasks like email sending
- **File Storage**: Integrate Azure Blob Storage for project media files
- **API Versioning**: Support multiple API versions
- **Rate Limiting**: Protect against abuse with API rate limiting
- **GraphQL Endpoint**: Alternative to REST for more flexible data fetching
- **Localization**: Support for multiple languages
- **Two-Factor Authentication**: Enhanced security with 2FA
- **Webhooks**: Event notifications for integration with other services
- **Analytics**: Basic analytics for tracking portfolio visits
- **CI/CD Pipeline**: Automated testing and deployment workflows
- **Docker Support**: Containerization for easier deployment

## Contributing

Contributions are welcome! To contribute:

1. **Fork the Repository**: Create your own copy of the project
2. **Create a Branch**:
   ```bash
   git checkout -b feature/amazing-feature
   ```
3. **Make Changes**: Implement your feature or fix
4. **Follow Code Standards**:
    - Use the .editorconfig for code style
    - Write tests for new functionality
    - Follow the existing architectural patterns
5. **Commit Your Changes**:
   ```bash
   git commit -m 'Add some amazing feature'
   ```
6. **Push to Your Branch**:
   ```bash
   git push origin feature/amazing-feature
   ```
7. **Open a Pull Request**: Describe your changes in detail

### Development Guidelines
- Maintain test coverage
- Update documentation for new features
- Follow domain-driven design principles
- Use the result pattern for method returns

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

Copyright (c) 2023 Ganeshan

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files.
