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

        public async Task<FavouriteList> Update(FavouriteListDTO updatedList)
        {

        }
    }
}
