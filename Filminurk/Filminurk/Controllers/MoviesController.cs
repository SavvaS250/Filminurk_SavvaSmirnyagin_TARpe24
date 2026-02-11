using Filminurk.Core.Domain;
using System.Diagnostics.Metrics;
using System.IO;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Filminurk.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Filminurk.Data.Migrations;

namespace Filminurk.Controllers
{
    public class MoviesController : Controller
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IMovieServices _movieServices;
        private readonly IFilesServices _filesServices;
        public MoviesController
            (
            FilminurkTARpe24Context context,
            IMovieServices movieServices,
            IFilesServices fileServices
            )
        {
            _context = context;
            _movieServices = movieServices;
            _filesServices = fileServices;
        }
        public IActionResult Index()
        {
            var result = _context.Movies.Select(x => new MoviesIndexViewModel
            {
                ID = x.ID,
                Title = x.Title,
                FirstPublished = x.FirstPublished,
                CurrentRating = x.CurrentRating,
                Country = x.Country,
                MovieGenre = x.MovieGenre,
            });
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            MoviesCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MoviesCreateUpdateViewModel vm)
        {
            if (ModelState.IsValid == true)
            {

                var dto = new MoviesDTO()
                {
                    ID = vm.ID,
                    Title = vm.Title,
                    Description = vm.Description,
                    FirstPublished = vm.FirstPublished,
                    CurrentRating = vm.CurrentRating,
                    Director = vm.Director,
                    Actors = vm.Actors,
                    Country = vm.Country,
                    MovieGenre = vm.MovieGenre,
                    Revenue = vm.Revenue,
                    EntryCreatedAt = vm.EntryCreatedAt,
                    EntryModifiedAt = vm.EntryModifiedAt,
                    Files = vm.Files,
                    FilesToApiDtos = vm.Images
                    .Select(x => new FileToApiDto
                    {
                        ImageID = x.ImageID,
                        FilePath = x.FilePath,
                        MovieID = x.MovieID,
                        IsPoster = x.IsPoster,
                    }).ToArray()
                };
                var result = await _movieServices.Create(dto);
                if (result == null)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var movie = await _movieServices.DetailsAsync(id);

            if (movie == null)
            {
                return NotFound();
            }
            ImageViewModel[] images = await FileFromDatabase(id);

            var vm = new MoviesDetailsViewModel();
            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.FirstPublished = movie.FirstPublished;
            vm.CurrentRating = movie.CurrentRating;
            vm.Director = movie.Director;
            vm.Actors = movie.Actors;
            vm.Country = movie.Country;
            vm.MovieGenre = movie.MovieGenre;
            vm.Revenue = movie.Revenue;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);

        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var movie = await _movieServices.DetailsAsync(id);

            if (movie == null)
            {
                return NotFound();
            }
            var images = await _context.FilesToApi
                .Where(x => x.MovieID == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageID = id
                }).ToArrayAsync();

            var vm = new MoviesCreateUpdateViewModel();
            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.FirstPublished = movie.FirstPublished;
            vm.CurrentRating = movie.CurrentRating;
            vm.Director = movie.Director;
            vm.Actors = movie.Actors;
            vm.Country = movie.Country;
            vm.MovieGenre = movie.MovieGenre;
            vm.Revenue = movie.Revenue;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.Images.AddRange(images);

            return View("CreateUpdate",vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(MoviesCreateUpdateViewModel vm)
        {
            var dto = new MoviesDTO()
            {
                ID = vm.ID,
                Title = vm.Title,
                Description = vm.Description,
                FirstPublished = vm.FirstPublished,
                CurrentRating = vm.CurrentRating,
                Director = vm.Director,
                Actors = vm.Actors,
                Country = vm.Country,
                MovieGenre = vm.MovieGenre,
                Revenue = vm.Revenue,
                EntryCreatedAt = vm.EntryCreatedAt,
                EntryModifiedAt = vm.EntryModifiedAt,
                Files = vm.Files,
                FilesToApiDtos = vm.Images
                .Select(x => new FileToApiDto
                {
                    MovieID = x.MovieID,
                    FilePath = x.FilePath,
                    ImageID = x.ImageID
                }).ToArray()
            };
            var result = await _movieServices.Update(dto);

            if (result == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
           var movie = await _movieServices.DetailsAsync(id);
            
            if(movie == null)
            {
                return NotFound();
            }
            var images = await _context.FilesToApi
                .Where(x => x.MovieID == id)
                .Select(y => new ImageViewModel
                {
                    FilePath = y.ExistingFilePath,
                    ImageID = y.ImageID,
                }).ToArrayAsync();

            var vm = new MoviesDeleteViewModel();
            vm.ID = movie.ID;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.FirstPublished = movie.FirstPublished;
            vm.CurrentRating = movie.CurrentRating;
            vm.Director = movie.Director;
            vm.Actors = movie.Actors;
            vm.Country = movie.Country;
            vm.MovieGenre = movie.MovieGenre;
            vm.Revenue = movie.Revenue;
            vm.EntryCreatedAt = movie.EntryCreatedAt;
            vm.EntryModifiedAt = movie.EntryModifiedAt;
            vm.Images.AddRange(images);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var movie = await _movieServices.Delete(id);
            if(movie == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<ImageViewModel[]> FileFromDatabase(Guid id)
        {
            return await _context.FilesToApi
                .Where(x => x.MovieID == id)
                .Select(y => new ImageViewModel
                {
                    ImageID = y.ImageID,
                    MovieID = y.MovieID,
                    IsPoster = y.IsPoster,
                    FilePath = y.ExistingFilePath
                }).ToArrayAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Import()
        {
            return View();
        }

    }
}
