using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;

namespace Filminurk.Core.Dto
{
    public class ActorDTO
    {
        [Key]
        public Guid? ActorID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? NickName { get; set; }
        List<string>? MoviesActedFor { get; set; }
        public Guid? PortraitID { get; set; }

        // neli minu oma
        public int? ActorAge { get; set; }
        public ActorType? ActorType { get; set; }
        public DateTime? CareerStart { get; set; }
        public DateTime? CareerEnd { get; set; }

        /* andmebaasi jaoks vajalikud */
        public DateTime? EntryCreatedAt { get; set; }
        public DateTime? EntryModifiedAt { get; set; }
    }
}
