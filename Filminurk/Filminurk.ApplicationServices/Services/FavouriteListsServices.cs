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
    public class FavouriteListsServices : IFavouriteListsServices
    {
        private readonly FilminurkTARpe24Context _context;
        private readonly IFilesServices _filesServices;
        public FavouriteListsServices(FilminurkTARpe24Context context, IFilesServices filesServices)
        {
            _context = context;
            _filesServices = filesServices;
        }

        public async Task<FavouriteList> DetailAsync(Guid id)
        {
            var result = await _context.FavouriteLists
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.FavouriteID == id);
            return result;
        }

        public async Task<FavouriteList> Create(FavouriteListDTO dto/*, List<Movie> selectedMovies*/)
        {
            FavouriteList newlist = new();
            newlist.FavouriteID = Guid.NewGuid();
            newlist.ListName = dto.ListName;
            newlist.ListDescription = dto.ListDescription;
            newlist.ListCreatedAt = dto.ListCreatedAt;
            newlist.ListModifietAt = dto.ListModifietAt;
            newlist.ListDeletedAt = dto.ListDeletedAt;
            newlist.ListOfMovies = dto.ListOfMovies;
            newlist.ListBelongsToUser = dto.ListBelongsToUser;
            await _context.FavouriteLists.AddAsync(newlist);
            await _context.SaveChangesAsync();

            //foreach (var movieid in selectedMovies)
            //{
            //    _context.FavouriteLists.Entry()
            //} 
            return newlist;

        }

        public async Task<FavouriteList> Update(FavouriteListDTO updatedList, string typeOfMethod)
        {
            

            FavouriteList updatedListInDB = new();

            updatedListInDB.FavouriteID = updatedList.FavouriteID;
            updatedListInDB.ListBelongsToUser = updatedList.ListBelongsToUser;
            updatedListInDB.IsMovieOrActor = updatedList.IsMovieOrActor;
            updatedListInDB.ListName = updatedList.ListName;
            updatedListInDB.ListDescription = updatedList.ListDescription;
            updatedListInDB.IsPrivate = updatedList.IsPrivate;
            updatedListInDB.ListOfMovies = updatedList.ListOfMovies;
            updatedListInDB.ListCreatedAt = updatedList.ListCreatedAt;
            updatedListInDB.ListDeletedAt = updatedList.ListDeletedAt;
            updatedListInDB.ListModifietAt = updatedList.ListModifietAt;
            
            if (typeOfMethod == "Delete")
            {
                _context.FavouriteLists.Attach(updatedListInDB);
                _context.Entry(updatedListInDB).Property(l => l.ListDeletedAt).IsModified = true;
            }
            else if (typeOfMethod == "Private")
            {
                _context.FavouriteLists.Attach(updatedListInDB);
                _context.Entry(updatedListInDB).Property(l => l.IsPrivate).IsModified = true;
            }
            _context.Entry(updatedListInDB).Property(l => l.ListModifietAt).IsModified = true;
            await _context.SaveChangesAsync();
            return updatedListInDB;

        }

        public async Task<FavouriteList> Delete(Guid id)
        {
            var result = await _context.FavouriteLists.
                FirstOrDefaultAsync(x => x.FavouriteID == id);
            if (result != null)
            {
                _context.FavouriteLists.Remove(result);
                await _context.SaveChangesAsync();
            }

            return result;
        }
    }
}
