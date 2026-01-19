using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;
using Filminurk.Core.ServiceInterface;
using Filminurk.Data;
using Microsoft.EntityFrameworkCore;

namespace Filminurk.ApplicationServices.Services
{
    public class ActorServices : IActorSevices
    {
        private readonly FilminurkTARpe24Context _context;

        public ActorServices(FilminurkTARpe24Context context)
        {
            _context = context;
        }

        public async Task<Actor> Create(ActorDTO dto)
        {
            Actor actor = new Actor();
            actor.ActorID = Guid.NewGuid();
            actor.FirstName = dto.FirstName;
            actor.LastName = dto.LastName;
            actor.NickName = dto.NickName;
            actor.ActorType = dto.ActorType;
            //actor.MoviesActedFor = dto.MoviesActedFor;
            //actor.PortraitID = dto.PortraitID;
            actor.ActorAge = (int)dto.ActorAge;
            actor.CareerStart = DateTime.Now;
            actor.CareerEnd = DateTime.Now;
            actor.EntryCreatedAt = DateTime.Now;
            actor.EntryModifiedAt = DateTime.Now;

            await _context.Actors.AddAsync(actor);
            await _context.SaveChangesAsync();

            return actor;
        }

        public async Task<Actor> DetailsAsync(Guid id)
        {
            var result = await _context.Actors.FirstOrDefaultAsync(x => x.ActorID == id);
            return result;
        }   

        public async Task<Actor> Delete(Guid id)
        {
            var result = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorID == id);

            _context.Actors.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<Actor> Update (ActorDTO dto)
        {
            Actor actor = new Actor();
            actor.ActorID = (Guid)dto.ActorID;
            actor.FirstName = dto.FirstName;
            actor.LastName = dto.LastName;
            actor.NickName = dto.NickName;
            actor.ActorType = dto.ActorType;
            //actor.MoviesActedFor = dto.MoviesActedFor;
            //actor.PortraitID = dto.PortraitID;
            actor.ActorAge = (int)dto.ActorAge;
            actor.CareerStart = DateTime.Now;
            actor.CareerEnd = DateTime.Now;
            actor.EntryCreatedAt = dto.EntryCreatedAt;
            actor.EntryModifiedAt = dto.EntryModifiedAt;

            _context.Actors.Update(actor);
            await _context.SaveChangesAsync();
            return actor;
        }
    }
}
