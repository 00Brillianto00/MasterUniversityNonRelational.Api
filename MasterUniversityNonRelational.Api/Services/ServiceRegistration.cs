using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MasterUniversityNonRelational.Api.Interfaces;

namespace MasterUniversityNonRelational.Api.Services
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUniversityService, UniversityService>();
            return services;
        }
    }
}
