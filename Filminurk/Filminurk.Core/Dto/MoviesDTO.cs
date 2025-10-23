using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using static Filminurk.Core.Domain.MovieGenre;

namespace Filminurk.Core.Dto
{
    public class MoviesDTO
    {
        public Guid? ID { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly? FirstPublished { get; set; }
        public string? Director { get; set; }
        public List<string>? Actors { get; set; }
        public double? CurrentRating { get; set; }
        //public List<UserComment>? Reviews { get; set; }

        /*kaasaoelvate piltide andmeomadused*/
        public List<IFormFile> Files { get; set; }
        public IEnumerable<FileToApiDto> FilesToApiDtos { get; set; } = new List<FileToApiDto>();
       
        
        public MovieGenre? MovieGenre { get; set; }
        public string? Country { get; set; }
        public int? Revenue { get; set; }

        /* andmebaasi jaoks vajalikud */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
