using Filminurk.Core.Domain;
using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Actros
{
    public class ActorsIndexViewModel
    {
        [Key]
        public Guid ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? NickName { get; set; }

        public ActorType? ActorType { get; set; }
        public DateTime? CareerStart { get; set; }
        
    }
}
