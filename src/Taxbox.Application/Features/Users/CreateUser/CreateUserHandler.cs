using Ardalis.Result;
using Taxbox.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Taxbox.Application.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, Result<GetUserResponse>>
{
    private readonly IContext _context;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;


    public CreateUserHandler(IContext context, IS3Service s3Service, IOptions<AWSConfiguration> appSettings)
    {
        _context = context;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }

    public async Task<Result<GetUserResponse>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var timeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
        var created = request.Adapt<User>();
        _context.Users.Add(created);
        created.Password = BC.HashPassword(request.Password);
        if (created.DisplayPicture != null)
        {
            var extension = request.DisplayPicture?.FileName.Split('.')[1];
            var uploadKey = $"{_appSettings.Value.S3BucketKeyForProfilePictures}/{created.Id}_{timeStamp}.{extension}";
            // Upload display picture to S3
            try
            {
                var uploadResult =
                    await _s3Service.UploadFileToS3(request.DisplayPicture, uploadKey, cancellationToken);
                created.DisplayPicture = uploadResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new Exception("Error uploading display picture");
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetUserResponse>();
    }
}