# Dotnet WebAPI Portfolio Project

This repository contains a structured .NET Web API project following clean architecture principles.

## Project Structure

The solution is organized into two main sections:

### Backend

The backend is built using .NET 7/8 and follows Clean Architecture principles with the following layers:

- **API**: Contains controllers, middleware, and API configuration
- **Application**: Contains business logic, DTOs, interfaces, and service implementations
- **Domain**: Contains domain entities, enums, exceptions, and domain logic
- **Infrastructure**: Contains implementations of repositories, external service integrations, and data access

### Frontend

This directory is prepared for the frontend implementation (React, Angular, etc.)

## Getting Started

1. Clone the repository
2. Navigate to the solution directory
3. Run `dotnet restore`
4. Run `dotnet build`
5. Navigate to the API project: `cd Backend/API`
6. Run the project: `dotnet run`

## Architecture

This project follows Clean Architecture principles:
- Domain-centric approach
- Dependency inversion (dependencies point inward)
- Separation of concerns
- Testability at all levels
