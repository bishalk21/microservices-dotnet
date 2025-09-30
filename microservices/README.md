# Microservices Architecture vs Monolithic Architecture

## Monolithic Architecture

In a monolithic architecture, the entire application is built as a single, unified unit. All components and functionalities are tightly integrated and run as a single service. This approach is straightforward to develop and deploy, making it suitable for small to medium-sized applications. However, as the application grows, it can become challenging to manage, scale, and maintain due to its complexity. Failures in one part of the application can potentially bring down the entire system.

### Architecture:

```
[Client] --> [Monolithic Application] --> [Database]

[User Interface (Server-side render)]
                |
            cart service
                |
            payment service
                |
            product service
                |
            order service
                |
            Auth service
                |
            [Database]

```

## Service-Oriented Architecture (SOA)

Service-Oriented Architecture (SOA) is an architectural pattern where services are provided to other components by application components, through a communication protocol over a network. SOA emphasizes reusability, interoperability, and the separation of concerns. Services in SOA are typically larger and more complex than those in microservices, often encompassing multiple business functionalities. SOA can be implemented using various technologies and protocols, such as SOAP and REST.

### Architecture:

```
[Client] --> [Service Bus] --> [Service 1] --> [Database 1]
                             |--> [Service 2] --> [Database 2]
                             |--> [Service 3] --> [Database 3]
                                |--> [Service 4] --> [Database 4]
```

## Microservices Architecture

Microservices architecture is an evolution of SOA that structures an application as a collection of small, autonomous services, each responsible for a specific business capability. These services communicate with each other through well-defined APIs, often using lightweight protocols like HTTP/REST or messaging queues. Microservices are independently deployable, allowing for greater flexibility in development, scaling, and maintenance. This architecture promotes continuous delivery and DevOps practices, enabling teams to work on different services simultaneously without affecting the entire application.

### Architecture:

```
[Client] --> [Web App]/[Mobile App] --> [Firewall] --> [API Gateway] -- (API) --> [Product MS] --> [Database] <-- event bus
                      |                                     |-----------(API) --> [Order MS] ----> [Database] ---> event bus
                      |                                     |-----------(API) --> [Cart MS] -----> [Database] ---> event bus
                      |                                     |-----------(API) --> [Payment MS] --> [Database] ---> event bus
                      |                                     |
                      |                                     |
        ------> Identity Provider (IDP) for Authentication and Authorization
```

- Any commercial softwares or services needs Identity Provider (IDP) for Authentication and Authorization. Examples of IDP are Auth0, AWS Cognito, Okta, etc.
  - The client (web app or mobile app) will redirect the user to the IDP for login.
    - After successful login, the IDP will redirect back to the client with an access token (JWT).
    - The client will include the access token in the Authorization header of each request to the API Gateway.
  - The API Gateway will validate the access token with the IDP before forwarding the request to the appropriate microservice.

### AWS Cognito as IDP

- Provides `User Identity, and User Authentication and Authorization via Email or Federated Access` (Google, Facebook, Apple, Amazon, etc). (user sign-up, sign-in, and access control)
- Every user created in AWS Cognito will have `attributes (i.e., email, phone number, address, etc), Groups (i.e., admin, user, etc), and Roles (i.e., permission to access AWS resources), and other attributes`.(unique identifier (sub) that can be used as the primary key in the microservice database.)
- Users can be created manually by the admin or can sign up themselves via the client application.
- AWS cognito provides built-in UI for user sign-up and sign-in, or you can create your own custom UI for web or mobile app.
- Users can also belong to a social identity provider (Google, Facebook, Apple, Amazon, etc) or enterprise identity provider (SAML, OIDC) or Active Directory (AD) via AWS Directory Service.
- AWS Cognito logins are based on open authentication standards such as OAuth 2.0, SAML 2.0, and OpenID Connect (OIDC).
- User Identity and Access Management (IAM) is a web service that helps you securely control access to AWS resources. You can use IAM to control who can use your AWS resources (authentication) and what resources they can use and in what ways (authorization).
- Amazon Cognito provides authentication, authorization, and user management for your web and mobile apps. Your users can sign in directly with a user name and password or through a third party such as Facebook, Amazon, Google, or Apple.
- Amazon Cognito scales to millions of users and supports sign-in with social identity providers, such as Facebook, Google, and Amazon, and enterprise identity providers via SAML 2.0 and OpenID Connect (OIDC).
- Amazon Cognito also supports multi-factor authentication (MFA) and encryption of data at rest and in transit.

### Principles of Microservices Architecture

1. **Single Responsibility**: Each microservice should focus on a specific business capability.
2. **Loose Coupling**: Services should be independent and communicate through well-defined APIs.
3. **Autonomy**: Each service can be developed, deployed, and scaled independently.
4. **Decentralized Data Management**: Each service manages its own database, promoting data ownership.
5. **Continuous Delivery**: Emphasizes automation and monitoring to enable frequent releases.

## Comparison of Monolithic, SOA, and Microservices Architectures

| Aspect          | Monolithic                                      | Service-Oriented (SOA)                                                 | Microservices                                |
| --------------- | ----------------------------------------------- | ---------------------------------------------------------------------- | -------------------------------------------- |
| Structure       | Single unified unit                             | Collection of services                                                 | Collection of small, autonomous services     |
| Deployment      | Single deployment unit                          | Multiple services, often deployed together                             | Independently deployable services            |
| Scalability     | Scale the entire application                    | Scale individual services, but often requires scaling related services | Scale individual services independently      |
| Communication   | Internal function calls                         | Often uses enterprise service bus (ESB) for communication              | Lightweight protocols (HTTP/REST, messaging) |
| Development     | Single codebase                                 | Larger services, often with shared databases                           | Small, focused codebases per service         |
| Maintenance     | Can become complex over time                    | Can be complex due to service dependencies                             | Simplified due to independent services       |
| Fault Isolation | Failure in one part can affect the whole system | Failures can cascade through services                                  | Failures are isolated to individual services |
