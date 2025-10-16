# Protecting GET API with Lambda Authorizers

## Step 1: Create a Lambda Authorizer project/solution using .NET 8

1. Open your terminal or command prompt.
2. Create a new directory for your Lambda Authorizer project:
   ```bash
   mkdir LambdaAuthorizer
   cd LambdaAuthorizer
   ```
3. Create a new .NET 8 Lambda project:

   ```bash
   dotnet new lambda.EmptyFunction --name LambdaAuthorizer
   ```

4. Open the project in your favorite IDE (e.g., Visual Studio, VS Code).
   Or

5. Open IDE (e.g., Visual Studio, Rider).
6. Create a new project.
7. Name it `LambdaAuthorizer`.
8. Choose`.NET 8` and `AWS Lambda Empty Function` template.
9. Click `Create`.
10. Add the following NuGet packages to your project:
    - `Amazon.Lambda.APIGatewayEvents`: to work with API Gateway events.
    - `Amazon.Lambda.Core`: to turn your class into a Lambda function.
    - `Newtonsoft.Json`: for JSON serialization and deserialization.
    - `AWSSDK.secretsmanager`: to access AWS Secrets Manager.
    - `System.IdentityModel.Tokens.Jwt`: for JWT token handling.
11. You can add these packages via the NuGet Package Manager in your IDE or by running the following commands in the terminal:

    ```bash
    dotnet add package Amazon.Lambda.APIGatewayEvents
    dotnet add package Amazon.Lambda.Core
    dotnet add package Newtonsoft.Json
    dotnet add package AWSSDK.SecretsManager
    dotnet add package System.IdentityModel.Tokens.Jwt
    ```

12. Replace the content of `Function.cs` with the following code:

    ```csharp
    using System.IdentityModel.Tokens.Jwt;
    using Amazon.Lambda.APIGatewayEvents;
    using Amazon.SecretsManager;
    using Amazon.SecretsManager.Model;

    namespace LamdaAuthorizers;

    public class Authorizer
    {
        // 1.  Handler method to be invoked by AWS Lambda
        public async Task<APIGatewayCustomAuthorizerResponse> Auth(APIGatewayCustomAuthorizerRequest request)
        {
            // 2.  Create a response object to return the authorization result
            var response = new APIGatewayCustomAuthorizerResponse();

            // 3.  Extract the token from the request to validate
            var idToken = request.AuthorizationToken;
            var idTokenDetails = new JwtSecurityToken(idToken);

            // 4.  Extract the kid, issuer, and audience from the token claims to validate the token
            var kid = idTokenDetails.Header["kid"].ToString();
            var issuer = idTokenDetails.Claims.First(x => x.Type == "iss").Value;
            var audience = idTokenDetails.Claims.First(x => x.Type == "aud").Value;

            // 5.  Fetch the secret from AWS Secrets Manager to validate the token
            var secretsClient = new AmazonSecretsManagerClient();
            var secret = await secretsClient.GetSecretValueAsync(new GetSecretValueRequest
            {
                // SecretId = idTokenDetails.Claims.First(x => x.Type == "secret").Value,
                SecretId = "hotelCognitoKeys"
            });
            var privateKeys = secret.SecretString;

            // 6. deserialize the secret to get the keys
            var jwks = JsonSerializer.Deserialize<JsonWebKeySet>(privateKeys, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // 7.  Find the key that matches the kid from the token
            var key = jwks.Keys.First(x => x.Kid == kid);

            // 8.  Create a SecurityKey from the key to validate the token
            var handler = new JwtSecurityTokenHandler();
            var result = await handler.ValidateTokenAsync(idToken, new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidIssuer = issuer,
                ValidAudience = audience
            });

            // 9.  If the token is valid, return an Allow policy, otherwise return a Deny policy
            if (!result.IsValid)
            {
                // response.StatusCode = 401;
                // response.PrincipalId = "user|unauthorized";
                // return response;

                throw new UnauthorizedAccessException("Invalid token");
            }

            // 10.  api group mapping to cognito groups for authorization
            // if the api contains listadminhotels then the user must be in Admin group
            var apiGroupMapping = new Dictionary<string, string>()
            {
                { "listadminhotels+", "Admin" },
                { "admin+", "Admins" }
            };

            // 11.  Check if the user is in the required group for the api
            var expectedGroup = apiGroupMapping.FirstOrDefault(x =>
                request.Path.Contains(x.Key, StringComparison.InvariantCultureIgnoreCase));
            if (!expectedGroup.Equals(default(KeyValuePair<string, string>)))
            {
                var userGroup = idTokenDetails.Claims.First(x=> x.Type == "cognito:groups").Value;
                if (string.Compare(userGroup, expectedGroup.Value, StringComparison.InvariantCultureIgnoreCase) != 0)
                {
                    // user is not authorized
                }
            }

            return response;
        }
    }
    ```
