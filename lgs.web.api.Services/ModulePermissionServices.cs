using lgs.web.api.Services.BASE;
using lgs.web.api.Model.Models;
using lgs.web.api.IRepository;
using lgs.web.api.IServices;
using lgs.web.api.IRepository.Base;

namespace lgs.web.api.Services
{	
	/// <summary>
	/// ModulePermissionServices
	/// </summary>	
	public class ModulePermissionServices : BaseServices<ModulePermission>, IModulePermissionServices
    {

        IBaseRepository<ModulePermission> _dal;
        public ModulePermissionServices(IBaseRepository<ModulePermission> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
