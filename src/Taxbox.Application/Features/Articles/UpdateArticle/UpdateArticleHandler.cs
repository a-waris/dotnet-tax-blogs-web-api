using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.UpdateArticle;

public class UpdateArticleHandler : IRequestHandler<UpdateArticleRequest, Result<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _eSservice;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;

    public UpdateArticleHandler(IElasticSearchService<Article> eSservice, IS3Service s3Service,
        IOptions<AWSConfiguration> appSettings)
    {
        _eSservice = eSservice;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }


    public async Task<Result<GetArticleResponse>> Handle(UpdateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var article = request.Adapt<Article>();

        var existingArticle = await _eSservice.Get(request.Id.Adapt<string>());
        if (existingArticle.Source == null)
        {
            return Result.NotFound();
        }

        article.UpdatedAt = DateTime.UtcNow;


        if (request.CoverImage != null || request.ThumbnailImage != null || request.Attachments is { Count: > 0 })
        {
            if (request.CoverImage != null)
            {
                if (!string.IsNullOrEmpty(existingArticle.Source.CoverImage))
                {
                    var uploadKey = existingArticle.Source.CoverImage?.Split(_appSettings.Value.S3BucketName)[1];

                    if (uploadKey == null) return Result.Error("Error uploading cover image");
                    var fileExists = await _s3Service.CheckIfFileExists(_appSettings.Value.S3BucketName,
                        uploadKey, cancellationToken);
                    if (fileExists)
                    {
                        await _s3Service.DeleteFile(_appSettings.Value.S3BucketName, uploadKey, cancellationToken);
                    }
                }

                var coverImgUrl = await S3Utils.UploadImage(_s3Service, request.CoverImage, article.Id,
                    _appSettings.Value.S3BucketName,
                    $"{_appSettings.Value.S3BucketKeyForArticleIndex}/coverimages", cancellationToken);
                article.CoverImage = $"{_appSettings.Value.S3BucketUrl}/{coverImgUrl}";
            }

            if (request.ThumbnailImage != null)
            {
                if (!string.IsNullOrEmpty(existingArticle.Source.ThumbnailImage))
                {
                    var uploadKey = existingArticle.Source.ThumbnailImage?.Split(_appSettings.Value.S3BucketName)[1];

                    if (uploadKey == null) return Result.Error("Error uploading thumbnail image");
                    var fileExists = await _s3Service.CheckIfFileExists(_appSettings.Value.S3BucketName,
                        uploadKey, cancellationToken);
                    if (fileExists)
                    {
                        await _s3Service.DeleteFile(_appSettings.Value.S3BucketName, uploadKey, cancellationToken);
                    }
                }

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
                        var fileName = attachment.File.FileName;

                        //check if fileName exists in existing attachments
                        var existingAttachment = existingArticle.Source.Attachments?.FirstOrDefault(x =>
                            x.File.Split(_appSettings.Value.S3BucketName)[1] == fileName);
                        if (existingAttachment != null)
                        {
                            var uploadKey = existingAttachment?.File.Split(_appSettings.Value.S3BucketName)[1];
                            if (uploadKey == null)
                            {
                                Console.WriteLine("Error deleting existing attachment");
                                continue;
                            }

                            var fileExists = await _s3Service.CheckIfFileExists(
                                _appSettings.Value.S3BucketName,
                                uploadKey, cancellationToken);
                            if (fileExists)
                            {
                                await _s3Service.DeleteFile(_appSettings.Value.S3BucketName, uploadKey,
                                    cancellationToken);
                            }
                        }

                        var attachmentUrl = await S3Utils.UploadImage(_s3Service, attachment.File, article.Id,
                            _appSettings.Value.S3BucketName,
                            $"{_appSettings.Value.S3BucketKeyForArticleIndex}/attachments", cancellationToken);
                        if (attachmentUrl == null) continue;
                        attachments.Add(new ArticleAttachment { File =  $"{_appSettings.Value.S3BucketUrl}/{attachmentUrl}", Type = attachment.Type });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                article.Attachments = attachments;
            }
        }


        var result = await _eSservice.AddOrUpdate(request.Adapt(existingArticle.Source));
        return result.Adapt<GetArticleResponse>();
    }
}