using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IS3Service
{
    Task<string> UploadFile(IFormFile? file, string bucketName, string uploadKey, CancellationToken cancellationToken);
    Task<Stream> GetFile(string bucketName, string filePath, CancellationToken cancellationToken);
    Task<bool> CheckIfFileExists(string bucketName, string key, CancellationToken cancellationToken);
    Task<bool> DeleteFile(string bucketName, string key, CancellationToken cancellationToken);
}