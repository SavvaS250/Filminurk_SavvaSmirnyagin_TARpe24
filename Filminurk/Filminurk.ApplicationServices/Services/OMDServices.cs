using Filminurk.Core.Dto.OMDDTOs;
using Filminurk.Core.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Filminurk.ApplicationServices.Services
{
    public class OMDServices : IOMDServices
    {
        public async Task<OMDSearchRootDTO> OMDbSearchResult(string movieName)
        {
            string apikey = Filminurk.Data.Environment.omdbapikey;
            var searchUrl = $"https://omdbapi.com/?apikey={apikey}&t={movieName}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
                );
                var response = client.GetAsync(searchUrl).GetAwaiter().GetResult();
                var responseJson = await response.Content.ReadAsStringAsync();
                OMDSearchRootDTO dto = JsonSerializer.Deserialize<OMDSearchRootDTO>(responseJson);
                return dto;
            }
        }
    }
}