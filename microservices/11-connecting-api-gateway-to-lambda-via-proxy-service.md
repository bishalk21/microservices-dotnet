# Connecting API Gateway to Lambda via Proxy Service

In this section, we will connect the API Gateway to a Lambda function through a proxy service. This setup allows us to handle incoming API requests and route them to the appropriate Lambda function for processing.

- We need to create a proxy service that will act as an intermediary between the API Gateway and the Lambda function to ensure proper request handling and response formatting.
- The proxy service will receive requests from the API Gateway, invoke the Lambda function, and return the response back to the API Gateway.
  - This can be implemented using a simple HTTP server that listens for incoming requests and forwards them to the Lambda function.
  - like for sending headers, body, etc to the lambda function and getting the response back.

## Setting Up the Proxy Service

1.  navigate to the `Resources` section of your Amazon API Gateway in the AWS Management Console.
2.  Create a new resource or select an existing one where you want to set up the proxy.
    - set resource name as `proxy` and resource path as `{admin+}`.
    - Enable "Configure as proxy resource" option.
    - enable "CORS" option.
3.  Click on "Create Resource".
4.  Two HTTP methods will be created automatically: `ANY` and `OPTIONS`.
    - The `ANY` method will handle all types of HTTP requests (GET, POST, PUT, DELETE, etc.) and route them to the proxy service.
    - The `OPTIONS` method will handle preflight requests for CORS. The browser will send an OPTIONS request to the server to check if the actual request is safe to send, to see if the CORS headers are returned properly and browser decides whether to proceed with the actual request or not.
5.  Select the `ANY` method and configure it as follows:
    - Integration type: `Lambda Function Proxy`
    - Lambda Region: Select the region where your Lambda function is deployed (e.g., `ap-southeast-2`).
    - Lambda Function: Select your Lambda function from the dropdown (e.g., `AddHotel`).
    - Endpoint URL: `http://<proxy-service-endpoint>` (replace `<proxy-service-endpoint>` with the actual endpoint of your proxy service)
    - Use HTTP Proxy integration: checked
    - Click on "Save".
    - Set up the method request and response as needed.
      - Method Request:
        - Authorization: select `NewHotelAuthorizer` (the authorizer you created earlier).
        - API Key Required: unchecked
        - Request Validator: `Validate body, query string parameters, and headers`
6.  Select the `OPTIONS` method and configure it as follows:
    - Integration type: `MOCK`
    - add the following headers in the Method Response:
      - `Access-Control-Allow-Headers`: `*`
      - `Access-Control-Allow-Methods`: `*`
      - `Access-Control-Allow-Origin`: `*`
      ```json
      {
        "statusCode": 200,
        "headers": {
          "Access-Control-Allow-Headers": "*",
          "Access-Control-Allow-Methods": "*",
          "Access-Control-Allow-Origin": "*"
        }
      }
      ```
    - Click on "Save".
    - Set up the method request and response as needed.
      - Method Request:
        - Authorization: `NONE`
        - API Key Required: unchecked
        - Request Validator: `Validate body, query string parameters, and headers`
7.  After configuring both methods, click on "Actions" and select "Deploy API".
    - Select the deployment stage (e.g., `dev`).
    - Click on "Deploy".
8.  Note down the `Invoke URL` for the deployed API stage. This URL will be used to access the proxy service. Go to the `Stages` section in the API Gateway console, select your stage (e.g., `dev`), select the `POST`, and copy the `Invoke URL`.
    - The `Invoke URL` will look something like this: `https://<api-id>.execute-api.<region>.amazonaws.com/<stage>/proxy/{admin+}`.
    - Replace `<api-id>`, `<region>`, and `<stage>` with your actual API ID, region, and stage name.
9.  Test the setup by sending a request to the `Invoke URL` using a tool like Postman or curl.
    - Make sure to include the necessary headers and body as required by your Lambda function.
    - You should receive a response from the Lambda function via the proxy service.
10. upload the invoke URL to the frontend application to make API requests through the proxy service.

        ```html
        <form
        id="upload-form"
        enctype="multipart/form-data"
        method="post"
        action="<api url here>"
        ></form>
        ```
