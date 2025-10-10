# Creating an Execution IAM Role for Lambda

## IAM Role for Lambda

- The IAM role allows your Lambda function to assume the necessary permissions to execute and interact with other AWS services.

In this section, you will create an IAM role that your Lambda function can assume to gain the necessary permissions to execute and interact with other AWS services.

## Step 1: Create an IAM Role

1. Sign in to the [AWS Management Console](https://aws.amazon.com/console/).
2. Navigate to the **IAM** service.
3. In the left navigation pane, click on **Roles**.
4. Click the **Create role** button.
5. Select **AWS service** as the type of trusted entity.
6. Choose **Lambda** from the list of services.
7. Click **Next: Permissions**.
8. Attach the necessary policies for your Lambda function. For basic execution, you can attach the **AWSLambdaBasicExecutionRole** policy. If your Lambda function needs to access other AWS services, attach the relevant policies as well.
9. Attach **AWSLambdaExecute** policy if your Lambda function needs to read from or write to S3 buckets.
10. Attach **AmazonDynamoDBFullAccess** if your Lambda function needs to interact with DynamoDB tables.
11. Give your role a name, such as `HotelAdminLambdaExecutionRole`, and add a description if desired.
12. Click **Next: Tags** (optional) to add tags to your role.
13. Click **Create role**.

## Step 2: Verify the Role

1. After creating the role, you will be redirected to the role's summary page.
2. Verify that the correct policies are attached to the role.
3. Note the Role ARN, as you will need it when configuring your Lambda function.
4. Ensure that the trust relationship allows Lambda to assume the role. The trust policy should look like this:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "lambda.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
```

## Step 3: Use the Role in Your Lambda Function

When creating or updating your Lambda function, specify the IAM role you just created in the **Execution role** section. This will allow your Lambda function to execute with the permissions defined in the role.

1. Go to the **Lambda** service in the AWS Management Console.
2. Create a new Lambda function or select an existing one **AddHotel**.
3. In the **Configuration** tab, under **Permissions**, click on the **Edit** button.
4. Select **Use an existing role** and choose the `HotelAdminLambdaExecutionRole` from the dropdown menu.
5. Click **Save**.

Your Lambda function now has the necessary execution role to perform its tasks securely.
