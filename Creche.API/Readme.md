# CrecheAPI - Serverless Management System for Day Care Centers

## Overview

CrecheAPI is a serverless application developed with ASP.NET Core, designed to optimize the management of daycare centers. With the adoption of the hexagonal architecture and CQRS (Command Query Responsibility Segregation) principles, this API offers a robust and highly scalable platform, allowing a clear separation between read and write operations, which improves system performance and maintainability.

## Architecture

The system is based on Hexagonal Architecture, favoring modularity and facilitating testing, maintenance and application evolution. The main components of the architecture include:

- **Central Domain**: Where the business logic and main entities are located.
- **Application Layer**: Contains use cases, commands, handlers, queries and responses.
- **Infrastructure Layer**: Responsible for data persistence and external communication.

Furthermore, the application uses the CQRS standard, which separates state modification operations (commands) from query operations, allowing greater flexibility and scalability.

## Functionalities

- **Daycare Management**: Complete features for daycare management, including registration, updating, recovery and deletion of data.
- **Integration with RabbitMQ**: Sending and receiving asynchronous messages through RabbitMQ, enabling decoupled communication between different parts of the system.
- **Persistence with DynamoDB**: Use of AWS DynamoDB, ensuring high availability and performance for data operations.
- **Cache with Redis**: Use of Redis to cache data, improving the response time of frequent queries and reducing the load on the database.
- **CQRS**: Adoption of the CQRS standard, separating the responsibility for reading and writing to facilitate scalability and maintenance.

## Serverless Platform on AWS

Designed to run on AWS, CrecheAPI benefits from the serverless nature of AWS Lambda and Amazon API Gateway, ensuring efficient operation with automatic scaling and cost optimization.


## Estructure
Creche.API
│
├── Connected Services
├── Controllers
│ └── UnitController.cs
├── Dependencies
├── Mappers
│ └── ProfileMapper.cs
├── Messaging
│ └── RabbitMqProducer.cs
├── Options
│ ├── RabbitMQOptions.cs
│ └── RedisOptions.cs
├── Program.cs
├── Properties
├── Readme.md
├── Services
│ └── EmailService.cs
├── Startup.cs
└── aws-lambda-tools-defaults.json
appsettings.Development.json
appsettings.json
serverless.template

Creche.Application
│
├── Commands
│ └── Unit
│ └── CreateUnitCommand.cs
├── Dependencies
├── DTOs
│ └── UnitDto.cs
├── Exceptions
│ └── ValidationException.cs
├── Handlers
│ └── Unit
│ ├── CreateUnitCommandHandler.cs
│ └── GetUnitByIdHandler.cs
├── Queries
│ └── Unit
│ └── GetUnitByIdQuery.cs
├── Responses
│ ├── ApiResponse.cs
│ └── UnitResponse.cs
└── Validators
└── UnitDtoValidator.cs

Creche.Domain
│
├── Dependencies
└── Entities
├── Atividade.cs
├── Comunicacao.cs
├── Crianca.cs
├── Funcionario.cs
├── RegistroAtividade.cs
└── UnitEntity.cs

Creche.Infrastructure
│
├── Dependencies
├── ExternalServices
│ └── StripePaymentService.cs
├── Interfaces
│ ├── IMessageProducer.cs
│ └── IUnitRepository.cs
├── Messaging
│ └── RabbitMqProducer.cs
└── Repositories
└── UnitRepository.cs

## Here are some steps to follow from Visual Studio:

To deploy your Serverless application, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed application open the Stack View window by double-clicking the stack name shown beneath the AWS CloudFormation node in the AWS Explorer tree. The Stack View also displays the root URL to your published application.

## Here are some steps to follow to get started from the command line:

Once you have edited your template and code you can deploy your application using the [Amazon.Lambda.Tools Global Tool](https://github.com/aws/aws-extensions-for-dotnet-cli#aws-lambda-amazonlambdatools) from the command line.

Install Amazon.Lambda.Tools Global Tools if not already installed.
```
    dotnet tool install -g Amazon.Lambda.Tools
```

If already installed check if new version is available.
```
    dotnet tool update -g Amazon.Lambda.Tools
```

Deploy application
```
    cd "SendEmail.API/src/SendEmail.API"
    dotnet lambda deploy-serverless
```
