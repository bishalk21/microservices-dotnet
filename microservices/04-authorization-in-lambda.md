# Authorization in AWS Lambda

although the userId and idToken are hidden in the input fields, but they are sent in request headers with bearer token. so we can read them from the request headers.

- as we have json web token, we need to deserialize it to read the claims. we can use `System.IdentityModel.Tokens.Jwt` package to deserialize the token.

```bash
dotnet add package System.IdentityModel.Tokens.Jwt
```

- once installed, we have access to `JwtSecurityToken` class which can be used to read the claims from the token.

```csharp
// var handler = new JwtSecurityTokenHandler();
// var jsonToken = handler.ReadToken(idToken);
// var tokenS = jsonToken as JwtSecurityToken;
// var email = tokenS.Claims.First(claim => claim.Type == "email").Value;
        var token = new JwtSecurityToken(idToken); // we can also use JwtSecurityToken class directly
        var group = token.Claims.FirstOrDefault(x => x.Type == "cognito:group"); // to get the group claim

        if (group == null || group.Value != "Admin")
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            // Log the unauthorized access attempt
            // serialize the response body to json
            response.Body = JsonSerializer.Serialize(new
            {
                Error = "Unauthorized, Must be a member of Admin Group."
            });
        }
```

- we can use the email claim to identify the user and authorize the request. for example, we can check if the email is in the list of authorized users.

```csharp
var authorizedEmails = new List<string> { "user1@example.com", "user2@example.com" };
if (!authorizedEmails.Contains(email))
{
    return new APIGatewayProxyResponse
    {
        StatusCode = (int)HttpStatusCode.Unauthorized,
        Body = "Unauthorized"
    };
}
```

- if the email is not in the list, we return a 401 Unauthorized response. otherwise, we proceed with processing the request.
- we can also check if the userId from the form data matches the email claim from the token to ensure that the user is authorized to perform the action.

```csharp
if (userId != email)
{
    return new APIGatewayProxyResponse
    {
        StatusCode = (int)HttpStatusCode.Forbidden,
        Body = "Forbidden"
    };
}
```

- if the userId does not match the email claim, we return a 403 Forbidden response. otherwise, we proceed with processing the request.
- this way, we can ensure that only authorized users can perform certain actions in our Lambda function.
