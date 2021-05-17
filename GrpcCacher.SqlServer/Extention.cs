using GrpcCacher.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GrpcCacher.SqlServer
{
    public static class Extention
    {
        public static IServiceCollection AddGrpcCacher(this IServiceCollection services, Action<IServiceProvider, GrpcCacherConfig> action = null)
        {
            services.AddSingleton<GrpcCacherConfigBase>(x =>
            {
                var result = new GrpcCacherConfig();
                action?.Invoke(x, result);
                return result;
            });
            services.AddScoped<IGrpcCacherService, GrpcCacherService>();
            return services;
        }
    }
}
