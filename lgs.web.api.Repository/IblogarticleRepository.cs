using lgs.web.api.IRepository.Base;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lgs.web.api.IRepository
{
	/// <summary>
	/// IblogarticleRepository
	/// </summary>	
	public interface IblogarticleRepository : IBaseRepository<blogarticle>
    {
        Task<PageModel<blogarticle>> GetMapList(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);
        Task<PageModel<blogarticle>> QueryMuchTable(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null);

	}
}