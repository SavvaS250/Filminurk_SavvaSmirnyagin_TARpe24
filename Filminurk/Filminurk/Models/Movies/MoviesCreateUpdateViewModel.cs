using Filminurk.Core.Domain;
using static Filminurk.Core.Domain.MovieGenre;

namespace Filminurk.Models.Movies
{
    public class MoviesCreateUpdateViewModel
    {
        public Guid? ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly FirstPublished { get; set; }
        public string Director { get; set; }
        public List<string>? Actors { get; set; }
        public double? CurrentRating { get; set; }
        //public List<UserComment>? Reviews { get; set; }

        /*kaasaoelvate piltide andmeomadused*/
        public List<IFormFile> Files { get; set; }
        public List<ImageViewModel> Images { get; set; } = new List<ImageViewModel>();

        public MovieGenre? MovieGenre { get; set; }
        public string? Country { get; set; }
        public int? Revenue { get; set; }

        /* andmebaasi jaoks vajalikud */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
