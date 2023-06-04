using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

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

        if (request.CoverImage != null || request.Attachments is { Count: > 0 })
        {
            if (request.CoverImage != null)
            {
                var uploadKey = GetUploadKey(request.CoverImage, article.Id, "coverimages");
                try
                {
                    var uploadResult =
                        await _s3Service.UploadFileToS3(request.CoverImage, uploadKey, cancellationToken);
                    article.CoverImage = uploadResult;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Result<GetArticleResponse>.Error("Error uploading cover image.");
                }
            }


            if (request.Attachments is { Count: > 0 })
            {
                var attachments = new List<ArticleAttachment>();
                foreach (var attachment in request.Attachments)
                {
                    var uploadKey = GetUploadKey(attachment.File, article.Id, "attachments");
                    var uploadResult = await _s3Service.UploadFileToS3(attachment.File, uploadKey,
                        cancellationToken);
                    try
                    {
                        attachments.Add(new ArticleAttachment { File = uploadResult, Type = attachment.Type });
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

    private string GetUploadKey(IFormFile file, ArticleId id, string folderName)
    {
        var extension = file.FileName.Split('.')[1];
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        return $"{_appSettings.Value.S3BucketKeyForArticleIndex}/{folderName}/{id}_{timeStamp}.{extension}";
    }
}