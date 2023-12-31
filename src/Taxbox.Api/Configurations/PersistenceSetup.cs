﻿using Taxbox.Application.Auth;
using Taxbox.Domain.Auth.Interfaces;
using Taxbox.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Taxbox.Api.Configurations;

public static class PersistenceSetup
{
    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddScoped<ISession, Session>();
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}