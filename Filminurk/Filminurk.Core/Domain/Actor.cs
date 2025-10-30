using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Domain
{
    public class Actor
    {
        public Guid ActorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? NickName { get; set; }
        List<string>? MoviesActedFor {  get; set; }
        public Guid? PortraitID { get; set; }

        // kolm minu oma
        public int ActorAge { get; set; }
        public ActorType? ActorType { get; set; }
        public DateTime CareerStart {  get; set; }
        public DateTime? CareerEnd {  get; set; }

    }
}
