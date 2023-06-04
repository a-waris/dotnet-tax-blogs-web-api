namespace Taxbox.Application.Common;

public class AWSConfiguration
{
    public string AccessKey { get; init; } = null!;
    public string SecretKey { get; init; } = null!;
    public string Region { get; init; } = null!;
    public string S3BucketName { get; init; } = null!;
    public string S3BucketUrl { get; init; } = null!;

    public string S3BucketKeyForProfilePictures { get; init; } = null!;
    public string S3BucketKeyForArticleIndex { get; init; } = null!;
}