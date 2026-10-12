# WorldAlerts

A C# ASP.NET Core backend for configurable event alerts with email and Slack notification channels.

## Main Components

- **REST API** – Manages alerts, processes incoming events, and provides admin endpoints.
- **Alert Management** – Creates, updates, lists, and deletes user alerts.
- **Event Processing** – Matches incoming events against configured alert rules.
- **Notification Channels** – Supports email and Slack through a shared interface.
- **Alert Service** – Coordinates event matching and notification delivery.
- **Delivery Tracking** – Records notification attempts and their status.
- **Admin API** – Provides alert overviews and delivery history.
- **Entity Framework Core** – Handles database access.
- **SQLite** – Stores alerts, notification channels, and delivery attempts.

## Extensibility

- New notification channels can be added through `INotificationChannel` implementations.
- New event sources can integrate through the event ingestion endpoint.
- Notification providers are registered through dependency injection.

## Getting Started

1. Install the .NET SDK compatible with the project target framework.
2. Restore dependencies with `dotnet restore`.
3. Start the application with `dotnet run`.
4. Update the base URL in `WebApplication1.http` if necessary.
5. Execute the HTTP requests to test the endpoints.

The SQLite database is created automatically on startup.

## API Endpoints

- `GET /api/users/{userId}/alerts` – Lists a user's alerts.
- `POST /api/users/{userId}/alerts` – Creates an alert.
- `PUT /api/users/{userId}/alerts/{id}` – Updates an alert.
- `DELETE /api/users/{userId}/alerts/{id}` – Deletes an alert.
- `POST /api/events` – Processes an incoming event.
- `GET /api/admin/alerts` – Lists all alerts.
- `GET /api/admin/deliveries` – Lists recent delivery attempts.

## Current Limitations

- Email and Slack providers are demo implementations.
- Authentication and authorization are not implemented.
- Delivery retries and background processing are not implemented.
- Database schema migrations are not configured.



## Developer Note

### Scope and Design Decisions

- The project intentionally uses Minimal APIs instead of controllers.
- The project intentionally focuses on the backend only.
- The implementation prioritizes a minimal, demonstrable solution over production readiness.
- The architecture and technology choices were defined before implementation.

### Development Process

- I defined the demo project's architecture and selected the technologies to be used.
- I then used AI to generate the initial implementation based on these decisions.
- I verified the generated code, established the project structure, fixed minor issues, and tested the endpoints.
- The implementation was developed locally and uploaded to GitHub in a single initial commit after a relatively short development cycle. Therefore, the repository does not reflect the individual development steps.

### Potential Improvements

The implementation can be extended and refactored in many directions. Examples include:

- Adding authentication and role-based authorization.
- Replacing demo notification providers with real email and Slack integrations.
- Introducing background workers and a message queue.
- Implementing retries, delivery guarantees, and idempotency.
- Adding database migrations and improved persistence configuration.
- Expanding automated unit and integration test coverage.
- Improving validation, error handling, logging, and observability.
- Adding a frontend for users and administrators.
- Introducing more sophisticated alert rules and event filtering.

### Final Remarks

The primary goal was to build a minimal working demo while deliberately leveraging AI assistance for implementation. The result is a starting point for demonstrating the proposed architecture and core functionality, rather than a fully production-ready application.

The code can be improved and refactored in numerous ways, depending on the project's future requirements.