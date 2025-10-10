# Creating and Configuring S3 Buckets

In this section, you will create and configure S3 buckets that your application will use to store and retrieve data. S3 (Simple Storage Service) is a scalable object storage service provided by AWS.

## Step 1: Create S3 Buckets

1. Sign in to the [AWS Management Console](https://aws.amazon.com/console/).
2. Navigate to the **S3** service.
3. Click the **Create bucket** button.
4. In the **Bucket name** field, enter a unique name for your bucket (e.g., `hotel-admin-bucket`).
5. Remove the checkmark from **Block all public access** to allow public access if your application requires it. Confirm the warning if you understand the implications.
6. Click on **Acknowledge** that the bucket will be public.
7. (Optional) Configure additional settings such as versioning, tags, and encryption as needed.
8. Click **Create bucket**.
9. Repeat the process to create any additional buckets your application may need (e.g., `hotel-admin-uploads`).

## Step 2: Configure Bucket Policies

1. After creating the buckets, select the bucket you want to configure from the S3 dashboard.
   - so that not everyone can access the data, only the necessary permissions should be granted.
2. Go to the **Permissions** tab.
3. Click on **Bucket policy**.
4. Add a bucket policy to allow your application to access the bucket. Here is an example policy that grants read and write access to a specific IAM role:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Sid": "AllowEveryoneReadOnlyAccess", // to allow public read access
      "Effect": "Allow", // can be changed to "Deny" to restrict access
      "Principal": "*", // can be changed to a specific IAM role or user ARN
      "Action": ["s3:GetObject"], // can be modified to include other actions like "s3:ListBucket"
      "Resource": ["arn:aws:s3:::hotel-admin-bucket/*"] // specify the bucket ARN - "arn:aws:s3:::*"
    },
    {
      "Sid": "AllowLambdaWriteAccess", // to allow write access for Lambda functions
      "Effect": "Allow", // can be changed to "Deny" to restrict access
      "Principal": {
        "AWS": "arn:aws:iam::123456789012:role/HotelAdminLambdaExecutionRole"
      }, // can be changed to a specific IAM role or user ARN
      "Action": [
        "s3:PutObject",
        "s3:PutObjectAcl",
        "s3:GetObject",
        "s3:GetObjectVersion",
        "s3:DeleteObject",
        "s3:DeleteObjectVersion"
      ],
      "Resource": "arn:aws:s3:::hotel-admin-bucket/*" // specify the bucket ARN with wildcard for objects
    }
  ]
}
```

5. Click **Save changes**.
6. Repeat the process for any additional buckets you created.

## Step 3: Set Up CORS Configuration (if needed)

1. If your application requires cross-origin requests (e.g., if your frontend is hosted on a different domain), you need to configure CORS (Cross-Origin Resource Sharing) for your S3 buckets.
2. Go to the **Permissions** tab of your bucket.
3. Click on **CORS configuration**.
4. Add a CORS configuration similar to the following example:

```xml
<CORSConfiguration>
    <CORSRule>
        <AllowedOrigin>*</AllowedOrigin> <!-- Change to specific origin for security -->
        <AllowedMethod>GET</AllowedMethod>
        <AllowedMethod>PUT</AllowedMethod>
        <AllowedMethod>POST</AllowedMethod>
        <AllowedMethod>DELETE</AllowedMethod>
        <AllowedHeader>*</AllowedHeader>
        <MaxAgeSeconds>3000</MaxAgeSeconds>
    </CORSRule>
</CORSConfiguration>
```

5. Click **Save**.

## Step 4: Verify Bucket Configuration

1. Test the configuration by uploading and retrieving objects from the S3 buckets using your application or AWS SDKs.
2. Ensure that the permissions and access controls are functioning as expected.
3. Monitor the bucket activity using AWS CloudWatch or S3 access logs if needed.
4. Make sure to follow best practices for security and access management to protect your data.
5. Ensure that the trust relationship allows Lambda to assume the role. The trust policy should look like this:

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

6. Use the IAM role in your Lambda function to interact with the S3 buckets as needed.

## Step 5: Use the Buckets in Your Application

1. Update your application code to use the S3 buckets for storing and retrieving data.
2. Ensure that your application has the necessary permissions to access the S3 buckets as configured in the bucket policies.
3. Test the integration thoroughly to ensure that your application can successfully interact with the S3 buckets.
4. When creating or updating your Lambda function, specify the IAM role you just created in the **Execution role** section. This will allow your Lambda function to execute with the permissions defined in the role.

   1. Go to the **Lambda** service in the AWS Management Console.
   2. Create a new Lambda function or select an existing one **AddHotel**.
   3. In the **Configuration** tab, under **Permissions**, click on the **Edit** button.
   4. Select **Use an existing role** and choose the `HotelAdminLambdaExecutionRole` from the dropdown menu.
   5. Click **Save**.

5. Your Lambda function now has the necessary execution role to perform its tasks securely.\

Your application is now set up to use S3 buckets for storage, and your Lambda functions have the necessary permissions to interact with those buckets securely.
