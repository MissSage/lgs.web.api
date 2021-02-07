using lgs.web.api.IServices.BASE;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Model.ViewModels;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{	
	/// <summary>
	/// IblogarticleServices
	/// </summary>	
    public interface IblogarticleServices :IBaseServices<blogarticle>
	{
		Task<PageModel<blogarticle>> GetBlogs(int page = 1, int intPageSize = 50, string key = "",bool isPublic=true,bool isTop=false, string tag = "", string category = "");
		Task<BlogViewModels> GetBlogDetails(int id);
	}
}