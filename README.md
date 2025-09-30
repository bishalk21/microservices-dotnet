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
