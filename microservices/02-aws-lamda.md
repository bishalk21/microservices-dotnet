# AWS Lambda

AWS Lambda is a serverless compute service that lets you run code without provisioning or managing servers. You can use AWS Lambda to build applications that respond quickly to new information and events, and you only pay for the compute time you consume.

## Key Features of AWS Lambda

- **Event-Driven**: AWS Lambda is designed to respond to events, such as changes in data, shifts in system state, or user actions. It can be triggered by various AWS services like S3, DynamoDB, Kinesis, SNS, and more.
- **Automatic Scaling**: Lambda automatically scales your application by running code in response to each trigger. Your code runs in parallel and processes each trigger individually, scaling precisely with the size of the workload.
- **Pay-as-You-Go**: With AWS Lambda, you are charged based on the number of requests for your functions and the time your code executes. There are no charges when your code is not running.
- **Supports Multiple Languages**: AWS Lambda supports several programming languages, including Node.js, Python, Java, C#, Go, and Ruby. You can also bring your own runtime.
- **Integrated with AWS Services**: Lambda integrates seamlessly with other AWS services, allowing you to build complex applications using a variety of AWS resources.

## Common Use Cases

- **Data Processing**: Process data in real-time from sources like S3, DynamoDB, or Kinesis.
- **Web Applications**: Build serverless web applications that can scale automatically.
- **Backend Services**: Create backend services for mobile or web applications without managing servers.
- **Automation**: Automate tasks such as backups, log analysis, and system monitoring.
- **IoT Applications**: Process data from IoT devices and respond to events.

## Getting Started with AWS Lambda

1. **Create a Lambda Function**: You can create a Lambda function using the AWS Management Console, AWS CLI, or AWS SDKs. Choose a runtime, configure the function, and write your code.
2. **Set Up Triggers**: Configure triggers for your Lambda function from various AWS services or custom events.
3. **Test Your Function**: Use the built-in testing tools in the AWS Management Console to test your Lambda function with sample events.
4. **Monitor and Optimize**: Use AWS CloudWatch to monitor your Lambda functions, track performance metrics, and optimize your code for better efficiency.
5. **Deploy and Manage**: Use AWS SAM (Serverless Application Model) or AWS CloudFormation to deploy and manage your serverless applications.

## Best Practices

- **Keep Functions Small**: Design your Lambda functions to perform a single task. This makes them easier to manage, test, and optimize.
- **Use Environment Variables**: Store configuration settings in environment variables to avoid hardcoding them in your code.
- **Optimize Cold Starts**: Minimize the impact of cold starts by keeping your deployment package
  small and using provisioned concurrency for critical functions.
- **Implement Error Handling**: Use try-catch blocks and implement retries for transient errors to ensure your functions are resilient.
- **Secure Your Functions**: Use IAM roles to grant the least privilege necessary for your Lambda functions to access other AWS resources.
- **Monitor Usage**: Regularly monitor your Lambda functions using CloudWatch to track performance and identify potential issues.
- **Version Control**: Use Lambda function versions and aliases to manage different stages of your application (development, staging, production).
- **Optimize Dependencies**: Only include necessary libraries and dependencies in your deployment package to reduce size and improve performance.
- **Use Layers**: Utilize Lambda Layers to share common code and dependencies across multiple functions, reducing duplication and simplifying updates.
- **Test Thoroughly**: Implement unit tests and integration tests to ensure your Lambda functions work
  as expected in different scenarios.
- **Leverage Asynchronous Invocation**: For tasks that do not require immediate response, use asynchronous invocation to improve performance and reduce latency.
- **Set Timeouts Appropriately**: Configure function timeouts based on the expected execution
  time to avoid unnecessary charges and ensure timely execution.
- **Use VPC Wisely**: If your Lambda function needs to access resources in a VPC, ensure that it is configured correctly to avoid connectivity issues and increased latency.
- **Stay Updated**: Keep your Lambda runtime and dependencies up to date to benefit from the latest features, security patches, and performance improvements.
- **Leverage Step Functions**: For complex workflows, consider using AWS Step Functions to orchestrate multiple Lambda functions and manage state transitions.
- **Optimize Memory Allocation**: Experiment with different memory settings for your Lambda functions to find the
  optimal balance between performance and cost.
