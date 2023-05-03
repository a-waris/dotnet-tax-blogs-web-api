using Taxbox.Domain.Entities.Common;
using System;

namespace Taxbox.Domain.Auth.Interfaces;

public interface ISession
{
    public UserId UserId { get; }

    public DateTime Now { get; }
}