using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using HttpMultipartParser;

// using Amazon.Lambda.Serialization.SystemTextJson;   

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HotelManager_HotelAdmin;

public class HotelAdmin
{
    public APIGatewayProxyResponse AddHotel(APIGatewayProxyRequest request, ILambdaContext context)
    {
        var response = new APIGatewayProxyResponse()
        {
            Headers = new Dictionary<string, string>(),
            StatusCode = 200,
        };

        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Access-Control-Allow-Headers", "*");
        response.Headers.Add("Access-Control-Allow-Methods", "OPTIONS,POST");

        var bodyContent = request.IsBase64Encoded
            ? Convert.FromBase64String(request.Body)
            : Encoding.UTF8.GetBytes(request.Body);

        using var memoryStream = new MemoryStream(bodyContent);

        var formData = MultipartFormDataParser.Parse(memoryStream);

        var hotelName = formData.GetParameterValue(name: "hotelName");
        var hotelRating = formData.GetParameterValue("hotelRating");
        var hotelCity = formData.GetParameterValue("hotelCity");
        var hotelPrice = formData.GetParameterValue("hotelPrice");

        var file = formData.Files.FirstOrDefault();
        var fileName = file?.Name;
        // file.data

        var userId = formData.GetParameterValue("userId");
        var idToken = formData.GetParameterValue("idToken");

        // request.Headers("Authorization")
        var token = new JwtSecurityToken(idToken);
        var group = token.Claims.FirstOrDefault(x => x.Type == "cognito:group");

        if (group == null || group.Value != "Admin")
        {
            response.StatusCode = (int)HttpStatusCode.Unauthorized;
            response.Body = JsonSerializer.Serialize(new
            {
                Error = "Unauthorized, Must be a member of Admin Group."
            });
        }

    Console.WriteLine("OK.");
        return response;
    }
}