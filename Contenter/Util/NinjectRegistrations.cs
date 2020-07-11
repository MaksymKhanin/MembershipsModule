using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject.Modules;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Contenter.Infrastructure.Repository.DI.Implementation;


namespace Contenter.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationDbContext>().ToConstructor(_ => new ApplicationDbContext());
            Bind(typeof(IEntityRepository<>)).To(typeof(EntityRepository<>));

        }
    }
}