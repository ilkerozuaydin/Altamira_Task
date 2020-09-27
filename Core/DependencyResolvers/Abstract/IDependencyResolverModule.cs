using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DependencyResolvers.Abstract
{
    public interface IDependencyResolverModule
    {
        void Load(IServiceCollection services);
    }
}
