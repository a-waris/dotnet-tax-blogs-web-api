using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Result<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _esService;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;

    public CreateArticleHandler(IElasticSearchService<Article> esService, IS3Service s3Service,
        IOptions<AWSConfiguration> appSettings)
    {
        _esService = esService;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }


    public async Task<Result<GetArticleResponse>> Handle(CreateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var article = request.Adapt<Article>();

        article.CreatedAt = DateTime.Now.Date;
        article.UpdatedAt = DateTime.Now.Date;

        if (request.CoverImage != null || request.ThumbnailImage != null || request.Attachments is { Count: > 0 })
        {
            if (request.CoverImage != null)
            {
                var coverImgUrl = await S3Utils.UploadImage(_s3Service, request.CoverImage, article.Id,
                    _appSettings.Value.S3BucketName,
                    $"{_appSettings.Value.S3BucketKeyForArticleIndex}/coverimages", cancellationToken);
                article.CoverImage = $"{_appSettings.Value.S3BucketUrl}/{coverImgUrl}";
            }

            if (request.ThumbnailImage != null)
            {
                var thumbnailUrl = await S3Utils.UploadImage(_s3Service, request.ThumbnailImage, article.Id,
                    _appSettings.Value.S3BucketName,
                    $"{_appSettings.Value.S3BucketKeyForArticleIndex}/thumbnailimages", cancellationToken);
                article.ThumbnailImage = $"{_appSettings.Value.S3BucketUrl}/{thumbnailUrl}";
            }


            if (request.Attachments is { Count: > 0 })
            {
                var attachments = new List<ArticleAttachment>();
                foreach (var attachment in request.Attachments)
                {
                    try
                    {
                        var attachmentUrl = await S3Utils.UploadImage(_s3Service, attachment.File, article.Id,
                            _appSettings.Value.S3BucketName,
                            $"{_appSettings.Value.S3BucketKeyForArticleIndex}/attachments", cancellationToken);
                        if (attachmentUrl == null) continue;
                        attachments.Add(new ArticleAttachment
                        {
                            File = $"{_appSettings.Value.S3BucketUrl}/{attachmentUrl}", Type = attachment.Type
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                article.Attachments = attachments;
            }
        }

        var created = await _esService.AddOrUpdate(article);
        return created.Adapt<GetArticleResponse>();
    }
}