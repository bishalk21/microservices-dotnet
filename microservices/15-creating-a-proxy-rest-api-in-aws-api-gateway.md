# Creating a Proxy REST API in AWS API Gateway

## What is a Proxy REST API?

- A Proxy REST API in AWS API Gateway is a type of API that acts as an intermediary between clients and backend services. It allows you to create a single API endpoint that can route requests to multiple backend services, such as AWS Lambda functions, HTTP endpoints, or other AWS services.
- The proxy API captures all incoming requests and forwards them to the appropriate backend service based on the request path and method. This allows you to create a unified API interface for your clients while keeping the backend services decoupled and independent.
- Proxy APIs are useful for scenarios where you want to expose multiple backend services through a single API endpoint, simplify client interactions, or implement cross-cutting concerns such as authentication, logging, and rate limiting.
- In a proxy REST API, you can define a single resource with a greedy path variable (e.g., `{proxy+}`) that captures all request paths. This allows you to forward requests to different backend services based on the captured path.

## Step 1: Create a Proxy Resource

1. Open the [API Gateway console](https://console.aws.amazon.com/apigateway).
   - Go to the API Gateway service's dashboard.
2. Click on "Create API".
3. Choose `REST API` and click `Build`.
4. Provide an API name (e.g., `ListAdminHotels`) and click "Create API".
5. From the Actions drop-down, choose Create Method and add a GET method to the API.
6. Ensure `Use Lambda Proxy integration` is enabled when creating the GET method.
7. Choose the `Lambda Function` as the integration type of the API.
8. Enable `Use Lambda Proxy integration.`
9. In the Lambda Function box, type L so the ListAdminHotels appears. Then select it.
10. Click "Save" and then "OK" to give API Gateway permission to invoke your Lambda function.
11. From the Actions drop-down button, select Enable CORS.
12. In the Enable CORS form, keep the default values and click `Enable CORS and replace existing CORS headers`.
13. Click on Resources on the left side of the screen. The GET and OPTIONS methods will appear.
14. Click on Options.
15. Click on Integration Request.
16. Expand the Mapping Templates section.
17. Click on application/json.
18. A box appears. Add the response code (200) and the CORS headers. Allow OPTIONS and GET.

    ```json
    {
      "statusCode": 200,
      "headers": {
        "Access-Control-Allow-Headers": "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token",
        "Access-Control-Allow-Methods": "OPTIONS,GET",
        "Access-Control-Allow-Origin": "*"
      }
    }
    ```

19. Click on Save.
20. Do Not configure authorization for this API.
21. Create a Proxy resource with a path like {listadminhotels+}.
22. Enable `Configure as a proxy resource` and `Enable API Gateway CORS`.
23. Set Resource Path to "{listadminhotels+}" and Resource Name to "listadminhotels".
24. Once the resource is created, click on "ANY" (under the resource path {listadminhotels+}) and connect it to the ListAdminHotels Lambda function. Then click on Save.
25. Under the resource of your method, click on ANY to see the "Method Execution" page (where four boxes are seen).
26. Click on the "Method Request" box.
27. Then, expand "URL Query String Parameters" in the Method Request page.
28. Click on the "Add query string" button.
29. Type in "token" in the name field. Then click on the tick button on the right to save.
30. Enable the "required" option.
31. Click on Authorizers on the left side of the screen
32. Click on Create New Authorizer.
33. Select Lambda as the type of authorizer.
34. Provide a name (i.e., MyLambdaAuthorizer).
35. Select Request as the Lambda event payload.
36. In Identity Sources, choose Query String and type in "token".
37. Tick the Enabled for Authorization Cache checkbox.
38. Click on OPTIONS under the created resource.
39. Make sure its integration type is MOCK and its Integration returns the HTTP 200 and the CORS headers.

    ```json
    {
      "statusCode": 200,
      "headers": {
        "Access-Control-Allow-Headers": "Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token",
        "Access-Control-Allow-Methods": "OPTIONS,GET",
        "Access-Control-Allow-Origin": "*"
      }
    }
    ```

40. Deploy your API to a new Stage (i.e., Test).
41. Make a GET request to the Invoke URL of the GET proxy resource.
