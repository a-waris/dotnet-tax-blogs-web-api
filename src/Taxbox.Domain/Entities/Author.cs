using Elastic.Clients.Elasticsearch;
using Taxbox.Domain.Entities.Common;
using MassTransit;
using System;

namespace Taxbox.Domain.Entities;

public class Author : Entity<AuthorId>
{
    public override AuthorId Id { get; set; } = NewId.NextGuid();

    public AuthorId AuthorId
    {
        get => Id;

        set => Id = value;
    }

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