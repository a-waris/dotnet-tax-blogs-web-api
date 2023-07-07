using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace Taxbox.Application.Features.Users.UpdateUser;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, Result>
{
    private readonly IContext _context;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;

    public UpdateUserHandler(IContext context, IS3Service s3Service, IOptions<AWSConfiguration> appSettings)
    {
        _context = context;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }


    public async Task<Result> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var originalUser = await _context.Users
            .FirstAsync(x => Equals(x.Id, request.Id), cancellationToken);

        if (!string.IsNullOrEmpty(request.FirstName))
        {
            originalUser.FirstName = request.FirstName;
        }

        if (!string.IsNullOrEmpty(request.LastName))
        {
            originalUser.LastName = request.LastName;
        }

        if (request.DisplayPicture != null)
        {
            if (!string.IsNullOrEmpty(originalUser.DisplayPicture))
            {
                var uploadKey = originalUser.DisplayPicture?.Split(_appSettings.Value.S3BucketName)[1];

                if (uploadKey == null) return Result.Error("Error uploading cover image");
                var fileExists = await _s3Service.CheckIfFileExists(_appSettings.Value.S3BucketName,
                    uploadKey, cancellationToken);
                if (fileExists)
                {
                    await _s3Service.DeleteFile(_appSettings.Value.S3BucketName, uploadKey, cancellationToken);
                }
            }

            originalUser.DisplayPicture = S3Utils.UploadImage(_s3Service, request.DisplayPicture,
                originalUser.Id.ToString(),
                _appSettings.Value.S3BucketName,
                _appSettings.Value.S3BucketKeyForProfilePictures, cancellationToken).Result;
        }

        if (!string.IsNullOrEmpty(request.MetadataJson))
        {
            originalUser.MetadataJson = request.MetadataJson;
        }

        if (!string.IsNullOrEmpty(request.StripeCustomerToken))
        {
            originalUser.StripeCustomerToken = request.StripeCustomerToken;
        }

        _context.Users.Update(originalUser);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}