using System;
using System.Linq;
using System.Web.Mvc;
using Tournament.Entities;
using Tournament.ViewModels;

namespace Tournament.Controllers
{
    public class PlayerController : BaseController
    {
        private PlayerViewModel ToViewModel(Player player)
        {
            var vm = new PlayerViewModel();
            vm.Id = player.Id;
            vm.FirstName = player.FirstName;
            vm.LastName = player.LastName;
            vm.NickName = player.NickName;
            vm.Twitter = player.Twitter;
            vm.Slug = player.Slug;
            return vm;
        }

        private Player ToEntity(PlayerViewModel viewModel)
        {
            var player = new Player();
            player.Id = viewModel.Id;
            player.FirstName = viewModel.FirstName;
            player.LastName = viewModel.LastName;
            player.NickName = viewModel.NickName;
            player.Twitter = viewModel.Twitter;
            player.Slug = viewModel.Slug;
            return player;
        }

        public ActionResult Index()
        {
            // execute the query to allow for the IEnumerable select execution below
            var players = RavenSession.Query<Player>().Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5))).ToList();

            // todo: use automapper?!
            var vm = players.Select(player => ToViewModel(player)).ToList();
            
            return View(vm);
        }

        public ActionResult Detail(int id)
        {
            var player = RavenSession.Load<Player>(id);
            if (player == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }

            var vm = ToViewModel(player);

            return View(vm);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Create(PlayerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var player = ToEntity(viewModel);
                RavenSession.Store(player, "players/");
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Team {0} does not exist", id));
            }
            return View(ToViewModel(model));
        }

        //[Authorize]
        [HttpPost]
        public ActionResult Edit(PlayerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var model = ToEntity(viewModel);
                RavenSession.Store(model);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        //[Authorize]
        [HttpGet]
        public ActionResult Delete(string id)
        {
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }
            return View(ToViewModel(model));
        }

        //[Authorize]
        [HttpPost]
        [ActionName("delete")]
        public ActionResult ConfirmDelete(string id)
        {
            var model = RavenSession.Load<Player>(id);
            if (model == null)
            {
                return HttpNotFound(string.Format("Player {0} does not exist", id));
            }
            RavenSession.Delete(model);
            return RedirectToAction("Index");
        }
    }
}