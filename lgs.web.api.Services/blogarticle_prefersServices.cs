
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using lgs.web.api.IRepository.Base;

namespace lgs.web.api.Services
{
    public class blogarticle_prefersServices : BaseServices<blogarticle_prefers>, Iblogarticle_prefersServices
    {
        private readonly IBaseRepository<blogarticle_prefers> _dal;
        public blogarticle_prefersServices(IBaseRepository<blogarticle_prefers> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}