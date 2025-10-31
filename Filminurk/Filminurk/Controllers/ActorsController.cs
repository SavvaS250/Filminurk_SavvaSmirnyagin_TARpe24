using Filminurk.ApplicationServices.Services;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Actors;
using Filminurk.Models.Actros;
using Microsoft.AspNetCore.Mvc;

namespace Filminurk.Controllers
{
    public class ActorsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IActorSevices _actorServices;

        public ActorsController
            (
            FilminurkTARpe24Context context,
            IActorSevices actorSevices
            )
        {
            _context = context;
            _actorServices = actorSevices;
        }
        public IActionResult Index()
        {
            var result = _context.Actors.Select(x => new ActorsIndexViewModel
            {
                ActorID = x.ActorID,
                FirstName = x.FirstName,
                LastName = x.LastName,
                NickName = x.NickName,
                ActorType = x.ActorType,
                CareerStart = x.CareerStart,
            });
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ActorsCreateUpdateViewModel result = new();
            return View("CreateUpdate" ,result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ActorsCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid == true)
            {

                var dto = new ActorDTO()
                {
                    ActorID = vm.ActorID,
                    FirstName = vm.FirstName,
                    LastName = vm.LastName,
                    NickName = vm.NickName,
                    //MoviesActedFor = vm.MoviesActedFor,
                    //PortraitID = vm.PortraitID,
                    ActorAge = vm.ActorAge,
                    ActorType = vm.ActorType,
                    CareerStart = vm.CareerStart,
                    CareerEnd = vm.CareerEnd,
                    EntryCreatedAt = vm.EntryCreatedAt,
                    EntryModifiedAt = vm.EntryModifiedAt,
                };
                var result = await _actorServices.Create(dto);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var actor = await _actorServices.DetailsAsync(id);
        }
    }
}
