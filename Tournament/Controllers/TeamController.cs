using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Tournament.Entities;
using Tournament.Infrastructure.Indexes;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class TeamController : BaseController
    {
        public ActionResult Index()
        {
            var teams = RavenSession.Query<Team, Teams_ByName>().ToList();
            var vm = Mapper.Map<IEnumerable<TeamViewModel>>(teams);
            return View(vm);
        }

        public ActionResult Detail(int id)
        {
            var model = RavenSession.Load<Team>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }

            return View(Mapper.Map<TeamViewModel>(model));
        }
    }
}