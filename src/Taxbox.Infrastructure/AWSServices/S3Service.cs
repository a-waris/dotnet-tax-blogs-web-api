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
    private readonly IOptions<AWSConfiguration> _appSettings;

    public S3Service(IAmazonS3 s3Client, IOptions<AWSConfiguration> appSettings)
    {
        _s3Client = s3Client;
        _appSettings = appSettings;
    }

    public async Task<string> UploadFileToS3(IFormFile? file, string uploadKey,
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
                BucketName = _appSettings.Value.S3BucketName,
                Key = uploadKey,
                InputStream = fileStream,
                ContentType = file.ContentType
            };

            var response = await _s3Client.PutObjectAsync(uploadRequest, cancellationToken);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new AmazonS3Exception("Error uploading file to S3");
            }
        }

        return $"{_appSettings.Value.S3BucketUrl}/{uploadKey}";
    }
}