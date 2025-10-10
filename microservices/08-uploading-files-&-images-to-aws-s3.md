# Uploading Files and Images to AWS S3

In this section, you will learn how to upload files and images to an AWS S3 bucket using a Lambda function. This is particularly useful for applications that require file storage, such as user profile pictures, documents, or any other media files.

## Prerequisites

- An AWS account with the necessary permissions to create and manage S3 buckets and Lambda functions.
- An S3 bucket created for storing the files and images.
- An IAM role with the necessary permissions for the Lambda function to access the S3 bucket.
- Basic knowledge of AWS Lambda and S3 services.
- AWS SDK for your programming language of choice (e.g., Boto3 for Python, AWS SDK for JavaScript).
- A development environment set up for writing and deploying Lambda functions.
- Ensure that your Lambda function has the necessary execution role with permissions to interact with S3. You can refer to the [Creating an Execution IAM Role for Lambda](06-creating-an-execution-iam-role-for-lambda.md) guide for detailed steps on setting up the IAM role.
- Ensure that your S3 bucket is properly configured with the necessary permissions and CORS settings. You can refer to the [Creating and Configuring S3 Buckets](07-create-%26-configure-s3-buckets.md) guide for detailed steps on setting up the S3 bucket.
- Ensure that your `.env` file contains the correct AWS account ID and IAM role ARN. For example:
  ```
  AWS_ACCOUNT_ID=64240234654589
  IAM_ARN=arn:aws:iam::64240234654589:role/HotelAdminLambdaExecutionRole
  ```

1. **Set Up Your Lambda Function**
2. **Install AWS SDK**

- In IDE or terminal, install the AWS SDK for your programming language. For example, in Node.js, you can use npm:

  ```bash
  npm install aws-sdk
  ```

- using nuget package manager for .net

  ```bash
  Install-Package AWSSDK.S3
  ```

- search for **awssdk.s3** in nuget package manager in visual studio and install the package.

- Instantiate Amazon S3 client to interact with S3 service.

```csharp
using Amazon.S3;

var s3Client = new AmazonS3Client();
```

    - AmazonS3Client has multiple constructors, you can pass in parameters like region, credentials, etc. based on your requirements.

- we need pass `region` endpoint while creating the instance of `AmazonS3Client`
- so in order to read the region from environment variable, we can use `Amazon.RegionEndpoint` class to get the region endpoint.

```csharp
using Amazon;
using Amazon.S3;

var region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "ap-southeast-2"; // default to ap-southeast-2 if not set
var s3Client = new AmazonS3Client(RegionEndpoint.GetBySystemName(region));
```

- the client then using the `PutObjectAsync` method to upload the file to the specified S3 bucket.

  - The `PutObjectRequest` object contains the details of the upload, including the bucket name, key (file name), file path, and content type.
  - since we are using `PutObjectAsync` method, we need to make the method `async` and use `await` keyword while calling the method.

  ```csharp
   public async Task<APIGatewayProxyResponse> AddHotel(APIGatewayProxyRequest request, ILambdaContext context)
  ```

```csharp
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

    await s3Client.PutObjectAsync(new Amazon.S3.Model.PutObjectRequest
    {
        BucketName = bucketName, // this is the name of the S3 bucket
        Key = fileName, // this is the name of the file to be saved in the bucket
        InputStream = fileContentStream, // this is the stream or actual file content to be uploaded
        AutoCloseStream = true, // this will close the stream after the upload is complete
        // ContentType = file?.ContentType // this is the content type of the file
    });

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

- In the above code, we read the file content from the multipart form data and copy it to a memory stream. We then use the `PutObjectAsync` method of the `AmazonS3Client` to upload the file to the specified S3 bucket.
- go back to the AWS Management Console and navigate to the Lambda service.
- Select your Lambda function (e.g., `AddHotel`).
- In the **Configuration** tab, under **Environment variables**, add the following environment variables:
  - `S3_BUCKET_NAME`: The name of your S3 bucket (e.g., `hotel-admin-bucket`).
  - `AWS_REGION`: The AWS region where your S3 bucket is located (e.g., `ap-southeast-2`).
- Click **Save** to apply the changes.
- Test the Lambda function by uploading a file through your application or using the AWS Lambda console's test feature.
  - using AWS Lambda console's test feature, you can create a test event with the necessary parameters and file data in the request body.
  - Make sure to include the `Content-Type: multipart/form-data` header in your test event.
  ```json
  {
    "key1": "value1",
    "key2": "value2",
    "key3": "value3"
  }
  ```
- Monitor the Lambda function's logs in Amazon CloudWatch to ensure that the file upload is successful and to troubleshoot any issues that may arise.
