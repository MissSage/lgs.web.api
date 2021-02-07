
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using lgs.web.api.IRepository.Base;

namespace lgs.web.api.Services
{
    public class blogarticle_commentsServices : BaseServices<blogarticle_comments>, Iblogarticle_commentsServices
    {
        private readonly IBaseRepository<blogarticle_comments> _dal;
        public blogarticle_commentsServices(IBaseRepository<blogarticle_comments> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}