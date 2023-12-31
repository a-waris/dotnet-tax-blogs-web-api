﻿using StronglyTypedIds;
using System;

[assembly: StronglyTypedIdDefaults(
    backingType: StronglyTypedIdBackingType.Guid,
    converters: StronglyTypedIdConverter.SystemTextJson | StronglyTypedIdConverter.EfCoreValueConverter |
                StronglyTypedIdConverter.Default | StronglyTypedIdConverter.TypeConverter,
    implementations: StronglyTypedIdImplementations.IEquatable | StronglyTypedIdImplementations.Default)]

namespace Taxbox.Domain.Entities.Common;

public interface IGuid
{
}

[StronglyTypedId]
public partial struct HeroId : IGuid
{
    public static implicit operator HeroId(Guid guid)
    {
        return new HeroId(guid);
    }
}


[StronglyTypedId]
public partial struct UserId : IGuid
{
    public static implicit operator UserId(Guid guid)
    {
        return new UserId(guid);
    }
}


[StronglyTypedId]
public partial struct ResourceId : IGuid
{
    public static implicit operator ResourceId(Guid guid)
    {
        return new ResourceId(guid);
    }
}

[StronglyTypedId]
public partial struct CategoryId : IGuid
{
    public static implicit operator CategoryId(Guid guid)
    {
        return new CategoryId(guid);
    }
}

[StronglyTypedId]
public partial struct UserSubscriptionId : IGuid
{
    public static implicit operator UserSubscriptionId(Guid guid)
    {
        return new UserSubscriptionId(guid);
    }
}

[StronglyTypedId]
public partial struct SubscriptionId : IGuid
{
    public static implicit operator SubscriptionId(Guid guid)
    {
        return new SubscriptionId(guid);
    }
}

[StronglyTypedId]
public partial struct PageId : IGuid
{
    public static implicit operator PageId(Guid guid)
    {
        return new PageId(guid);
    }
}

[StronglyTypedId]
public partial struct TicketId : IGuid
{
    public static implicit operator TicketId(Guid guid)
    {
        return new TicketId(guid);
    }
}