using Taxbox.Application.Common;
using Taxbox.Domain.Entities;
using Taxbox.Infrastructure.Configuration;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System;

namespace Taxbox.Infrastructure.Context;

public class ApplicationDbContext : DbContext, IContext
{
    private readonly IConfiguration? _configuration;
    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Resource> Resources { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<UserSubscription> UserSubscriptions { get; set; } = null!;
    public DbSet<Page> Pages { get; set; } = null!;
    public DbSet<Ticket> Tickets { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var cx = _configuration?.GetConnectionString("DefaultConnection");
            // var cx = "Server=DESKTOP-KI6EKBL;Database=TaxboxDB;User=root;Password=1234;TrustServerCertificate=True";

            optionsBuilder.UseSqlServer(cx, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });

            base.OnConfiguring(optionsBuilder);
        }

        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDbFunction(
            typeof(ApplicationDbContext).GetMethod(nameof(JsonValue))!
        ).HasName("JSON_VALUE").IsBuiltIn();

        modelBuilder.HasDbFunction(
            typeof(ApplicationDbContext).GetMethod(nameof(JsonQuery))!
        ).HasName("JSON_QUERY").IsBuiltIn();


        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(ResourceConfiguration).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(CategoryConfiguration).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(SubscriptionConfiguration).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(UserSubscriptionConfiguration).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(UserSubscriptionConfiguration).Assembly)
            .ApplyConfigurationsFromAssembly(typeof(TicketConfiguration).Assembly);
    }

    public static string JsonValue(string column, [NotParameterized] string path)
        => throw new NotSupportedException();

    public static string JsonQuery(string column, [NotParameterized] string path) =>
        throw new NotSupportedException();
}