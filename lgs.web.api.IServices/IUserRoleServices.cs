using lgs.web.api.IServices.BASE;
using lgs.web.api.Model.Models;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{	
	/// <summary>
	/// UserRoleServices
	/// </summary>	
    public interface IUserRoleServices :IBaseServices<UserRole>
	{

        Task<UserRole> SaveUserRole(int uid, int rid);
        Task<int> GetRoleIdByUid(int uid);
    }
}

