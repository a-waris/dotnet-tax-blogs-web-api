using Taxbox.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Taxbox.Application.Common;

public interface IContext : IAsyncDisposable, IDisposable
{
    public DatabaseFacade Database { get; }

    public DbSet<User> Users { get; }
    public DbSet<Resource> Resources { get; }
    public DbSet<Category> Categories { get; }
    public DbSet<Subscription> Subscriptions { get; }
    public DbSet<UserSubscription> UserSubscriptions { get; }
    public DbSet<Page> Pages { get; }
    public DbSet<Ticket> Tickets { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}