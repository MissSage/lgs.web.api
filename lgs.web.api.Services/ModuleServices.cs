using lgs.web.api.IRepository.Base;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;

namespace lgs.web.api.Services
{
    /// <summary>
    /// ModuleServices
    /// </summary>	
    public class ModuleServices : BaseServices<Modules>, IModuleServices
    {

        IBaseRepository<Modules> _dal;
        public ModuleServices(IBaseRepository<Modules> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }
       
    }
}
