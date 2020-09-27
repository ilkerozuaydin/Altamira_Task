using Core.DependencyResolvers.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.DependencyResolvers
{
    public class DataAccessModule : IDependencyResolverModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IUserOperationClaimDal, EfUserOperationClaimDal>();
        }
    }
}
