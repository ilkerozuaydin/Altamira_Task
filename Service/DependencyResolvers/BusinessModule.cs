using Business.Abstract;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.DependencyResolvers.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.DependencyResolvers
{
   public class BusinessModule: IDependencyResolverModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserManager>();
        }
    }
}
