using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301128209alvarado_Lab2.Utility
{
    public class MainConfig
    {

        public static BasicAWSCredentials credentials;

        public static BasicAWSCredentials GetCredentials()
        {
            // this method is to avoid recreating the basic AWS credentials
            if (credentials == null)
            {
                var builder = new ConfigurationBuilder()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

                var Configuration = builder.Build();
                String accessKeyID = Configuration.GetSection("AWSCredentials").GetSection("AccessKeyID").Value;
                String secretKey = Configuration.GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

                credentials = new BasicAWSCredentials(accessKeyID, secretKey);
            }

            return credentials;
        }
    }
}
