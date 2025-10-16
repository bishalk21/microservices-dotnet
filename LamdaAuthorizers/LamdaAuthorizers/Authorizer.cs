using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.IdentityModel.Tokens;

namespace LamdaAuthorizers;

public class Authorizer
{
    public async Task<APIGatewayCustomAuthorizerResponse> Auth(APIGatewayCustomAuthorizerRequest request)
    {

        var idToken = request.AuthorizationToken;
        var idTokenDetails = new JwtSecurityToken(idToken);

        var kid = idTokenDetails.Header["kid"].ToString();
        var issuer = idTokenDetails.Claims.First(x => x.Type == "iss").Value;
        var audience = idTokenDetails.Claims.First(x => x.Type == "aud").Value;

        var response = new APIGatewayCustomAuthorizerResponse();
        
        var secretsClient = new AmazonSecretsManagerClient();
        var secret = await secretsClient.GetSecretValueAsync(new GetSecretValueRequest
        {
            // SecretId = idTokenDetails.Claims.First(x => x.Type == "secret").Value,
            SecretId = "hotelCognitoKeys"
        });
        var privateKeys = secret.SecretString;

        var jwks = JsonSerializer.Deserialize<JsonWebKeySet>(privateKeys, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
        var privateKey = jwks.Keys.First(x => x.Kid == kid);

        var handler = new JwtSecurityTokenHandler();
        var result = await handler.ValidateTokenAsync(idToken, new TokenValidationParameters
        {
            IssuerSigningKey = privateKey,
            ValidIssuer = issuer,
            ValidAudience = audience
        });

        if (!result.IsValid)
        {
            // response.StatusCode = 401;
            // response.PrincipalId = "user|unauthorized";
            // return response;

            throw new UnauthorizedAccessException("Invalid token");
        }

        var apiGroupMapping = new Dictionary<string, string>()
        {
            { "listadminhotels+", "Admin" },
            { "admin+", "Admins" }
        };

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