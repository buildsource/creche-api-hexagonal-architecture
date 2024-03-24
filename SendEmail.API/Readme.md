# SendEmail API - Serverless Application for Sending Emails

## Overview
The SendEmail API is a serverless solution built on the .NET 8 platform that operates within the AWS ecosystem. The application is designed to read messages from a RabbitMQ queue and, upon receiving new messages, triggers corresponding emails.

This approach allows the application to be highly scalable and efficient, as message processing and email sending occur in response to events, leveraging the capabilities of AWS Lambda and Amazon API Gateway for an event-driven architecture.

## Operation
When a new message is published to the queue configured in RabbitMQ, the SendEmail API processes that message and sends an email based on the received content. This is ideal for automatic notifications, transaction confirmations or any other automated communication that requires a quick and reliable response.

Serverless infrastructure means there is no need to manage servers or runtime environments, allowing the team to focus on development and business logic while AWS takes care of infrastructure scalability and maintenance.  

## Estructure
SendEmail.API
│
├── Connected Services
├── Dependencies
├── Properties
├── DTOs
│   └── EmailDto.cs
├── Interfaces
│   ├── IEmailService.cs
│   └── IMessageConsumer.cs
├── Messaging
│   └── RabbitMQMessageConsumer.cs
├── Options
│   ├── EmailOptions.cs
│   └── RabbitMQOptions.cs
├── Services
│   └── EmailService.cs
├── appsettings.json
├── aws-lambda-tools-defaults.json
├── Program.cs
├── Readme.md
└── serverless.template

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
