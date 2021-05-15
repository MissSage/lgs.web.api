using lgs.web.api.IServices.BASE;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lgs.web.api.IServices
{	
	/// <summary>
	/// IblogarticleServices
	/// </summary>	
    public interface IblogarticleServices :IBaseServices<blogarticle>
	{
		Task<blogarticle> GetBlogDetails(int id);
		Task<PageModel<blogarticle>> GetMapList(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);
		Task<PageModel<blogarticle>> QueryMuchTable(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

	}
}