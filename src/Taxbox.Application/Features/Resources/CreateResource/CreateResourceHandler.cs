using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Resources.CreateResource;

public class CreateResourceHandler : IRequestHandler<CreateResourceRequest, Result<GetResourceResponse>>
{
    private readonly IContext _context;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;


    public CreateResourceHandler(IContext context, IS3Service s3Service, IOptions<AWSConfiguration> appSettings)
    {
        _context = context;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }

    public async Task<Result<GetResourceResponse>> Handle(CreateResourceRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Resource>();
        // determine Content Type
        var contentType = request.File.ContentType;
        
        if (contentType.Contains("image"))
            created.ResourceType = ResourceType.Image;
        else if (contentType.Contains("video"))
            created.ResourceType = ResourceType.Video;
        else if (contentType.Contains("audio"))
            created.ResourceType = ResourceType.Audio;
        else
            created.ResourceType = ResourceType.Other;
        
        var resourceUrl = await S3Utils.UploadImage(_s3Service, request.File, created.Id,
            _appSettings.Value.S3BucketName,
            _appSettings.Value.S3BucketKeyForResources, cancellationToken);
        created.FileUrl = $"{_appSettings.Value.S3BucketUrl}/{resourceUrl}";

        _context.Resources.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetResourceResponse>();
    }
}