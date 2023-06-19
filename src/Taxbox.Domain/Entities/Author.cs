using Taxbox.Domain.Entities.Common;
using MassTransit;
using Nest;
using System;

namespace Taxbox.Domain.Entities;

public class Author
{
    public string Id { get; set; } = NewId.NextGuid().ToString();
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public SocialMedia? SocialMedia { get; set; }
    public GeoLocation? Location { get; set; }
    public DateTime? JoinDate { get; set; }
}

public class SocialMedia
{
    public string Linkedin { get; set; } = null!;
    public string Website { get; set; } = null!;
    public string Twitter { get; set; } = null!;
}