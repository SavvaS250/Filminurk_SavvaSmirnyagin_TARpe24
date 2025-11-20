using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filminurk.Core.Domain;
using Filminurk.Core.Dto;

namespace Filminurk.Core.ServiceInterface
{
    public interface IFavouriteListsServices
    {
        Task<FavouriteList> DetailAsync(Guid id);
        Task<FavouriteList> Create(FavouriteListDTO dto, List<Movie> selectedMovies);
    }
}