- **Use Dead Letter Queues (DLQ)**: Configure DLQs for your Lambda functions to capture failed events for later analysis and reprocessing.
- **Document Your Functions**: Maintain clear documentation for your Lambda functions, including their purpose,
  input/output parameters, and any dependencies or configurations.
- **Automate Deployments**: Use CI/CD pipelines to automate the deployment of your Lambda functions, ensuring consistent and reliable updates.
- **Consider Cost Implications**: Regularly review your Lambda usage and costs to identify opportunities for optimization and cost savings.
- **Leverage API Gateway**: Use Amazon API Gateway to create RESTful APIs that can trigger your Lambda functions, enabling you to build serverless web applications and services.
- **Implement Logging**: Use structured logging within your Lambda functions to facilitate easier debugging and monitoring of application behavior.
- **Use Tracing**: Enable AWS X-Ray tracing for your Lambda functions to gain insights into performance bottlenecks and latency issues.
- **Plan for Scalability**: Design your Lambda functions and architecture to handle sudden spikes in traffic, ensuring that your application remains responsive under load.
- **Understand Limits**: Familiarize yourself with AWS Lambda limits (e.g., execution time, memory, payload size) to design your functions within these constraints and avoid unexpected failures.
- **Engage with the Community**: Participate in AWS forums, attend webinars, and follow AWS blogs to stay informed about best practices, new features, and use cases for AWS Lambda.
- **Experiment and Iterate**: Continuously experiment with different configurations, optimizations, and architectures to find the best fit for your specific use case and requirements.
- **Leverage Third-Party Tools**: Explore third-party tools and frameworks that can enhance your development experience with AWS Lambda, such as serverless frameworks, monitoring tools, and deployment utilities.

## Set up in Jetbrains Rider

1. Open Jetbrains Rider and create a new solution.
2. Select "Class Library" as the project type.
3. Choose ".NET Core" or the ".NET 8.0" framework and give solution a name.
4. Click "Create" to generate the project.
5. Add the `Amazon.Lambda.Core` and `Amazon.Lambda.Serialization.SystemTextJson` NuGet packages to your project.
   - `Amazon.Lambda.Core` provides the core functionality for AWS Lambda functions.
   - `Amazon.Lambda.Serialization.SystemTextJson` allows you to use System.Text.Json for serialization and deserialization of JSON data.
6. Also add the `Amazon.Lambda.APIGatewayEvents` package if you plan to use API Gateway as a trigger.
   - This package contains classes that represent the request and response formats for API Gateway.
7. Create a new class `HotelAdmin.cs` and implement the Lambda function handler.
8. Add methods to handle different HTTP methods (GET, POST, PUT, DELETE) for managing hotel data.
9. Build the project to ensure there are no errors.
10. In Terminal, install `Amazon.Lambda.Tools` globally to enable deployment and management of Lambda functions from the command line.

    ```bash
    dotnet tool install -g Amazon.Lambda.Tools
    // locally
    dotnet new tool-manifest
    dotnet tool install --local Amazon.Lambda.Tools

    // To update the tool in the future, use:
    dotnet tool update -g Amazon.Lambda.Tools
    ```

11. navigate to the project directory, find the `.csproj` file, and run the following command to package the project into a ZIP file for deployment:

    ```bash
    // dotnet lambda package HotelManager_HotelAdmin.csproj -o HotelAdmin.zip
    dotnet lambda package HotelManager_HotelAdmin.csproj -o HotelAdmin.zip
    ```

12. Deploy the packaged Lambda function to AWS using the following command:

    ```bash
    dotnet lambda deploy-function HotelAdmin --function-role <YourLambdaExecutionRoleARN>
    ```

    or

13. Go to AWS Management Console, navigate to AWS Lambda, and create a new function. Upload the `HotelAdmin.zip` file as the function code.
    - set function name to `addHotel`
    - set runtime to `.NET 8 (C#/PowerShell)`
    - set execution role to an existing role with necessary permissions or create a new one.
    - Upload the `HotelAdmin.zip` file as the function code.
    - once uploaded, we need to tell AWS Lambda which method or endpoint to use as the entry point for our function. In the "Handler" field, enter `HotelAdmin::HotelManager.HotelAdmin::FunctionHandlerAsync`.
    - also can test the function by creating a test event with sample input data.
    - After testing, you can set up triggers for your Lambda function, such as API Gateway, S3, or DynamoDB, depending on your use case.
14. After deployment, you can monitor the function's performance and logs using AWS CloudWatch or just in Monitoring.
