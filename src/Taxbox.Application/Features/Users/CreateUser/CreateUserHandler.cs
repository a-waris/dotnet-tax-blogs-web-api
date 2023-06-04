using Amazon.S3;
using Amazon.S3.Model;
using Ardalis.Result;
using Taxbox.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities.Common;
using BC = BCrypt.Net.BCrypt;

namespace Taxbox.Application.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, Result<GetUserResponse>>
{
    private readonly IContext _context;
    private readonly IAmazonS3 _s3Client;
    private readonly IOptions<AWSConfiguration> _appSettings;


    public CreateUserHandler(IContext context, IAmazonS3 s3Client, IOptions<AWSConfiguration> appSettings)
    {
        _context = context;
        _s3Client = s3Client;
        _appSettings = appSettings;
    }

    public async Task<Result<GetUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var created = request.Adapt<User>();
        _context.Users.Add(created);
        created.Password = BC.HashPassword(request.Password);

        var extension = request.DisplayPicture?.FileName.Split('.')[1];

        // Upload display picture to S3
        var uploadResult = await UploadDisplayPictureToS3(request.DisplayPicture, created.Id, timeStamp, extension,
            cancellationToken);
        if (!uploadResult.IsSuccess)
        {
            return Result<GetUserResponse>.Error("Error uploading display picture");
        }

        created.DisplayPicture = uploadResult.Value;

        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetUserResponse>();
    }

    private async Task<Result<string>> UploadDisplayPictureToS3(IFormFile? file, UserId userId, string timeStamp,
        string? extension, CancellationToken cancellationToken)
    {
        if (file == null)
        {
            return Result<string>.Error("No display picture provided");
        }

        var uploadKey = $"{_appSettings.Value.S3BucketKeyForProfilePictures}/{userId}_{timeStamp}.{extension}";

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
                return Result<string>.Error("Error uploading display picture");
            }
        }

        var s3ImageUrl = $"{_appSettings.Value.S3BucketUrl}/{uploadKey}";
        return Result<string>.Success(s3ImageUrl);
    }
}