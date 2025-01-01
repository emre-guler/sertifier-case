# Sertifier Technical Case

This project is a technical case study for Sertifier, implementing a course management system with attendee tracking and certification capabilities. The system is built using .NET Core and follows a clean architecture pattern.

## Features

- Course Management
  - Create and list courses
  - Search courses by title
  - Integration with Sertifier's certification system
  
- Attendee Management
  - Create and manage attendees
  - Enroll attendees in courses
  - Track course completion
  
- Certification System
  - Automatic certificate generation for completed courses
  - Integration with Sertifier's API for certificate delivery
  - Credential tracking and management
  
- Leaderboard System
  - Track and display top performers
  - View number of completed courses per attendee

## Project Structure

- **SertifierCase.API**: Main API project containing controllers and endpoints
- **SertifierCase.Data**: Data access layer with Entity Framework Core
- **SertifierCase.Infrastructure**: Shared models, utilities, and error handling
- **SertifierCase.Services**: Business logic and service implementations

## Technical Stack

- .NET Core
- Entity Framework Core
- MySQL Database
- RESTful API
- Mapster for object mapping

## API Endpoints

### Courses
- `POST /api/courses/`: Create a new course
- `GET /api/courses/`: List courses with pagination and search

### Attendees
- `POST /api/attendees/`: Create a new attendee
- `POST /api/attendees/{attendeeId}/EnrollAttendee`: Enroll an attendee in a course
- `GET /api/attendees/LeaderBoard`: Get the leaderboard of top performers

## Database Schema

The system uses the following main entities:
- `Course`: Stores course information
- `Attendee`: Manages attendee data
- `CourseAttendee`: Tracks course enrollment
- `Credential`: Manages certification records

## Getting Started

1. Ensure you have .NET Core SDK installed
2. Configure your database connection in `appsettings.json`
3. Run database migrations:
   ```bash
   dotnet ef database update
   ```
4. Start the application:
   ```bash
   dotnet run
   ```

## Configuration

Update the `appsettings.json` file with your specific settings:
- Database connection string
- Sertifier API credentials
- Other environment-specific configurations

## Error Handling

The system implements a standardized error handling mechanism with custom error codes:
- E_100: General error
- E_101: Invalid input
- E_102: Duplicate email
- E_103: Attendee not found
- E_104: Course not found
- E_105: Already enrolled

## Response Format

All API endpoints return responses in a standardized format:
```json
{
    "hasError": boolean,
    "message": string,
    "data": object,
    "metaData": {
        "offset": integer,
        "limit": integer,
        "count": integer
    }
}
```
