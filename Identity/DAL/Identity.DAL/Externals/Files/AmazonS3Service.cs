using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Identity.BLL.Abstractions.Externals;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Identity.DAL.Implementations.Externals.Files
{
    public sealed class AmazonS3Service : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        public AmazonS3Service(IConfiguration configuration)
        {
            _configuration = configuration;

            var accessKey = Environment.GetEnvironmentVariable("AWS__AccessKey");
            var secretKey = Environment.GetEnvironmentVariable("AWS__SecretKey");
            var region = _configuration["AWS:Region"];
            _bucketName = _configuration["AWS:BucketName"];

            var awsCredentials = new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey);
            _s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.EUCentral1);
        }
        public async Task RemoveFileAsync(string fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
                throw new ArgumentNullException("File url cannot be empty");
            var deletedFile = new DeleteObjectRequest()
            {
                BucketName = _bucketName,
                Key = fileKey
            };
            await _s3Client.DeleteObjectAsync(deletedFile);
        }

        public async Task<string> UpdateFileAsync(IFormFile file, string fileKey,string folder)
        {
            if (string.IsNullOrEmpty(fileKey))
                throw new ArgumentNullException("File cannot be empty");
            string newUrl = await UploadFileAsync(file, folder);
            if(fileKey is not null)
               await RemoveFileAsync(fileKey);
            return newUrl;
        }

        public async Task<string> UploadFileAsync(IFormFile file,string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var key = $"{folder}/{Guid.NewGuid()}_{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = stream,
                    Key = key,
                    BucketName = _bucketName,

                };

                var transferUtility = new TransferUtility(_s3Client);
                await transferUtility.UploadAsync(uploadRequest);
            }

            var url = $"https://{_bucketName}.s3.amazonaws.com/{key}";
            return url;
        }
    }
}
