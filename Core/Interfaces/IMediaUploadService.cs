using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMediaUploadService
    {
        Task<PutObjectResponse> UploadFileAsync(string key, IFormFile file);
        Task<GetObjectResponse> GetFileAsync(string key);
        Task<DeleteObjectResponse> DeleteFileAsync(string key);

    }
}
