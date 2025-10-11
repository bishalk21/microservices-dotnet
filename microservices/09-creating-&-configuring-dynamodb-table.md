# Creating & Configuring a DynamoDB Table

In this section, we will create and configure a DynamoDB table to store our data. DynamoDB is a fully managed NoSQL database service provided by AWS that offers fast and predictable performance with seamless scalability.

## DynamoDB (Amazon Web Services)

- fully managed NoSQL database service
- key-value and document database
- provides fast and predictable performance with seamless scalability
- delivers single-digit millisecond latency at any scale
- serverless - no servers to manage, automatically scales up and down
- designed to handle large amounts of data and high request rates
- supports both key-value and document data models
- offers built-in security, backup and restore, and in-memory caching
- information is stored in tables, which consist of items (rows) and attributes (columns)

  DynamoDB is a key-value and document database that delivers single-digit millisecond performance at any scale. It is a fully managed service that automatically handles the administrative tasks of operating and scaling a distributed database, such as hardware provisioning, setup and configuration, replication, software patching, and cluster scaling.
  DynamoDB is designed to be highly available and durable, with built-in fault tolerance and automatic data replication across multiple availability zones.
  DynamoDB supports both key-value and document data models, making it a versatile choice for a wide range of applications. It also offers features such as:

- **Global Tables**: Multi-region, fully replicated tables for high availability and low latency.
- **DynamoDB Streams**: A time-ordered sequence of item-level changes in a table, which can be used for real-time processing and triggering AWS Lambda functions.
- **Transactions**: Support for ACID transactions to ensure data integrity.
- **Fine-Grained Access Control**: Integration with AWS Identity and Access Management (IAM) for secure access to tables and items.
- **Backup and Restore**: Automated backups and point-in-time recovery for data protection.
  DynamoDB is commonly used for applications that require low-latency data access, such as gaming, IoT, mobile apps, and real-time analytics.

- there are two ways of working with DynamoDB tables:
  - Document Model (low-level API): have to work with individual attributes and items, more control but more complex like dictionaries
  - Object Persistence Model (high-level API): work with .NET classes and objects, easier to use and more intuitive
  - there are two ways of working with DynamoDB in .NET:
    - low-level API: using `AmazonDynamoDBClient` and `PutItemRequest`, `GetItemRequest`, etc.
    - high-level API: using `DynamoDBContext` and data model classes with attributes.

## Creating a DynamoDB Table

To create a DynamoDB table, follow these steps:

1. Sign in to the AWS Management Console and open the DynamoDB console at [https://console.aws.amazon.com/dynamodb/](https://console.aws.amazon.com/dynamodb/).
2. In the navigation pane, click on "Tables".
3. Click on the "Create table" button.
4. In the "Create DynamoDB table" page, provide the following details:
   - **Table name**: Enter a unique name for your table (e.g., `Hotels`).
   - **Primary key**: Define the primary key for your table. You can choose between a simple primary key (partition key) or a composite primary key (partition key and sort key). For example, you can use `userId` as the partition key and `hotelId` as the sort key.
5. Configure the table settings:
   - **Provisioned capacity**: Choose between "On-demand" or "Provisioned". On-demand is suitable for unpredictable workloads, while provisioned is better for steady workloads.
   - **Auto Scaling**: Enable auto-scaling to automatically adjust the read and write capacity based on traffic patterns.
   - **Encryption**: Choose the encryption settings for your table. You can use AWS-managed keys or customer-managed keys.
6. (Optional) Configure additional settings such as:
   - **Secondary indexes**: Create global or local secondary indexes to enable efficient queries on non-key attributes.
   - **Time to Live (TTL)**: Enable TTL to automatically delete expired items from the table.
   - **Stream settings**: Enable DynamoDB Streams to capture item-level changes in the table.
7. Review your settings and click on the "Create table" button to create the table.
   Once the table is created, you can start adding items to it and configuring additional settings as needed. You can also use the AWS SDKs or CLI to interact with your DynamoDB table programmatically.

## Configuring the DynamoDB Table

To configure your DynamoDB table, you can use the AWS Management Console, AWS SDKs, or AWS CLI. Here are some common configuration tasks:

1. **Updating Table Settings**: You can modify the table settings, such as read and write capacity, auto-scaling, and encryption, by selecting the table in the DynamoDB console and choosing the "Edit" option.

2. **Adding Secondary Indexes**: To add a global or local secondary index, go to the "Indexes" tab of your table in the DynamoDB console and click on "Create index". Follow the prompts to define the index key schema and projection settings.

3. **Enabling Time to Live (TTL)**: To enable TTL, go to the "Time to Live" tab of your table in the DynamoDB console and click on "Enable TTL". Specify the attribute that will be used to determine the expiration time for items.

4. **Configuring Streams**: To enable DynamoDB Streams, go to the "Streams" tab of your table in the DynamoDB console and click on "Enable Streams". Choose the desired stream view type (e.g., NEW_IMAGE, OLD_IMAGE) and click "Enable".

5. **Monitoring and Metrics**: Use Amazon CloudWatch to monitor the performance and usage metrics of your DynamoDB table. You can set up alarms and notifications based on specific metrics, such as read/write capacity usage and throttled requests.

By following these steps, you can effectively configure and manage your DynamoDB table to meet the needs of your application.
