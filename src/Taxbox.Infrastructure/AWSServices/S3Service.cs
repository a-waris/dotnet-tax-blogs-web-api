using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;

namespace Taxbox.Infrastructure.AWSServices;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;

    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<string> UploadFile(IFormFile? file, string bucketName, string uploadKey,
        CancellationToken cancellationToken)
    {
        if (file == null)
        {
            throw new FileNotFoundException("File not found");
        }

        await using (Stream? fileStream = file.OpenReadStream())
        {
            var uploadRequest = new PutObjectRequest
            {
                BucketName = bucketName, Key = uploadKey, InputStream = fileStream, ContentType = file.ContentType
            };

            var response = await _s3Client.PutObjectAsync(uploadRequest, cancellationToken);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new AmazonS3Exception("Error uploading file to S3");
            }
        }

        return $"{uploadKey}";
    }

    public async Task<Stream> GetFile(string bucketName, string key, CancellationToken cancellationToken)
    {
        var response = await _s3Client.GetObjectAsync(bucketName, key, cancellationToken);
        if (response.HttpStatusCode != HttpStatusCode.OK)
        {
            throw new AmazonS3Exception("Error getting file from S3");
        }

        return response.ResponseStream;
    }

    public async Task<bool> CheckIfFileExists(string bucketName, string key, CancellationToken cancellationToken)
    {
        var response = await _s3Client.GetObjectAsync(bucketName, key, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteFile(string bucketName, string key, CancellationToken cancellationToken)
    {
        var response = await _s3Client.DeleteObjectAsync(bucketName, key, cancellationToken);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }
}