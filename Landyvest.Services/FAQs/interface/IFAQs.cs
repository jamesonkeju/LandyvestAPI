using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Landyvest.Data.Payload;
using Landyvest.Services.Country.DTO;
using Landyvest.Utilities.Common;

namespace Landyvest.Services.FAQs.Interface
{
    public interface IFAQs
    {
        Task<List<Landyvest.Data.Models.Domains.Faq>> GetAllQuestionAndAnswers(FAQsFilter payload);
        Task<Landyvest.Data.Models.Domains.Faq> GetFAQsById(long id, bool CheckDeleted);
        Task<MessageOut> AddUpdateQuestionAndAnswer(Landyvest.Services.FAQsDTO.DTO.NotificationLogDTO payload);


    }
}
