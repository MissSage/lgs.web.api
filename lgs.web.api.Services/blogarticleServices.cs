using lgs.web.api.IRepository;
using lgs.web.api.IRepository.Base;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lgs.web.api.Services
{
	public class blogarticleServices : BaseServices<blogarticle>, IblogarticleServices
    {
        readonly IblogarticleRepository _dal;
        readonly IBaseRepository<blogarticle_tags> _tagdal;
        readonly IBaseRepository<sysUserInfo> _userdal;
        public blogarticleServices(
            IblogarticleRepository dal, 
            IBaseRepository<blogarticle_tags> tagdal,
            IBaseRepository<sysUserInfo> userdal
            )
        {
            this._tagdal = tagdal;
            this._userdal = userdal;
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<blogarticle> GetBlogDetails(int id)
        {
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            //var bloglist = await base.Query(a => a.IsDeleted==false, a => a.Id);
            var blogarticle = await QueryById(id);
            if (blogarticle != null)
            {
                //要取下一篇和上一篇，以当前id开始，按id排序后top(2)，而不用取出所有记录
                //这样在记录很多的时候也不会有多大影响
                var nextBlogs = await base.Query(a => a.Id >= id && a.IsDeleted == false, 2, "Id");
                if (nextBlogs.Count == 2)
                {
                    blogarticle.next = nextBlogs[1].btitle;
                    blogarticle.nextID = nextBlogs[1].Id;
                }
                var prevBlogs = await base.Query(a => a.Id <= id && a.IsDeleted == false, 2, "Id desc");
                if (prevBlogs.Count == 2)
                {
                    blogarticle.previous = prevBlogs[1].btitle;
                    blogarticle.previousID = prevBlogs[1].Id;
                }
                blogarticle.Tags =await _tagdal.Query(p => p.bBlogID == blogarticle.Id);
                blogarticle.Author = await _userdal.QueryById(blogarticle.bsubmitter);
                blogarticle.btraffic += 1;
                await base.Update(blogarticle, new List<string> { "btraffic" });
            }

            return blogarticle;

        }


		public async Task<PageModel<blogarticle>> GetMapList(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
		{
            return await _dal.GetMapList(whereExpression, intPageIndex, intPageSize, strOrderByFileds);
        }

		public async Task<PageModel<blogarticle>> QueryMuchTable(Expression<Func<blogarticle, bool>> whereExpression, int intPageIndex = 1, int intPageSize = 20, string strOrderByFileds = null)
		{
            return await _dal.QueryMuchTable(whereExpression, intPageIndex, intPageSize, strOrderByFileds);
        }
	}
}