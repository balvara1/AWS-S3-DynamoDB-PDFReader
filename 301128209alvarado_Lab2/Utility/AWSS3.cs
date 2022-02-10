using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301128209alvarado_Lab2.Utility
{
    public class AWSS3
    {

        private static AmazonS3Client s3Client;
        private static AmazonS3Client GetClient()
        {
            if (s3Client == null)
            {
                BasicAWSCredentials aWSCredentials = MainConfig.GetCredentials();
                s3Client = new AmazonS3Client(aWSCredentials, RegionEndpoint.USEast1);
            }

            return s3Client;
        }

        public static Task<GetObjectResponse> LoadBook(string key)
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "lab02booksbucket",
                Key = key
                
            };

            return GetClient().GetObjectAsync(request);
        }
    }
}
