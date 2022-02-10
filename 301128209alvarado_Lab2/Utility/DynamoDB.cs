using _301128209alvarado_Lab2.Models;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace _301128209alvarado_Lab2.Utility
{
    public class DynamoDB
    {
        private static AmazonDynamoDBClient dynamoDBClient;
        public static string UsersTable = "Users";

        // for dynamoDB client
        private static AmazonDynamoDBClient GetClient()
        {
            if (dynamoDBClient == null)
            {
                BasicAWSCredentials aWSCredentials = MainConfig.GetCredentials();
                dynamoDBClient = new AmazonDynamoDBClient(aWSCredentials, RegionEndpoint.USEast1);
            }
            
            return dynamoDBClient;
        }

        public static async void CreateTable()
        {
            CreateTableRequest req = new CreateTableRequest
            {
                TableName = UsersTable,
                AttributeDefinitions = new List<AttributeDefinition>
                {
                    new AttributeDefinition
                    {
                        AttributeName = "username",
                        AttributeType = "S"
                    }
               },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "username",
                        KeyType = "Hash"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 2,
                    WriteCapacityUnits = 1
                }
            };

            try
            {
                await GetClient().CreateTableAsync(req).ContinueWith(task =>
                {
                    LoadUsers();
                });
            }
            catch(AmazonDynamoDBException exception)
            {
                Console.WriteLine("An error occurred on the server side " + exception.Message);
            }
        }

        private static async void LoadUsers()
        {
            // add 3 users
            try
            {
                await AddNewUser("user1", "user1");
                await AddNewUser("user2", "user2");
                await AddNewUser("user3", "user3");

                
            }
            catch (AmazonDynamoDBException exception)
            {
                Console.WriteLine("An error occurred on the server side " + exception.Message);
            }
        }

        public static Task<PutItemResponse> AddNewUser(string username, string password)
        {
            // Create the put item request to submit to DynamoDB
            PutItemRequest req = new PutItemRequest
            {
                TableName = UsersTable,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"username", new AttributeValue{S=username} },
                    {"password", new AttributeValue{S=GetHash(password) } }
                    //{"password", new AttributeValue{S=password} }
                }
            };

            return GetClient().PutItemAsync(req);
        }

        // In the case of the password, we need to use a hash function to conceal the actual password
        // so it does not appear as an item entry in the DynamoDB Users table
        private static string GetHash(string message)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(message));

                StringBuilder builder = new StringBuilder();
                foreach (byte info in bytes)
                {
                    builder.Append(info.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // output of ListTables operation
        public static Task<ListTablesResponse> ListTables()
        {
            return GetClient().ListTablesAsync();
        }

        public static Task<QueryResponse> UserLogin(string username, string password)
        {
            // query for user login
            QueryRequest req = new QueryRequest
            {
                TableName = UsersTable,
                // set username and password
                KeyConditionExpression = "username = :v_username",
                FilterExpression = "password = :v_password",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    { ":v_username", new AttributeValue{ S = username} },
                    { ":v_password", new AttributeValue{ S = GetHash(password)} }
                },
            };
            return GetClient().QueryAsync(req);
        }

        public static Task<List<BookItem>> GetBooks(string username)
        {
            
            //MessageBox.Show("DynamoDB Util - GetBooks! " + username);
            DynamoDBContext context = new DynamoDBContext(GetClient());
            return context.QueryAsync<BookItem>(username).GetRemainingAsync();
        }

        public static Task UpdateBook(BookItem book)
        {
            DynamoDBContext context = new DynamoDBContext(GetClient());
            return context.SaveAsync<BookItem>(book);
        }
    }
}
