using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class MediaUploadService : IMediaUploadService
    {
        private readonly IAmazonS3 _s3Client;
        private const string BucketName = "newprojectdeal-media";

        public MediaUploadService()
        {
            _s3Client = new AmazonS3Client("AKIA27DDRANTZ6ESXHMX", "hplpTDC4SfMx7ExbPcBWQLt1hoWmQMqKWdUfx4wn", Amazon.RegionEndpoint.USEast1);
        }

        public async Task<PutObjectResponse> UploadFileAsync(string key, IFormFile file, CancellationToken cancellationToken)
        {
            var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, BucketName);
            if (!bucketExists) return null;
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                Key = key,
                ContentType = file.ContentType,
                InputStream = file.OpenReadStream(),
            };
            putObjectRequest.Metadata.Add("Content-Type", file.ContentType);
            return await _s3Client.PutObjectAsync(putObjectRequest, cancellationToken);
        }

        public async Task<GetObjectResponse> GetFileAsync(string url)
        {
            try
            {
                var bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, BucketName);
                if (!bucketExists) return null;

                var fileUri = new Uri(url);
                var key = string.Concat(fileUri.Segments.Skip(0));
                var getObjectRequest = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = key
                };

                return await _s3Client.GetObjectAsync(getObjectRequest);

            } catch (AmazonS3Exception ex) when (ex.Message is "The specified key does not exist.") 
            {
                return null;
            }

        }

        public async Task<DeleteObjectResponse> DeleteFileAsync(string url, CancellationToken cancellationToken)
        {
            var fileUri = new Uri(url);
            var key = string.Concat(fileUri.Segments.Skip(1));
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = BucketName,
                Key = key
            };
            return await _s3Client.DeleteObjectAsync(deleteObjectRequest, cancellationToken);
        }
    }


}
