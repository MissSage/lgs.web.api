using lgs.web.api.IRepository;
using lgs.web.api.IRepository.UnitOfWork;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Repository.Base;
using SqlSugar;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lgs.web.api.Repository
{
	/// <summary>
	/// blogarticleRepository
	/// </summary>
	public class blogarticleRepository : BaseRepository<blogarticle>, IblogarticleRepository
	{
		public blogarticleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
		{

		}

		/// <summary>
		/// 查询出角色-菜单-接口关系表全部Map属性数据
		/// </summary>
		/// <returns></returns>
		public async Task<PageModel<blogarticle>> GetMapList(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
		{
			RefAsync<int> totalCount = 0;
			var tags = Db.Queryable<blogarticle_tags>().ToList();
			var prefers = Db.Queryable<blogarticle_prefers>().ToList();
			int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
			var list = await Db.Queryable<blogarticle>()
				.Mapper(rmp => rmp.Author, rmp => rmp.bsubmitter)
				.Mapper(rmp => rmp.Tags, rmp => rmp.Id)
				.Mapper(rmp => rmp.Prefers, rmp => rmp.Id)
				.OrderByIF(!string.IsNullOrEmpty(strOrderByFileds), strOrderByFileds)
			 .WhereIF(whereExpression != null, whereExpression)
			 .ToPageListAsync(intPageIndex, intPageSize, totalCount);
			return new PageModel<blogarticle>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list };
		}

		public async Task<PageModel<blogarticle>> QueryMuchTable(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
		{
			RefAsync<int> totalCount = 0;
			int pageCount = (Math.Ceiling(totalCount.ObjToDecimal() / intPageSize.ObjToDecimal())).ObjToInt();
			var list= await Db.Queryable<blogarticle>()
				.Mapper((it,cache)=> {
					var allTags = cache.GetListByPrimaryKeys<blogarticle_tags>(v => v.Id);
					it.Tags = allTags.Where(v => v.bBlogID == it.Id).ToList();
					var allPrefers = cache.GetListByPrimaryKeys<blogarticle_prefers>(v => v.Id);
					it.Prefers = allPrefers.Where(v => v.pBlogID == it.Id).ToList();
					var author = cache.GetListByPrimaryKeys<sysUserInfo>(v => v.bsubmitter);
					it.Author = author.FirstOrDefault(v => v.uID == it.bsubmitter);
				})
				.Where(whereExpression)
				.OrderByIF(true, strOrderByFileds)
				.ToPageListAsync(intPageIndex, intPageSize, totalCount);
			return new PageModel<blogarticle>() { dataCount = totalCount, pageCount = pageCount, page = intPageIndex, PageSize = intPageSize, data = list };
		}
	}
}