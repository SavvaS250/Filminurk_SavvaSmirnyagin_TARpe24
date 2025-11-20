using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.FavouriteLists;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Filminurk.Controllers
{
    public class FavouriteListsController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFavouriteListsServices _favouriteListsServices;
        public FavouriteListsController(FilminurkTARpe24Context context, IFavouriteListsServices favouriteListsServices)
        {
            _context = context;
            _favouriteListsServices = favouriteListsServices;
        }
        public IActionResult Index()
        {
            var resultingLists = _context.FavouriteLists
                .OrderByDescending(y => y.ListCreatedAt)
                .Select(x => new FavouriteListsIndexViewModel
                {
                    FavouriteID = x.FavouriteID,
                    ListBelongsToUser = x.ListBelongsToUser,
                    IsMovieOrActor = x.IsMovieOrActor,
                    ListName = x.ListName,
                    ListDescription = x.ListDescription,
                    ListCreatedAt = x.ListCreatedAt,
                    Image = (List<FavouriteListIndexImageViewModel>)_context.FilesToDatabase
                    .Where(ml => ml.ListID == x.FavouriteID)
                    .Select(li => new FavouriteListIndexImageViewModel
                    {
                        ListID = li.ListID,
                        ImageID = li.ImageID,
                        ImageData = li.ImageData,
                        ImageTitle = li.ImageTitle,
                        Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(li.ImageData)),
                    })
                });
                
                
            return View(resultingLists);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var movies = _context.Movies
                .OrderBy(m => m.Title)
                .Select(mo => new MoviesIndexViewModel
                {
                    ID = mo.ID,
                    Title = mo.Title,
                    FirstPublished = mo.FirstPublished,
                    MovieGenre = mo.MovieGenre,
                })
                .ToList();
            ViewData["allMovies"] = movies;
            ViewData["userHasSelected"] = new List<string>();
            FavouriteListUserCreateViewModel vm = new();
            return View("UserCreate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> UserCreate(FavouriteListUserCreateViewModel vm, List<string> userHasSelected,
            List<MoviesIndexViewModel> movies) 
        {
            List<Guid> tempParse = new();
            foreach (var stringID in userHasSelected)
            {
                tempParse.Add(Guid.Parse(stringID));
            }

            var newListDTO = new FavouriteListDTO()
            {

            };
            newListDTO.ListName = vm.ListName;
            newListDTO.ListDescription = vm.ListDescription;
            newListDTO.IsMovieOrActor = vm.IsMovieOrActor;
            newListDTO.IsPrivate = vm.IsPrivate;
            newListDTO.ListCreatedAt = DateTime.UtcNow;
            newListDTO.ListBelongsToUser = "00000000-0000-0000-0000-000000000001";
            newListDTO.ListModifietAt = DateTime.UtcNow;
            newListDTO.ListDeletedAt = vm.ListDeletedAt;
            
            List<Guid> convertedIDs = new List<Guid>();
            if (newListDTO.ListOfMovies != null)
            {
                convertedIDs = MovieToID(newListDTO.ListOfMovies);
            }
            var newList = await _favouriteListsServices.Create(newListDTO, convertedIDs);
            if (newList != null)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        private List<Guid> MovieToID(List<Movie> listOfMovies)
        {
            var result = new List<Guid>();
            foreach (var movie in listOfMovies)
            {
                result.Add(movie.ID);
            }
            return result;
        }
    }
}
