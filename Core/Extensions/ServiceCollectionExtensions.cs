using Core.DependencyResolvers;
using Core.DependencyResolvers.Abstract;
using Core.Utilities.IoC.ServiceTools;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDependencyResolvers(this IServiceCollection
          services, IDependencyResolverModule[] modules)
        {
            foreach (var module in modules)
            {
                module.Load(services);
            }
            return ServiceTool.Create(services);
        }
    }
}