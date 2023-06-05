using Taxbox.Application.Common;
using Taxbox.Domain.Entities;
using Taxbox.Infrastructure.Configuration;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Taxbox.Infrastructure.Context;

public class ApplicationDbContext : DbContext, IContext
{
    private readonly IConfiguration? _configuration;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<Hero> Heroes { get; set; } = null!;

    public DbSet<User> Users { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(_configuration?.GetConnectionString("DefaultConnection"));
        }

        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
    }
}