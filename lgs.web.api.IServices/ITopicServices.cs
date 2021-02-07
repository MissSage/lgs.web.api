using lgs.web.api.IServices.BASE;
using lgs.web.api.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{
    public interface ITopicServices : IBaseServices<Topic>
    {
        Task<List<Topic>> GetTopics();
    }
}
