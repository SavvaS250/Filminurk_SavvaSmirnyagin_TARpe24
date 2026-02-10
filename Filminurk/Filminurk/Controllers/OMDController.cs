using Filminurk.Core.Domain;
using Filminurk.Core.Dto.OMDbDTOs;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Filminurk.Controllers
{
    public class OMDBController : Controller
    {
        private readonly IOMDbServices _omdbServices;
        private readonly FilminurkTARpe24Context _context;

        public OMDBController(IOMDbServices omdbServices, FilminurkTARpe24Context context)
        {
            _omdbServices = omdbServices;
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ImportMovie(MovieImportViewModel model)
        {
            OMDbSearchRootDTO dto = await _omdbServices.OMDbSearchResult(model.movieName);

            if (dto.Response == "False")
            {
                return RedirectToAction("Import", "Movies");
            }

            if (dto != null)
            {
                List<string> movieActors = new List<string>();
                string[] actorsArray = dto.Actors.Split(", ");
                foreach (string actorName in actorsArray)
                {
                    movieActors.Add(actorName);
                }

                Movie movie = new Movie();
                {
                    movie.ID = Guid.NewGuid();
                    movie.Description = dto.Plot;
                    movie.Title = dto.Title;
                    movie.Director = dto.Director;
                    movie.CurrentRating = Convert.ToDouble(dto.imdbRating.Replace(".", ","));
                    movie.Country = dto.Country;
                    movie.FirstPublished = DateOnly.ParseExact(
                        dto.Released,
                        "dd MMM yyyy",
                        CultureInfo.InvariantCulture
                    );
                    movie.MovieGenre = dto.Genre;
                    movie.Actors = movieActors;
                    movie.EntryCreatedAt = DateTime.Now;
                    movie.EntryModifiedAt = DateTime.Now;
                }
                _context.Movies.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movies");
            }
            else
            {
                return View();
            }
        }
    }
}