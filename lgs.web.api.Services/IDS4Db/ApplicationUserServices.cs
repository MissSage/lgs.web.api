using lgs.web.api.IRepository.Base;
using lgs.web.api.Model.IDS4DbModels;
using lgs.web.api.Services.BASE;

namespace lgs.web.api.IServices
{
    public class ApplicationUserServices : BaseServices<ApplicationUser>, IApplicationUserServices
    {

        IBaseRepository<ApplicationUser> _dal;
        public ApplicationUserServices(IBaseRepository<ApplicationUser> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

    }
}