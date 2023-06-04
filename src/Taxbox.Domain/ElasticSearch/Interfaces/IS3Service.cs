using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IS3Service
{
    Task<string> UploadFileToS3(IFormFile? file, string uploadKey, CancellationToken cancellationToken);
}