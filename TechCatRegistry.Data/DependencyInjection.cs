using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TechCatDbContext>(options =>
        {
            var connStr = configuration.GetConnectionString("Default");
            options.UseSqlServer(connStr);
        });

        return services;
    }
}
