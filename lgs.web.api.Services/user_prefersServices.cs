
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using lgs.web.api.IRepository.Base;

namespace lgs.web.api.Services
{
    public class user_prefersServices : BaseServices<user_prefers>, Iuser_prefersServices
    {
        private readonly IBaseRepository<user_prefers> _dal;
        public user_prefersServices(IBaseRepository<user_prefers> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
    }
}