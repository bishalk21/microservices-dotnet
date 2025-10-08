# Build Microservices with .NET & Amazon Web Services

## Preparing development tools and environment

1. Git Client: GitHub Desktop, Git CLI, or any Git client of your choice. [Download Git](https://git-scm.com/downloads)
2. latest version of .NET SDK [Download .NET](https://dotnet.microsoft.com/download)
3. install Visual Studio or JetBrains Rider as your Integrated Development Environment (IDE)
   - [Download Visual Studio](https://visualstudio.microsoft.com/vs/)
   - [Download JetBrains Rider](https://www.jetbrains.com/rider/download/)
4. web server
   - In Windows, install Internet Information Services (IIS) [IIS Installation Guide](https://docs.microsoft.com/en-us/iis/install/installing-iis-7/installing-iis-on-windows-vista-and-windows-7). Then, deploy the HTML code under a website in IIS.
   - On Mac, you can download and install Apache Tomcat. To install Tomcat manually, follow this link: https://serverfault.com/questions/183496/how-do-i-start-apache-tomcat-at-boot-on-mac-os-x
     Alternatively, you can use Homebrew to install Tomcat by running "brew install tomcat" in the Terminal.
   - If any of these methods are too complex, you can try a Google Chrome Extension called Web Server For Chrome, from https://bit.ly/3QJF53O
5. create a free-tier AWS account [Create AWS Account](https://aws.amazon.com/free) to deploy the microservice.
   - This account will be necessary for the deployment process.
6. remember to set up a local IAM profile (credentials) to allow the AWS CLI to deploy resources on your behalf.
   - Follow this guide to set up your credentials: [AWS CLI Configuration Guide](https://docs.aws.amazon.com/cli/latest/userguide/cli-configure-files.html)
7. install AWS CLI [Install AWS CLI](https://docs.aws.amazon.com/cli/latest/userguide/install-cliv2.html)
8. install AWS Toolkit for Visual Studio or JetBrains Rider
   - [AWS Toolkit for Visual Studio](https://aws.amazon.com/visualstudio/)
   - [AWS Toolkit for JetBrains Rider](https://plugins.jetbrains.com/plugin/11349-aws-toolkit)
9. install Postman for testing the microservice APIs [Download Postman](https://www.postman.com/downloads/)
10. (Optional) install Docker Desktop for containerizing the microservice [Download Docker Desktop](https://www.docker.com/products/docker-desktop/)
11. Depending on what framework used for the front end, you might need to install Node.js and npm.
    - [Download Node.js](https://nodejs.org/en/download/)

## AWS Cognito Setup

1. Sign in to the AWS Management Console and open the Amazon Cognito console at https://console.aws.amazon.com/cognito/.
2. Choose "Manage User Pools", and then choose "Create a user pool".
3. Choose "Application type" in Define your application settings. - `Traditional web app or mobile app`.
4. Specify name of application, e.g., `web-app`.
5. select options for sign-in identifiers, e.g., `Email address or phone number or username`.
6. select attributes to be collected from users, e.g., `email, phone_number, given_name, address, family_name`. (optional)
7. add a return url, e.g., `http://localhost:8080/hotel` (for local testing) or `http://your-domain.com` (for production).
8. click on create user directory.
9. click on go to overview.
10. from the overview, copy `user pool id` to be used in the microservice configuration. `ap-southeast-4_7Lodsfgdsfg7V`
11. update `user pool name` in the microservice configuration. `Hotel-booking-users`
12. click on applications, then click on `Client Apps` to see all the app clients created.
13. copy the `app client id` to be used in the microservice configuration. `adghsthdgfhdgfhgfhgfh`
14. click on login pages to customize the hosted UI for user sign-up and sign-in.
    - added url
    - Oauth 2.0 grant types: by default, only `Authorization code grant` is selected. You can also select `Implicit grant` if you want to use it.
    - OpenID Connect (OIDC) scopes: by default, only `openid` is selected. You can also select `email`, `phone`, and `profile` if you want to use them. Do not remove `openid` scope. Add `profile` scope to get user attributes such as given_name, family_name, etc.
15. click on save changes.
16. click on domain name to set up a custom domain for the hosted UI. You can use your own domain or use the default domain provided by AWS Cognito.
    - e.g., `your-domain.auth.ap-southeast-4.amazoncognito.com`
    - copy the domain to setup javascript sdk in the front end application. `your-domain.auth.ap-southeast-4.amazoncognito.com`
    - can create custom domain if you have your own domain name. (optional)
17. need to set up users and groups for the application.
    - click on `Users and groups` from the left menu.
    - click on `Create user` to create a new user. (optional, users can also sign up themselves via the hosted UI)
    - click on `Create group` to create a new group. (optional)
    - add users to the group. (optional)

## API Gateway (Pattern)

- stops the consumers of a microservice from directly accessing the microservice.
- simplifies the system's interface by combining multiple services into a single API.
- can handle tasks such as authentication, authorization, rate limiting, and caching.
- simplifies monitoring and logging by providing a single point of entry for all requests to the microservice.
- simplifies catalogue and documentation of the microservice APIs.

### AWS API Gateway

- https://aws.amazon.com/api-gateway/
- fully managed service.
- easy to create, publish, maintain, monitor, and secure APIs at any scale.
- part of the AWS serverless platform.
- no complex infrastructure to manage.
- needs to be created per API microservice.

### Google Apigee

- https://cloud.google.com/apigee
- enterprise-grade API management platform.
- various deployment options: cloud, on-premises, hybrid.
- hybrid cloud/on-premises deployment.
- advanced features: API analytics, developer portal, monetization, security policies.
- can manage multiple APIs from a single platform.
- costly compared to AWS API Gateway.
- complex to set up and manage.

### Kong

- https://konghq.com/
- open-source API management platform.
- enterprise version with additional features and support.
- very expensive for the enterprise version.
- high throughput and low latency.
- can be deployed on-premises or in the cloud.
- requires more setup and management compared to AWS API Gateway.

## Creating Mock API with AWS API Gateway - Rest API

1. Sign in to the AWS Management Console and open the API Gateway console at https://console.aws.amazon.com/apigateway.
2. Choose "Create API".
3. Under REST API, choose "Build".
4. For "API name", enter `HotelBookingAPI`.
5. For "Description", enter `API for Hotel Booking Microservice`, Leave endpoint type as "Regional".
6. Choose "Create API".
7. In the Resources pane, choose "Actions", and then choose "Create Resource".
8. For "Resource Name", enter `hotels`. The "Resource Path" will auto-populate.
9. Select the checkbox for "Enable API Gateway CORS".
10. Choose "Create Resource".
11. With the /hotels resource selected, choose "Actions", and then choose "Create Method".
12. From the dropdown list, choose "GET" or "POST", and then choose the checkmark icon to save.
13. In the "GET - Setup" pane, do the following:
    - For "Integration type", choose "Mock".
    - Leave the other default settings as they are, and then choose "Save".
14. In the "Method Execution" pane, choose "Method Response".
15. Expand the "200" response, and then choose "Add Response Model".
16. For "Content-Type", enter `application/json`.
17. For "Model", choose `Empty`, and then choose the checkmark icon to save.
18. Choose the back arrow to return to the "Method Execution" pane.
19. Choose "Intergration Request".
20. Expand "Mapping Templates", and then choose "Add mapping template".
21. For "Content-Type", enter `multipart/form-data`, and then choose the checkmark icon to save.
22. In the popup, choose "OK" to confirm that you want to create a new mapping template.
23. In the text area, enter the following code to extract form data:
    ```velocity
    #set($inputRoot = $input.path('$'))
    {
      "name": "$inputRoot.name",
      "location": "$inputRoot.location",
      "price": $inputRoot.price
    }
    ```
24. Choose "Save".
25. Choose "Integration Response".
26. Expand the "200" response, and then choose "Add Header".
27. For "Name", enter `Content-Type`, and then choose the checkmark icon to save.
28. In the "Header Mappings" section, choose "Add Mapping Template".
29. For "Content-Type", enter `application/json`, and then choose the checkmark icon to save.
30. In the popup, choose "OK" to confirm that you want to create a new mapping template.
31. In the text area, enter the following mock response:
    ```json
    {
      "hotels": [
        {
          "id": 1,
          "name": "Hotel A",
          "location": "City A",
          "price": 100
        },
        {
          "id": 2,
          "name": "Hotel B",
          "location": "City B",
          "price": 150
        }
      ]
    }
    ```
32. Choose "Save".
33. Choose the back arrow to return to the "Method Execution" pane.
34. Choose "Actions", and then choose "Deploy API".
35. For "Deployment stage", choose `[New Stage]`.
36. For "Stage name", enter `dev`.
37. For "Stage description", enter `Development Stage`.
38. For "Deployment description", enter `Initial deployment`.
39. Choose "Deploy".
40. The "Invoke URL" for the `dev` stage appears at the top of the page. Copy this URL to use in Postman or your web browser to test the API.
    - e.g., `https://abcde12345.execute-api.ap-southeast-2.amazonaws.com/dev/hotels`
41. Test the API by sending a GET request to the Invoke URL using Postman or your web browser.
    - You should receive the mock response defined in step 25.

### Authenticating API Requests

1. In the API Gateway console, choose your API.
2. In the left navigation pane, choose "Authorizers".
3. Choose "Create New Authorizer".
4. For "Name", enter `CognitoAuthorizer`.
5. For "Type", choose `Cognito`.
6. For "Cognito User Pool", choose the user pool you created earlier.
7. For "Token Source", enter `Authorization`.
8. Choose "Create".
9. Test the authorizer by choosing "Test" in the Authorizers pane.
   - Enter a valid JWT token from Cognito in the "Authorization" field, and then choose "Test".
   - You should see a successful response indicating that the token is valid.
10. In the left navigation pane, choose "Resources".
11. Select the `/hotels` resource.
12. Choose the method (GET or POST) you created earlier.
13. In the "Method Execution" pane, choose "Method Request".
14. Under "Settings", choose the pencil icon next to "Authorization".
15. From the dropdown list, choose `CognitoAuthorizer`, and then choose the checkmark icon to save.
16. Choose the back arrow to return to the "Method Execution" pane.
17. Choose "Actions", and then choose "Deploy API".
18. Choose the deployment stage (e.g., `dev`), and then choose "Deploy".
19. Test the API by sending a GET request to the Invoke URL using Postman or your web browser, including the `Authorization` header with a valid JWT token from Cognito.
    - You should receive the mock response defined in step 25 if the token is valid.

## CORS Configuration

1. In the API Gateway console, choose your API.
2. In the left navigation pane, choose "Resources".
3. Select the `/hotels` resource.
4. Choose the method (GET or POST) you created earlier.
5. In the "Method Execution" pane, choose "Method Response".
6. Expand the "200" response, and then choose "Add Response Header".
7. For "Name", enter `Access-Control-Allow-Origin`, and then choose the checkmark icon to save.
8. Choose "Add Response Header" again.
9. For "Name", enter `Access-Control-Allow-Headers`, and then choose the checkmark icon to save.
10. Choose "Add Response Header" again.
11. For "Name", enter `Access-Control-Allow-Methods`, and then choose the checkmark icon to save.
12. Choose the back arrow to return to the "Method Execution" pane.
13. Choose "Integration Response".
14. Expand the "200" response, and then choose "Add Header".
15. For "Name", enter `Access-Control-Allow-Origin`, and then choose the checkmark icon to save.
16. In the "Header Mappings" section, choose "Add Mapping Template".
17. For "Content-Type", enter `application/json`, and then choose the checkmark icon to save.
18. In the text area, enter `'*'`, and then choose "Save".
19. Choose "Add Header" again.
20. For "Name", enter `Access-Control-Allow-Headers`, and then choose the checkmark icon to save.
21. In the "Header Mappings" section, choose "Add Mapping Template".
22. For "Content-Type", enter `application/json`, and then choose the checkmark icon to save.
23. In the text area, enter `'Content-Type,X-Amz-Date,Authorization,X-Api-Key,X-Amz-Security-Token'`, and then choose "Save".
24. Choose "Add Header" again.
25. For "Name", enter `Access-Control-Allow-Methods`, and then choose the checkmark icon to save.
26. In the "Header Mappings" section, choose "Add Mapping Template".
27. For "Content-Type", enter `application/json`, and then choose the checkmark icon to save.
28. In the text area, enter `'GET,POST,OPTIONS'`, and then choose "Save".
29. Choose the back arrow to return to the "Method Execution" pane.
30. Choose "Actions", and then choose "Deploy API".
31. Choose the deployment stage (e.g., `dev`), and then choose "Deploy".
32. Test the API by sending a GET request to the Invoke URL using Postman or your web
    browser, including the `Authorization` header with a valid JWT token from Cognito.
    - You should receive the mock response defined in step 25 if the token is valid.
    - The response headers should include the CORS headers you configured.
