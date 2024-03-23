using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BucketsController : BaseApiController
    {
        private readonly IAmazonS3 _s3Client;

        public BucketsController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListAsync()
        {

            //var credentials = new BasicAWSCredentials(accessKey, secretKey);
            //var s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1);
            var data = await _s3Client.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName; });
            return Ok(buckets);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
            return NoContent();
        }

    }
}
