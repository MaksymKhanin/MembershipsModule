using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Contenter.Models;
using Contenter.Infrastructure.Repository.DI.Abstract;
using Ninject;

namespace Contenter.Controllers
{
    public class UsersController : Controller
    {
        IEntityRepository<User> _repository;

        [Inject]
        public UsersController(IEntityRepository<User> repository) =>
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        public UsersController() { }

        public ActionResult Index()
        {

            return View();
        }

        public ActionResult PersonMemberships(int? id)
        {
            return View(id);
        }
    }
}