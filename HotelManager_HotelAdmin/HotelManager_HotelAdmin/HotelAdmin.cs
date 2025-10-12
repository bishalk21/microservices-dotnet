using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using HotelManager_HotelAdmin.Models;
using HttpMultipartParser;

// using Amazon.Lambda.Serialization.SystemTextJson;   

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HotelManager_HotelAdmin;

public class HotelAdmin
{
    public async Task<APIGatewayProxyResponse> AddHotel(APIGatewayProxyRequest request, ILambdaContext context)
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
      
        await using var fileContentStream = new MemoryStream();
        await file.Data.CopyToAsync(fileContentStream);
        fileContentStream.Position = 0;
        
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

        var region  = Environment.GetEnvironmentVariable("AWS_REGION")  ?? "ap-southeast-2";
        var bucketName = Environment.GetEnvironmentVariable("bucketName");
        
        var s3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(region));
        var dbDynamoClient = new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region));

        try
        {
            await s3Client.PutObjectAsync(new PutObjectRequest
            {
                BucketName = bucketName,
                FilePath = fileName,
                InputStream = fileContentStream,
                AutoCloseStream = true,
            });

            var hotel = new Hotel
            {
                userId = userId,
                hotelId = Guid.NewGuid().ToString(),
                Name = hotelName,
                CityName = hotelCity,
                Price = int.Parse(hotelPrice),
                // Price = hotelPrice,
                // Rating = hotelRating,
                // Price = Convert.ToDecimal(hotelPrice),
                FileName = fileName,
                // Price = Decimal.Parse(hotelPrice)
                Rating = int.Parse(hotelRating)
            };
            
             using var dbContext = new DynamoDBContext(dbDynamoClient);
             await dbContext.SaveAsync(hotel);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    Console.WriteLine("OK.");
        return response;
    }
}