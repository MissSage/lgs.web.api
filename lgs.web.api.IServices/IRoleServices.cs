using lgs.web.api.IServices.BASE;
using lgs.web.api.Model.Models;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{	
	/// <summary>
	/// RoleServices
	/// </summary>	
    public interface IRoleServices :IBaseServices<Role>
	{
        Task<Role> SaveRole(string roleName);
        Task<string> GetRoleNameByRid(int rid);

    }
}
