using Filminurk.Core.Dto.OMDDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.ServiceInterface
{
    public interface IOMDServices
    {
        Task<OMDSearchRootDTO> OMDbSearchResult(string movieName);
    }
}