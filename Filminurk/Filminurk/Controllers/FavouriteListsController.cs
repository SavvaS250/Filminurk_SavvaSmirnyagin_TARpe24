using System.Threading.Tasks;
using Filminurk.ApplicationServices.Services;
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
                    ListDeletedAt = (DateTime)x.ListDeletedAt,
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
            // tekib ajutiline guid list movieiD-de hoidmiseks
            foreach (var stringID in userHasSelected)
            {
                //lisame iga stringi kohta järjendis userhasselected teisendatud guidi
                tempParse.Add(Guid.Parse(stringID));
            }
            //teeme uue  DTO nimekirja jaoks
            var newListDTO = new FavouriteListDTO()
            {

            };
            newListDTO.ListName = vm.ListName;
            newListDTO.ListDescription = vm.ListDescription;
            newListDTO.IsMovieOrActor = vm.IsMovieOrActor;
            newListDTO.IsPrivate = vm.IsPrivate;
            newListDTO.ListCreatedAt = DateTime.UtcNow;
            newListDTO.ListBelongsToUser = Guid.NewGuid().ToString();
            newListDTO.ListModifietAt = DateTime.UtcNow;
            newListDTO.ListDeletedAt = vm.ListDeletedAt;
            newListDTO.ListOfMovies = vm.ListOfMovies;
            
            //lisa filmid nimekirja, olemasolevate id-de põhiselt
            var listOfMoviesAdd = new List<Movie>();
            foreach (var movieId in tempParse)
            {
                var thisMovie = _context.Movies.Where(tm => tm.ID == movieId).ToList().First();
                listOfMoviesAdd.Add((Movie)thisMovie);
            }
            newListDTO.ListOfMovies = listOfMoviesAdd;  
            //List<Guid> convertedIDs = new List<Guid>();
            //if (newListDTO.ListOfMovies != null)
            //{
            //    convertedIDs = MovieToID(newListDTO.ListOfMovies);
            //}
            var newList = await _favouriteListsServices.Create(newListDTO /*, convertedIDs*/);
            if (newList == null)
            {
                return BadRequest();
            }
            return RedirectToAction("Index", vm);
        }

        [HttpGet]
        public async Task<IActionResult> UserDetails(Guid id, Guid thisUserID)
        {
            if (id == null || thisUserID == null)
            {
                return BadRequest();
                //TODO returnn corresponding errorviews. id not found for list, and user login error for userid
            }

            var thisList = _context.FavouriteLists.Where(tl => tl.FavouriteID == id && tl.ListBelongsToUser == thisUserID.ToString())
                .Select(
                stl => new FavouriteListUserDetailsViewModel
                {
                    FavouriteID = stl.FavouriteID,
                    ListBelongsToUser = stl.ListBelongsToUser,
                    IsMovieOrActor = stl.IsMovieOrActor,
                    ListName = stl.ListName,
                    ListDescription = stl.ListDescription,
                    IsPrivate = stl.IsPrivate,
                    ListOfMovies = stl.ListOfMovies,
                    IsReported = stl.IsReported,
                    //Image = _context.FilesToDatabase
                    //.Where(i => i.ListID == stl.FavouriteID)
                    //.Select(si => new FavouriteListIndexImageViewModel
                    //{
                    //    ImageID = si.ImageID,
                    //    ListID = si.ListID,
                    //    ImageData = si.ImageData,
                    //    ImageTitle = si.ImageTitle,
                    //    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
                    //}).ToList().First()
                }).First();

            //if (!ModelState.IsValid)
            //{
            //    return NotFound();
            //}
            
            if (thisList == null)
            {
                return NotFound();
            }

            return View("Details", thisList);
        }

        //[HttpGet]
        //public async Task<IActionResult> UserTogglePrivacy (Guid id, Guid thisUserID)
        //{
        //    if (id == null || thisUserID == null)
        //    {
        //        return BadRequest();
        //        //TODO returnn corresponding errorviews. id not found for list, and user login error for userid
        //    }

        //    var thisList = _context.FavouriteLists.Where(tl => tl.FavouriteID == id && tl.ListBelongsToUser == thisUserID.ToString())
        //        .Select(
        //        stl => new FavouriteListUserDetailsViewModel
        //        {
        //            FavouriteID = stl.FavouriteID,
        //            ListBelongsToUser = stl.ListBelongsToUser,
        //            IsMovieOrActor = stl.IsMovieOrActor,
        //            ListName = stl.ListName,
        //            ListDescription = stl.ListDescription,
        //            IsPrivate = stl.IsPrivate,
        //            ListOfMovies = stl.ListOfMovies,
        //            IsReported = stl.IsReported,
        //            //Image = _context.FilesToDatabase
        //            //.Where(i => i.ListID == stl.FavouriteID)
        //            //.Select(si => new FavouriteListIndexImageViewModel
        //            //{
        //            //    ImageID = si.ImageID,
        //            //    ListID = si.ListID,
        //            //    ImageData = si.ImageData,
        //            //    ImageTitle = si.ImageTitle,
        //            //    Image = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(si.ImageData))
        //            //}).ToList().First()
        //        }).First();

        //    //if (!ModelState.IsValid)
        //    //{
        //    //    return NotFound();
        //    //}

        //    if (thisList == null)
        //    {
        //        return NotFound();
        //    }

        //    return View("UserTogglePrivacy", thisList);
        //}


        [HttpPost]
        public async Task<IActionResult> UserTogglePrivacy(Guid id)
        {
           FavouriteList thisList = await _favouriteListsServices.DetailAsync(id);
           FavouriteListDTO updatedList = new FavouriteListDTO();
           updatedList.FavouriteID = thisList.FavouriteID;
           updatedList.ListBelongsToUser = thisList.ListBelongsToUser;
           updatedList.ListName = thisList.ListName;
           updatedList.ListDescription = thisList.ListDescription;
           updatedList.IsPrivate = !thisList.IsPrivate;
           updatedList.ListOfMovies = thisList.ListOfMovies;
           updatedList.IsReported = thisList.IsReported;
           updatedList.IsMovieOrActor = thisList.IsMovieOrActor;
           updatedList.ListCreatedAt = thisList.ListCreatedAt;
           updatedList.ListModifietAt = DateTime.Now;
           updatedList.ListDeletedAt = thisList.ListDeletedAt;
            ViewData["UpdateServiceType"] = "Private";

            var result = await _favouriteListsServices.Update(updatedList, "Private");
            //if (result == null)
            //{
            //    return NotFound();
            //}
            //if (result .IsPrivate != !result.IsPrivate)
            //{
            //    return BadRequest();
            //}
           //return RedirectToAction("UserDetails", result.FavouriteID);
           return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UserDelete (Guid id)
        {
            var deleteList = await _favouriteListsServices.DetailAsync(id);
            deleteList.ListDeletedAt = DateTime.Now;

            var dto = new FavouriteListDTO();
            dto.FavouriteID = deleteList.FavouriteID;
            dto.ListBelongsToUser = deleteList.ListBelongsToUser;
            dto.ListName = deleteList.ListName;
            dto.ListDescription = deleteList.ListDescription;
            dto.IsPrivate = !deleteList.IsPrivate;
            dto.ListOfMovies = deleteList.ListOfMovies;
            dto.IsReported = deleteList.IsReported;
            dto.IsMovieOrActor = deleteList.IsMovieOrActor;
            dto.ListCreatedAt = deleteList.ListCreatedAt;
            dto.ListModifietAt = DateTime.Now;
            dto.ListDeletedAt = deleteList.ListDeletedAt;

            var result = await _favouriteListsServices.Update(dto, "Delete");
            if (deleteList == null)
            {
                return NotFound();
            }
            
            return RedirectToAction("Index");
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
