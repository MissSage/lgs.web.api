using lgs.web.api.IServices.BASE;
using lgs.web.api.Model.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{
    public interface ITopicDetailServices : IBaseServices<TopicDetail>
    {
        Task<List<TopicDetail>> GetTopicDetails();
    }
}
