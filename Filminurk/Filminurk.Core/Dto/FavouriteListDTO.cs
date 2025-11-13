using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;

namespace Filminurk.Core.Dto
{
    public class FavouriteListDTO
    {
        public Guid FavouriteID { get; set; }
        public string ListBelongsToUser { get; set; }
        public bool IsMovieOrActor { get; set; }
        public string ListName { get; set; }
        public string? ListDescription { get; set; }
        public bool IsPrivate { get; set; }
        public List<Movie>? ListOfMovies { get; set; }
        //public List<Actor>? ListOfMovies { get; set; }

        public DateTime ListCreatedAt { get; set; }
        public DateTime ListModifietAt { get; set; }
        public DateTime ListDeletedAt { get; set; }
        public bool IsReported { get; set; }
    }
}
