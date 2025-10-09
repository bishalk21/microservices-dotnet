In c#, when client sends the request body of form data, it can be sent in two ways: as plain text or as a base64-encoded string. To handle both cases, we can check the `IsBase64Encoded` property of the `APIGatewayProxyRequest` object.

- if `IsBase64Encoded` is true, we decode the base64 string to get the original byte array and then convert it to a string using UTF-8 encoding.
- if `IsBase64Encoded` is false, we convert the plain text body to a byte array using UTF-8 encoding like this:

```csharp
var bodyContent:byte[]= request.IsBase64Encoded
    ? Convert.FromBase64String(request.Body)
    : Encoding.UTF8.GetBytes(request.Body);
```

This approach ensures that we correctly handle the request body regardless of its encoding, allowing us to process the data as needed in our Lambda function.

- we install HttpMultipartParser package to parse multipart form data.

```bash
dotnet add package HttpMultipartParser
```

- we can use the `HttpMultipartParser` library to parse the form data. Here's an example of how to do this in a Lambda function:

```csharp
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
```

- memoryStream is disposed automatically by using `using` statement. MemoryStream is created from the byte array `bodyContent` which contains the request body.
