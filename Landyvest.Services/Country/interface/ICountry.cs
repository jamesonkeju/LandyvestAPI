using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Services.Country.DTO;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.Country
{
    public interface ICountry
    {
        Task<List<Landyvest.Data.Models.Domains.Country>> GetAllCountries(Data.Payload.CountryFilter payload);
        Task<Landyvest.Data.Models.Domains.Country> GetCountryById(long id, bool CheckDeleted);
        Task<MessageOut> AddUpdateCountry(CountryDTO payload);
       
        Task<MessageOut> ToggleCountryStatus(long id);
    }
}
