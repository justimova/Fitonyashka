# Fitonyashka

Full-stack health and fitness tracking application built with ASP.NET Core and Angular.

Fitonyashka is a pet project focused on personal fitness progress tracking. It includes user authentication, profile management, BMI calculation, goals, weight tracking and a modular Angular frontend connected to an ASP.NET Core backend.

## Project Overview

The project was created as a full-stack fitness application with a modern Angular frontend and a structured ASP.NET Core backend.

The goal was to build more than a static UI: the application includes backend business logic, REST API endpoints, authentication, data access layer, file/avatar handling and a structured frontend architecture. The planned direction is to extend the platform with AI-powered functionality for personalized recommendations, workout program generation, progress analysis and other health and fitness insights.

## Features

- User registration and login
- JWT-based authentication
- Personal user profile management
- Avatar upload and update
- BMI calculation
- Fitness goal management
- Weight tracking
- Date range support for weight progress
- Dashboard-oriented user area
- REST API for frontend/backend communication

## Tech Stack

### Backend

- C#
- .NET
- ASP.NET Core Web API
- JWT Bearer Authentication
- Entity Framework Core
- Layered backend architecture

### Frontend

- Angular
- TypeScript
- Angular Material
- Bootstrap
- Chart.js / ng2-charts
- HTML
- CSS

### Cloud / Deployment

- Azure deployment in progress

## Architecture

The backend is organized into separate layers:

```text
Fitonyashka/
├── PresentationLayer/      # API controllers and view models
├── BusinessLogicLayer/     # Business services and application logic
├── DataAccessLayer/        # Entities, repositories and database context
├── InfrastructureLayer/    # Infrastructure helpers, auth and extensions
└── ClientApp/              # Angular frontend
```

The Angular application is structured by responsibility:

```text
ClientApp/src/app/
├── core/       # core services, guards and shared infrastructure
├── features/   # feature modules: auth, public, guest and user areas
├── layouts/    # page layouts
└── shared/     # reusable shared components
```

## Engineering Highlights

- Full-stack application with ASP.NET Core backend and Angular frontend
- JWT authentication for protected user-specific functionality
- Domain-oriented backend services for BMI, goals, users, files and weight tracking
- REST API controllers separated by application area
- Layered backend structure with presentation, business logic, data access and infrastructure responsibilities
- Angular feature-based structure with separate modules for authentication, public pages and user area
- Chart-ready frontend stack for visualizing user progress
- Planned AI-powered functionality for personalized fitness recommendations and workout generation
- Azure deployment preparation in progress

## Current Status

The project is a personal pet project focused on fitness tracking, full-stack development practice and gradual product expansion.

Implemented:

- backend layer structure
- authentication flow
- user profile functionality
- avatar handling
- BMI-related logic
- goals functionality
- weight tracking logic
- Angular application structure

In progress / planned:

- Azure deployment
- AI-powered personalized recommendations
- AI-generated workout programs
- progress analysis and fitness insights
- improved UI/UX for dashboard and statistics pages
- automated backend and frontend tests
- Swagger/OpenAPI documentation
- Docker configuration
- screenshots and demo video

## What I Practiced

While building this project, I practiced:

- developing a full-stack application with ASP.NET Core and Angular
- designing REST API endpoints for user-specific features
- implementing JWT-based authentication
- structuring backend code into separate layers
- working with Entity Framework Core and repository-based data access
- building a modular Angular application
- connecting frontend features with backend business logic
- planning future AI-powered product functionality

## Author

Developed by [@justimova](https://github.com/justimova)
