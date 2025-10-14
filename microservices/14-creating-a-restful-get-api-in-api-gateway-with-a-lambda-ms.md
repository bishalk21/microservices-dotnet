# Creating a RESTful GET API in API Gateway with a Lambda Microservice

In this section, we will create a RESTful GET API using AWS API Gateway that integrates with a Lambda function to serve as a microservice. This microservice will handle incoming GET requests and return appropriate responses.

## Step 1: Create a Lambda Function

1. Navigate to the AWS Management Console and open the Lambda service.
2. Click on "Create function".
3. Choose "Author from scratch".
4. Provide a function name (e.g., `ListAdminHotels`).
5. upload the HotelAdmin.zip file as its code
6. Select the runtime (.net 8.0).
7. Go to the Code table, and under the Runtime Settings section, edit the Handler attribute and point it to `HotelManager_HotelAdmin::HotelManager_HotelAdmin::ListHotels`
8. Click "Create function".
9. In the function code section, implement the logic to handle GET requests. For example:

```csharp
public class HotelAdmin
{
    public async Task<APIGatewayProxyResponse> ListHotels(APIGatewayProxyRequest request)
    {

        // to get id of user, we need to query string parameter called token is passed to this lambda function

        var response = new APIGatewayProxyResponse
        {
            Headers = new Dictionary<string, string>(),
            StatusCode = 200,
        };
        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        response.Headers.Add("Access-Control-Allow-Headers", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "GET,OPTIONS,POST");
        response.Headers.Add("Content-Type", "application/json");

        var token = request.QueryStringParameters["token"];
        // this token is idToken from cognito user pool, it is jwt token
        // we can use this token to get user information
        // we need to parse or validate this token to get user information
        var tokenDetails = new JwtSecurityToken(token);
        var userId = tokenDetails.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        // for region, we can get it from environment variable
        var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "ap-southeast-2"; // default to ap-southeast-2 if not set
        // dynamodb client
        var client = new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region));
        var context = new DynamoDBContext(client);

           var hotels = await dbContext.ScanAsync<Hotel>(new[] { new ScanCondition("UserId", ScanOperator.Equal, userId) })
            .GetRemainingAsync();

        response.Body = JsonSerializer.Serialize(new { Hotels = hotels });
        return response;
    }
}
```

8. Click "Deploy" to save your changes.

## Step 2: Create an API Gateway

1. Navigate to the AWS Management Console and open the API Gateway service.
2. Click on "Create API".
3. Choose "REST API" and click "Build".
4. Provide an API name (e.g., `HotelAdminAPI`) and click "Create API".
5. In the left-hand menu, click on "Actions" and select "Create Resource".
6. Provide a Resource Name (e.g., `hotels`) and Resource Path (e.g., `/hotels`), then click "Create Resource".
7. With the `/hotels` resource selected, click on "Actions" and select "Create Method".
8. Choose "GET" from the dropdown and click the checkmark to confirm.
9. In the "Setup" section, select "Lambda Function" as the integration type.
10. Check the box for "Use Lambda Proxy integration".
11. In the "Lambda Function" field, enter the name of the Lambda function you created earlier (e.g., `ListAdminHotels`).
12. Click "Save" and then "OK" to give API Gateway permission to invoke your Lambda function.
13. Click on "Actions" and select "Deploy API".
14. Create a new stage (e.g., `prod`) and click "Deploy".
15. Note the Invoke URL provided after deployment (e.g., `https://{api-id}.execute-api.{region}.amazonaws.com/prod`).

## Step 3: Test the API

1. Use a tool like Postman or curl to send a GET request to the API Gateway endpoint.
2. Include the `token` query parameter with a valid JWT token from your Cognito User Pool.
   Example:
   `GET https://{api-id}.execute-api.{region}.amazonaws.com
/prod/hotels?token=your_jwt_token_here`
3. You should receive a JSON response containing the list of hotels associated with the user identified by the token.
4. If you encounter any issues, check the CloudWatch logs for your Lambda function to debug and resolve them.
   `GET https://{api-id}.execute-api.{region}.amazonaws.com/prod/hotels?token=your_jwt_token_here`

## Creating and Assigning the execution role to the lambda function

1. Navigate to the AWS Management Console and open the IAM service.
2. Click on "Roles" in the left-hand menu and then click "Create role".
3. Select "AWS service" as the type of trusted entity.
4. Choose "Lambda" from the list of services and click "Next: Permissions".
5. Attach the necessary policies to allow the Lambda function to access DynamoDB. You can use the "AmazonDynamoDBFullAccess" policy for testing purposes.
6. Click "Next: Tags" and then "Next: Review".
7. Provide a name for the role (e.g., `ListAdminHotelsExecutionRole`) and click "Create role".
8. Note the ARN of the role you just created.
9. Go back to the Lambda function you created earlier.
10. In the "Configuration" tab, click on "Permissions" in the left-hand menu
11. Click on the role name under or click on edit "Execution role" to open the IAM console.
12. Click on "Attach policies" and search for the role you created earlier (e.g., `ListAdminHotelsExecutionRole`).
13. Select the role and click "Attach policy".
14. Your Lambda function now has the necessary permissions to access DynamoDB.
15. Test the API again to ensure everything is working as expected.
16. If you encounter any issues, check the CloudWatch logs for your Lambda function to debug and resolve them.
