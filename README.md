# UsersManager
ASP.NET Core Web CRUD API to manage the list of users. Add pagination and filtration, mail confirmation and deploy it to k8s.

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
At first you need setup user secrets:
```shell
dotnet user-secrets set "Smtp:Password" "ebqatxmtxaurcdfu" --project $PROJECT_PATH
dotnet user-secrets set "Jwt:Key" "865D92FD-B1C8-41A4-850F-409792C9B740" --project $PROJECT_PATH
dotnet user-secrets set "Jwt:Audience" "identity" --project $PROJECT_PATH
dotnet user-secrets set "Jwt:Issuer" "identity" --project $PROJECT_PATH
dotnet user-secrets set "Admin:Password" "Adm1n.dev-31_13%" --project $PROJECT_PATH
dotnet user-secrets set "Admin:Email" "identity.dev@gmail.com" --project $PROJECT_PATH
dotnet user-secrets set "Admin:Id" "4e274126-1d8a-4dfd-a025-806987095809" --project $PROJECT_PATH
```shell

```shell
# Run posgresql database
C:\...\src> docker compose up

# Run WebApi Project
C:\...\src> dotnet run External/Presentation/Presentation.csproj
```shell

Or to launch application with k8s, you need run these commands:

```shell
# Build Docker Images
C:\...\src> docker build -t yourname/vebtechtask .\

# Push Docker Images
C:\...\src> docker push yourname/vebtechtask

# Run Kubernetes services
C:\...\src> kubectl apply -f .\Deploy\
shell```

