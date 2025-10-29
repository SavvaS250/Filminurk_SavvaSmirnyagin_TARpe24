using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Domain
{

    
    public class Movie
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly FirstPublished { get; set; }
        public string Director { get; set; }
        public List<string>? Actors { get; set; }
        public double? CurrentRating { get; set; }
        public List<UserComment>? Reviews { get; set; }

        public MovieGenre? MovieGenre { get; set; }
        public string? Country { get; set; }
        public int? Revenue { get; set; }

        /* andmebaasi jaoks vajalikud */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
