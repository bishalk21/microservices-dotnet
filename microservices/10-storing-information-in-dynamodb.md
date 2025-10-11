# Storing Information in DynamoDB

In this section, we will explore how to store information in a DynamoDB table using the AWS SDK for .NET. We will cover the process of adding, retrieving, updating, and deleting items in the table.

1. put the code of uploading image to S3 and storing hotel information inside try/catch block to handle potential errors gracefully.
2. Add sdk of dynamodb (install)

```bash
dotnet add package AWSSDK.DynamoDBv2
```

- or in Visual Studio, right-click on the project in Solution Explorer and select "Manage NuGet Packages". Search for `AWSSDK.DynamoDBv2` and install it.

  - there are two ways of working with DynamoDB tables:
    - Document Model (low-level API): have to work with individual attributes and items, more control but more complex like dictionaries
    - Object Persistence Model (high-level API): work with .NET classes and objects, easier to use and more intuitive
  - there are two ways of working with DynamoDB in .NET:
    - low-level API: using `AmazonDynamoDBClient` and `PutItemRequest`, `GetItemRequest`, etc.
    - high-level API: using `DynamoDBContext` and data model classes with attributes.

- creating a data model class for the hotel information

  - right click on the project in Solution Explorer and select "Add" > "Directory". Name the directory `Models`.

    - right click on the `Models` directory and select "Add" > "Class". Name the class `Hotel.cs`.

    ```csharp
    // Models/Hotel.cs
    using Amazon.DynamoDBv2.DataModel;
    namespace HotelManager_HotelAdmin.Models;
    // to use the Object Persistence Model, we need to decorate the class and its properties with attributes from the `Amazon.DynamoDBv2.DataModel` namespace.
    [DynamoDBTable("Hotels")] // specify the table name
    public class Hotel
    {
        [DynamoDBHashKey("UserId")] // specify the partition key
        public string UserId { get; set; } // userId as partition key

        [DynamoDBRangeKey("HotelId")] // specify the sort key
        public string HotelId { get; set; } // hotelId as sort key

        [DynamoDBProperty("Name")]
        public string Name { get; set; }

        [DynamoDBProperty("Rating")]
        public int Rating { get; set; }

        [DynamoDBProperty("CityName")]
        public string CityName { get; set; }

        [DynamoDBProperty("Price")]
        public decimal Price { get; set; }
    }
    ```

- updating the `HotelAdmin.cs` file to store hotel information in DynamoDB after uploading the image to S3
- add hotels models to HotelAdmin.cs by instantiating the class and setting its properties
- create an instance of `AmazonDynamoDBClient` to interact with DynamoDB
- use `DynamoDBContext` to save the hotel object to the DynamoDB table

```csharp
// HotelAdmin.cs
using System.IdentityModel.Tokens.Jwt;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Net;
using System.Text;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using HttpMultipartParser;
using HotelManager_HotelAdmin.Models;

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
        var hotelRating = formData.GetParameterValue("hotelRating"); // int
        var hotelCity = formData.GetParameterValue("hotelCity");
        // var hotelPrice = formData.GetParameterValue("hotelPrice"); // decimal
        // getting error as Cannot convert source type 'decimal' to target type 'string'
        var hotelPrice = formData.GetParameterValue("hotelPrice"); // string

        var file = formData.Files.FirstOrDefault();
        var fileName = file?.Name;
        // file.data

        await using var fileContentStream = new MemoryStream(); // create a new memory stream to hold the file content
        await file.Data.CopyToAsync(fileContentStream); // copy the file content to the memory stream
        fileContentStream.Position = 0; // reset the position of the memory stream to the beginning

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

        var bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME") ?? "hotel-admin-bucket";

       var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "ap-southeast-2"; // default to ap-southeast-2 if not set
    var s3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(region));
    var dynamoDbClient = new AmazonDynamoDBClient(RegionEndpoint.GetBySystemName(region));

  try {
      await s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
    {
        BucketName = bucketName, // this is the name of the S3 bucket
        Key = fileName, // this is the name of the file to be saved in the bucket
        InputStream = fileContentStream, // this is the stream or actual file content to be uploaded
        AutoCloseStream = true, // this will close the stream after the upload is complete
        // ContentType = file?.ContentType // this is the content type of the file
    });

     var hotel = new Hotel {
        UserId = userId,
        HotelId = Guid.NewGuid().ToString(), // generate a new unique id for the hotel
        Name = hotelName,
        // Rating = int.Parse(hotelRating),
        Rating = Convert.ToInt32(hotelRating),
        CityName = hotelCity,
        // Price = decimal.Parse(hotelPrice),
        // Price = int.Parse(hotelPrice),
        // Price = Convert.ToDecimal(hotelPrice),
        Price = Decimal.Parse(hotelPrice),
        FileName = fileName
     }

     await using var context = new DynamoDBContext(dynamoDbClient);
        await context.SaveAsync(hotel);
  } catch (Exception ex) {
      Console.WriteLine($"Error uploading file to S3: {ex.Message}");
      throw;
        // context.Logger.LogError($"Error uploading file to S3: {ex.Message}");
        // response.StatusCode = (int)HttpStatusCode.InternalServerError;
        // response.Body = JsonSerializer.Serialize(new
        // {
        //     Error = "Error uploading file to S3"
        // });
        // return response;
  }


        // response.Body = JsonSerializer.Serialize(new
        // {
        //     Message = "Hotel added successfully",
        //     Hotel = new
        //     {
        //         HotelName = hotelName,
        //         HotelRating = hotelRating,
        //         HotelCity = hotelCity,
        //         HotelPrice = hotelPrice,
        //         ImageFileName = fileName,
        //         UserId = userId
        //     }
        // });


    Console.WriteLine("OK.");
        return response;
    }
}
```
