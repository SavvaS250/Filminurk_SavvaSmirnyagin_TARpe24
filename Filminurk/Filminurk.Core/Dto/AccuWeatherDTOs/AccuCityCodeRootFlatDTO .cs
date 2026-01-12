using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Metrics;
using System.Text;
using System.Threading.Tasks;

namespace Filminurk.Core.Dto.AccuWeather
{
    public class AccuCityCodeRootFlatDTO
    {
        public int Version { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int Rank { get; set; }
        public string LocalizedName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string PrimaryPostalCode { get; set; } = string.Empty;
        public Region? Region { get; set; }
        public Country? Country { get; set; }
        public AdministrativeArea? AdministrativeArea { get; set; }
        public TimeZone? TimeZone { get; set; }
        public Geoposition? Geoposition { get; set; }
        public bool IsAlias { get; set; }
        public SupplementalAdminArea[]? SupplementalAdminAreas { get; set; }
        public string[]? DataSets { get; set; }



    
        
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);


}