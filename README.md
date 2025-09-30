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
