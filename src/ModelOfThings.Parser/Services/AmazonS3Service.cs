using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ModelOfThings.Parser.Services
{
    public class AmazonS3Service
    {
        private readonly IAmazonS3 _amazonS3;


        private const string FilePath = @"C:\Users\userName\Documents\workspace\amazon\test.txt";

        private const string UploadWithKeyName = @"UploadWithKeyName";
        private const string FileStreamUpload = @"FileStreamUpload";
        private const string AdvancedUpload = @"func130287/flow.json";

        // You need to manually create this directory in your OS, or change the path
        private const string PathAndFileName = @"C:\S3Temp\";

        public AmazonS3Service()
        {
            _amazonS3 = new AmazonS3Client("AKIAICDDFQBHCFAIVNUA", "0sZ76/TZtFfnr1m1zrFiCXTjsjh+dmGWbH8vf5cc", RegionEndpoint.USEast2);
        }

        public async Task CreateBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_amazonS3, bucketName) == false)
                {
                    var putBucketRequest = new PutBucketRequest
                    {
                        BucketName = bucketName
                    };
                    await _amazonS3.PutBucketAsync(putBucketRequest);
                }
            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                throw e;
            }

            // Catch other errors
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///     Deleting a bucket, sending the Bucket Name
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <returns></returns>
        public async Task DeleteBucketAsync(string bucketName)
        {
            try
            {
                if (await AmazonS3Util.DoesS3BucketExistAsync(_amazonS3, bucketName))
                {
                    // Delete the bucket and return the response
                    var response = await _amazonS3.DeleteBucketAsync(bucketName);
                }
            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                throw e;
            }

            // Catch other errors
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///     Upload a file to an S3, here four files are uploaded in four different ways
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <returns></returns>
        public async Task UploadFileAsync(string bucketName, Stream fileStream, string lambda)
        {
            try
            {
                var fileTransferUtility = new TransferUtility(_amazonS3);

                // Option 4 (Upload and create the file in the process)
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = bucketName,
                    InputStream = fileStream,
                    StorageClass = S3StorageClass.Standard,
                    PartSize = 6291456,
                    Key = $@"{lambda}/flow.json",
                    CannedACL = S3CannedACL.PublicReadWrite
                };
                //fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                //fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                throw e;
            }

            // Catch other errors
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///     Get a file from S3
        /// </summary>
        /// <param name="bucketName">Bucket where the file is stored</param>
        /// <param name="keyName">Key name of the bucket (File Name)</param>
        /// <returns></returns>
        public async Task GetObjectFromS3Async(string bucketName, string keyName)
        {
            if (string.IsNullOrEmpty(keyName)) keyName = "test.txt";

            try
            {
                // Build the request with the bucket name and the keyName (name of the file)
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };
                string responseBody;

                using (var response = await _amazonS3.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    var title = response.Metadata["x-amz-meta-title"];
                    var contentType = response.Headers["Content-Type"];

                    Console.WriteLine($"Object meta, Title: {title}");
                    Console.WriteLine($"Content type, Title: {contentType}");
                    responseBody = reader.ReadToEnd();
                }

                var createText = responseBody;
                File.WriteAllText(PathAndFileName + keyName, createText);

            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                throw e;
            }

            // Catch other errors
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///     Delete a file from an S3
        /// </summary>
        /// <param name="bucketName">Bucket where file is stored</param>
        /// <param name="keyName">Key name of the file</param>
        /// <returns></returns>
        public async Task DeleteObjectFromS3Async(string bucketName, string keyName)
        {
            if (string.IsNullOrEmpty(keyName)) keyName = "test.txt";

            try
            {
                // Build the request with the bucket name and the keyName (name of the file)
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName
                };

                await _amazonS3.GetObjectAsync(request);

                await _amazonS3.DeleteObjectAsync(bucketName, keyName);

            }

            // Catch specific amazon errors
            catch (AmazonS3Exception e)
            {
                throw e;
            }

            // Catch other errors
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
