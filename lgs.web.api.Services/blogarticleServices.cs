using AutoMapper;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using lgs.web.api.IRepository.Base;
using System.Threading.Tasks;
using lgs.web.api.Model.ViewModels;
using System.Collections.Generic;
using lgs.web.api.Common;
using System.Linq;
using System.Linq.Expressions;
using System;
using lgs.web.api.Common.Helper;
using lgs.web.api.Model;

namespace lgs.web.api.Services
{
    public class blogarticleServices : BaseServices<blogarticle>, IblogarticleServices
    {
        IMapper _mapper;
        private readonly IBaseRepository<blogarticle> _dal;
        private readonly IBaseRepository<blogarticle_tags> _tagdal;
        public blogarticleServices(IBaseRepository<blogarticle> dal)
        {
            this._dal = dal;
            base.BaseDal = dal;
        }

        /// <summary>
        /// 获取视图博客详情信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BlogViewModels> GetBlogDetails(int id)
        {
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            //var bloglist = await base.Query(a => a.IsDeleted==false, a => a.Id);
            var blogarticle = (await base.Query(a => a.Id == id)).FirstOrDefault();

            BlogViewModels models = null;

            if (blogarticle != null)
            {
                models = _mapper.Map<BlogViewModels>(blogarticle);

                //要取下一篇和上一篇，以当前id开始，按id排序后top(2)，而不用取出所有记录
                //这样在记录很多的时候也不会有多大影响
                var nextBlogs = await base.Query(a => a.Id >= id && a.IsDeleted == false, 2, "Id");
                if (nextBlogs.Count == 2)
                {
                    models.next = nextBlogs[1].btitle;
                    models.nextID = nextBlogs[1].Id;
                }
                var prevBlogs = await base.Query(a => a.Id <= id && a.IsDeleted == false, 2, "Id desc");
                if (prevBlogs.Count == 2)
                {
                    models.previous = prevBlogs[1].btitle;
                    models.previousID = prevBlogs[1].Id;
                }
                models.tags = _tagdal.Query(p => p.bBlogID == models.Id).Result.Select(p=>p.bTag).ToList();
                blogarticle.btraffic += 1;
                await base.Update(blogarticle, new List<string> { "btraffic" });
            }

            return models;

        }


        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Caching(AbsoluteExpiration = 10)]
        public async Task<PageModel<blogarticle>> GetBlogs(int page = 1, int intPageSize = 50, string key = "", bool isPublic = true,bool isTop=false ,string tag = "", string category = "")
        {
            //Expression<Func<blogarticle, bool>> where = a => a.IsDeleted == false;
            //Expression<Func<blogarticle, bool>> where1 = a => a.btitle.Contains(key) || a.bcontent.Contains(key);
            ////Expression<Func<blogarticle_tags, bool>> where2 = a => a.bTag == tag;
            //Expression<Func<blogarticle, bool>> where3 = a => a.bcategory == category;
            ////if (!string.IsNullOrEmpty(tag) || !string.IsNullOrWhiteSpace(tag))
            ////{
            ////    where = where.And(where2);
            ////}
            //if (!string.IsNullOrEmpty(key) || !string.IsNullOrWhiteSpace(key))
            //{
            //    where = where.And(where1);
            //}
            //if (!string.IsNullOrEmpty(category) || !string.IsNullOrWhiteSpace(category))
            //{
            //    where = where.And(where3);
            //}
            //var bloglist = await base.QueryPage(where, page, intPageSize);
            //return bloglist;
            Expression<Func<blogarticle, bool>> where = a => a.IsDeleted == false&&a.isPublic== isPublic;
            Expression<Func<blogarticle, bool>> where1 = a => a.btitle.Contains(key) || a.bcontent.Contains(key);
            Expression<Func<blogarticle, bool>> where3 = a => a.bcategory == category;
            if (!string.IsNullOrEmpty(tag) || !string.IsNullOrWhiteSpace(tag))
            {
                Expression<Func<blogarticle_tags, bool>> where2 = p => p.bTag == tag;
                var tags = _tagdal.Query(where2);
                tags.Result.ForEach(t =>
                {
                    where = where.Or(p => p.Id == t.bBlogID);
                });
            }
            if (!string.IsNullOrEmpty(key) || !string.IsNullOrWhiteSpace(key))
            {
                where = where.And(where1);
            }
            if (!string.IsNullOrEmpty(category) || !string.IsNullOrWhiteSpace(category))
            {
                where = where.And(where3);
            }
            var bloglist = await _dal.QueryPage(where, page, intPageSize);
            return bloglist;
        }
    }
}