# VebTechTestTask
A test assignment to a VebTechnologies company for the junior position .NET developer.

# Task:
Create a ASP.NET Core Web API that will expose CRUD (Create, Read, Update, Delete) operations to manage the list of users.

# Architecture
This project was developed using clean monolit arhitecture with following tech stack:

### Main Technologies
- ASP.Core used as main web fraemwork for .NET
- EF Core for ORM
- ASP.Core Identity for Authorization
### Databases
- PostgreSQL
- Redis Stack for Repsonse Caching
### Libraries
- FluentValidation to validate requests and models
- ElasticSearch is used for structured loggging
- Serilog as log provider, integrate with ELS
- MediatR for implementing CQRS pattern
- MailKit for sending emails
- Swagger for documentation

### CI/CD

- Docker and Kubernetes for deploying

# Launch

To launch application in local environment, you need run these commands:

```shell
# Build Docker Images
C:\...\src> docker build -t yourname/vebtechtask .\

# Push Docker Images
C:\...\src> docker push yourname/vebtechtask

# Run Kubernetes services
C:\...\src> kubectl apply -f .\Deploy\

