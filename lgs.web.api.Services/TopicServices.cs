using lgs.web.api.Common;
using lgs.web.api.IRepository.Base;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lgs.web.api.Services
{
    public class TopicServices: BaseServices<Topic>, ITopicServices
    {

        IBaseRepository<Topic> _dal;
        public TopicServices(IBaseRepository<Topic> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取开Bug专题分类（缓存）
        /// </summary>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 60)]
        public async Task<List<Topic>> GetTopics()
        {
            return await base.Query(a => !a.tIsDelete && a.tSectendDetail == "tbug");
        }

    }
}
