using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;

namespace Taxbox.Application.Features.Resources.UpdateResource;

public class UpdateResourceHandler : IRequestHandler<UpdateResourceRequest, Result<GetResourceResponse>>
{
    private readonly IContext _context;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;

    public UpdateResourceHandler(IContext context, IS3Service s3Service, IOptions<AWSConfiguration> appSettings)
    {
        _context = context;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }

    public async Task<Result<GetResourceResponse>> Handle(UpdateResourceRequest request,
        CancellationToken cancellationToken)
    {
        var originalResource = await _context.Resources
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (originalResource == null) return Result.NotFound();

        originalResource.DisplayName = request.DisplayName;
        originalResource.ResourceType = request.ResourceType;
        originalResource.CategoryId = request.CategoryId;

        if (request.File != null)
        {
            if (!string.IsNullOrEmpty(originalResource.FileUrl))
            {
                var uploadKey = originalResource.FileUrl.Split(_appSettings.Value.S3BucketName)[1];

                if (string.IsNullOrEmpty(uploadKey)) return Result.Error("Error uploading resource.");
                var fileExists = await _s3Service.CheckIfFileExists(_appSettings.Value.S3BucketName,
                    uploadKey, cancellationToken);
                if (fileExists)
                {
                    await _s3Service.DeleteFile(_appSettings.Value.S3BucketName, uploadKey, cancellationToken);
                }
            }

            var resourceUrl = await S3Utils.UploadImage(_s3Service, request.File, originalResource.Id,
                _appSettings.Value.S3BucketName,
                _appSettings.Value.S3BucketKeyForResources, cancellationToken);
            originalResource.FileUrl = $"{_appSettings.Value.S3BucketUrl}/{resourceUrl}";
        }

        _context.Resources.Update(originalResource);
        await _context.SaveChangesAsync(cancellationToken);
        return originalResource.Adapt<GetResourceResponse>();
    }
}