# Order System

This is a .NET Core-based order system designed to explore different architectural techniques while maintaining a structured and scalable approach. 
Users will be able to interact with orders based on their role.
- Sales users will be able to create, edit and delete orders
- Finance users will be able to mark an order as paid
- Shipping users will be able to mark an order as shipped

In the process, the different roles will only see the data they need for their actions.


## Architecture

- **API**: A standalone backend handling business logic and data processing.
- **Frontend**: A separate client-side application.
- **Authentication & Authorization**: Implements role-based access control.
- **CQRS with MediatR**: Commands and queries are managed through MediaTR.

## Current State

- Authentication logic and controllers have been implemented in the API.

## Technical Considerations

This project follows a vertical slice architecture to keep concerns isolated and maintain clear separation of features. The application serves as a foundation to experiment with different techniques.

Future updates will refine and expand upon these concepts as additional functionality is introduced.

## How to run

The project itself is a c# 9 project with a database contained inside a docker file.
