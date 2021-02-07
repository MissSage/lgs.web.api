using lgs.web.api.IRepository.Base;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;

namespace lgs.web.api.Services
{
    /// <summary>
    /// PermissionServices
    /// </summary>	
    public class PermissionServices : BaseServices<Permission>, IPermissionServices
    {

        IBaseRepository<Permission> _dal;
        public PermissionServices(IBaseRepository<Permission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
