using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Common;

public static class S3Utils
{
    public static async Task<string?> UploadImage(IS3Service s3Service, IFormFile file, IGuid id
        , string bucketName, string folderName,
        CancellationToken cancellationToken)
    {
        var uploadKey = GetUploadKey(file, id, folderName);
        try
        {
            // var fileExists = await s3Service.CheckIfFileExists(bucketName,
            //     uploadKey, cancellationToken);
            // if (fileExists)
            // {
            //     var fileDeleted = await s3Service.DeleteFile(bucketName, uploadKey, cancellationToken);
            //     if (!fileDeleted)
            //     {
            //         Console.WriteLine($"Error deleting file. Upload Key is: {uploadKey}");
            //         return null;
            //     }
            // }

            var uploadResult =
                await s3Service.UploadFile(file, bucketName, uploadKey, cancellationToken);
            return uploadResult;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            {
                Console.WriteLine($"Error uploading cover image. Upload Key is: {uploadKey}");
                Console.WriteLine(e);
                return null;
            }
        }
    }

    public static string GetUploadKey(IFormFile file, IGuid id, string folderName)
    {
        var extension = file.FileName.Split('.')[1];
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        return $"{folderName}/{id}_{timeStamp}.{extension}";
    }
}