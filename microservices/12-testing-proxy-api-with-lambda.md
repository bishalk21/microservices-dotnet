# Testing the Proxy API with Lambda

1.  Open the API Gateway console.
2.  Select the API you created for the proxy service.
3.  In the left-hand menu, click on `Resources`.
4.  Select the resource you created for the proxy (e.g., `/proxy/{admin+}`).
5.  Click on the `ANY` method.
6.  Click on the `Test` button in the top right corner.
7.  In the `Method Test` section, you can configure the request parameters:

    - `Path`: Enter a sample path that matches the `{admin+}` parameter (e.g., `admin/hotels`).
    - `Query Strings`: Add any query parameters if needed.
    - `Headers`: Add any headers required by your Lambda function (e.g., `Content-Type`, `Authorization`).
    - `Request Body`: If your Lambda function expects a body (e.g., for POST requests), enter a sample JSON payload.

8.  Click on the `Test` button at the bottom of the `Method Test` section.
9.  Review the response returned by your Lambda function in the `Response` section.

    - `Status`: Check the HTTP status code (e.g., `200 OK` for successful requests).
    - `Body`: Review the response body returned by your Lambda function.
    - `Headers`: Check the response headers.

10. If you encounter any errors, review the error message and adjust your request parameters or Lambda function as needed.

## API settings - Binary Media Types

1. From the API Gateway console, select your API.
2. In the left-hand menu, click on `API Settings`.
3. Review the settings such as `Name`, `Description`, `Endpoint Type`, and `Binary Media Types`.
4. Select the `Binary Media Types` tab.
   - Binary Media Types is used to specify the media types that should be treated as binary data. This is important for handling file uploads or downloads through the API Gateway.
5. Add any additional binary media types if needed (e.g., `image/png`, `application/pdf`).
   - if you do not see `multipart/form-data` in the list, add it by clicking on `Add Binary Media Type` and entering `multipart/form-data`.
6. Click on `Save Changes` to apply any updates to the API settings.
7. After making changes to the API settings, you may need to redeploy your API to apply the changes.
   - Click on `Actions` and select `Deploy API`.
   - Choose the deployment stage (e.g., `dev`) and click on `Deploy`.
8. Test the API again using the `Test` feature or by sending requests from a tool like Postman or curl to ensure that binary data is handled correctly.

## If any JSON parsing errors occur

1. Select the `ANY` method of your proxy resource in the API Gateway console.
2. Click on the `Integration Request` section.
3. Scroll down to the `Mapping Templates` section.
4. If there are no mapping templates defined, click on `Add mapping template`.
5. In the `Content-Type` field, enter `multipart/form-data` and click on the checkmark to save.
6. In the `Generate template` dialog, select `Method Request Passthrough` and click on `Save`.
7. In the template editor, enter the following mapping template to handle `multipart/form-data`

   ```json
    {
      "body": $input.body,
      "headers": {
         #foreach($header in $input.params().header.keySet())
         "$header": "$util.escapeJavaScript($input.params().header.get($header))"
         #if($foreach.hasNext),#end
         #end
      },
      "queryStringParameters": {
         #foreach($param in $input.params().querystring.keySet())
         "$param": "$util.escapeJavaScript($input.params().querystring.get($param))"
         #if($foreach.hasNext),#end
         #end
      },
      "pathParameters": {
         #foreach($param in $input.params().path.keySet())
         "$param": "$util.escapeJavaScript($input.params().path.get($param))"
         #if($foreach.hasNext),#end
         #end
      },
      "stageVariables": {
         #foreach($var in $stageVariables.keySet())
         "$var": "$util.escapeJavaScript($stageVariables.get($var))"
         #if($foreach.hasNext),#end
         #end
      },
      "requestContext": {
         "accountId": "$context.identity.accountId",
         "apiId": "$context.apiId",
         "authorizer": {
            #foreach($key in $context.authorizer.keySet())
            "$key": "$util.escapeJavaScript($context.authorizer.get($key))"
            #if($foreach.hasNext),#end
            #end
         },
         "httpMethod": "$context.httpMethod",
         "identity": {
            "accessKey": "$context.identity.accessKey",
            "accountId": "$context.identity.accountId",
            "apiKey": "$context.identity.apiKey",
            "caller": "$context.identity.caller",
            "cognitoAuthenticationProvider": "$context.identity.cognitoAuthenticationProvider",
            "cognitoAuthenticationType": "$context.identity.cognitoAuthenticationType",
            "cognitoIdentityId": "$context.identity.cognitoIdentityId",
            "cognitoIdentityPoolId": "$context.identity.cognitoIdentityPoolId",
            "sourceIp": "$context.identity.sourceIp",
            "user": "$context.identity.user",
            "userAgent": "$context.identity.userAgent",
            "userArn": "$context.identity.userArn"
         },
            "path": "$context.path",
            "protocol": "$context.protocol",
            "requestId": "$context.requestId",
            "requestTime": "$context.requestTime",
            "requestTimeEpoch": $context.requestTimeEpoch,
            "resourceId": "$context.resourceId",
            "resourcePath": "$context.resourcePath",
            "stage": "$context.stage"
        }
    }
   ```
