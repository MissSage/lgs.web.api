
using lgs.web.api.Common.HttpContextUser;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using lgs.web.api.Common.Helper;
using System.Linq;
using AutoMapper;
using System.Collections.Generic;

namespace lgs.web.api.Api.Controllers
{
	//[Route("api/[controller]/[action]")]
	//[ApiController]
    [Produces("application/json")]
    [Route("api/blogarticle")]
    //[Authorize(Permissions.Name)]
    public class blogarticleController : ControllerBase
    {
        /// <summary>
        /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
        /// </summary>
        readonly IblogarticleServices _blogarticleServices;
        readonly Iblogarticle_tagsServices _blogarticle_TagsServices;
        readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly IUser _user;
        IMapper _mapper;
        private readonly ILogger<blogarticleController> _logger;
        public blogarticleController(IblogarticleServices blogarticleServices,
            ISysUserInfoServices sysUserInfoServices,
            ILogger<blogarticleController> logger, 
            IUser user, 
            Iblogarticle_tagsServices blogarticle_TagsServices, IMapper mapper)
        {
            _blogarticleServices = blogarticleServices;
            _logger = logger;
            _user = user;
            _mapper = mapper;
            _blogarticle_TagsServices = blogarticle_TagsServices;
            _sysUserInfoServices = sysUserInfoServices;
        }

        /// <summary>
        /// 获取文章
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="intPageSize">展示数量</param>
        /// <param name="key">标题或文章内容</param>
        /// <param name="isPublic">是否公开</param>
        /// <param name="isTop">是否置顶</param>
        /// <param name="tag">标签</param>
        /// <param name="category">类别</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<blogarticle>>> Get(int page = 1, int intPageSize = 50, string key = "",bool isPublic=true,bool isTop=false,string tag="",string category="")
        {
            Expression<Func<blogarticle, bool>> where = a => a.IsDeleted == false && a.isPublic == isPublic&&a.isTop==isTop;
            Expression<Func<blogarticle, bool>> where1 = a => a.btitle.Contains(key) || a.bcontent.Contains(key);
            Expression<Func<blogarticle, bool>> where3 = a => a.bcategory == category;
            if (!string.IsNullOrEmpty(tag) || !string.IsNullOrWhiteSpace(tag))
            {
                Expression<Func<blogarticle_tags, bool>> where2 = p => p.bTag == tag;
                var tags = _blogarticle_TagsServices.Query(where2);
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
            var bloglist = await _blogarticleServices.QueryPage(where, page, intPageSize);
            //return bloglist;
            //var res = await _blogarticleServices.GetBlogs(page, intPageSize, key, isPublic, tag, category);
            return new MessageModel<PageModel<blogarticle>>()
            {
                msg = "获取成功",
                success = true,
                response = bloglist
            };
        }
        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        //[Authorize(Policy = "Scope_BlogModule_Policy")]
        public async Task<MessageModel<BlogViewModels>> Get(int id = 0)
        {
            // 此处想获取上一条下一条数据，因此将全部数据list出来，有好的想法请提出
            //var bloglist = await base.Query(a => a.IsDeleted==false, a => a.Id);
            var blogarticle = (await _blogarticleServices.Query(a => a.Id == id)).FirstOrDefault();

            BlogViewModels models = null;

            if (blogarticle != null)
            {
                models = _mapper.Map<BlogViewModels>(blogarticle);
                //要取下一篇和上一篇，以当前id开始，按id排序后top(2)，而不用取出所有记录
                //这样在记录很多的时候也不会有多大影响
                var nextBlogs = await _blogarticleServices.Query(a => a.Id >= id && a.IsDeleted == false, 2, "Id");
                if (nextBlogs.Count == 2)
                {
                    models.next = nextBlogs[1].btitle;
                    models.nextID = nextBlogs[1].Id;
                }
                var prevBlogs = await _blogarticleServices.Query(a => a.Id <= id && a.IsDeleted == false, 2, "Id desc");
                if (prevBlogs.Count == 2)
                {
                    models.previous = prevBlogs[1].btitle;
                    models.previousID = prevBlogs[1].Id;
                }
                models.tags = _blogarticle_TagsServices.Query(p => p.bBlogID == models.Id).Result.Select(p => p.bTag).ToList();
                models.Author = await _sysUserInfoServices.QueryById(blogarticle.bsubmitter);
                blogarticle.btraffic += 1;
                await _blogarticleServices.Update(blogarticle, new List<string> { "btraffic" });
            }
            return new MessageModel<BlogViewModels>()
            {
                msg = "获取成功",
                success = true,
                response = models
            };
        }
        [HttpPost]
        [Authorize]
        public async Task<MessageModel<string>> Post(blogarticle request)
        {
            var data = new MessageModel<string>();
            request.bCreateTime = DateTime.Now;
            request.bUpdateTime = DateTime.Now;
            request.IsDeleted = false;
            request.bsubmitter = _user.ID;
            request.bcommentNum = 0;
            request.btraffic = 0;
            var id = await _blogarticleServices.Add(request);
            data.success = id > 0;

            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }
            return data;
        }

        [HttpPut]
        public async Task<MessageModel<string>> Put(blogarticle blogarticle)
        {
            var data = new MessageModel<string>();
            if (blogarticle != null && blogarticle.Id > 0)
            {
                var model = await _blogarticleServices.QueryById(blogarticle.Id);

                if (model != null)
                {
                    model.btitle = blogarticle.btitle;
                    model.bcategory = blogarticle.bcategory;
                    model.isPublic = blogarticle.isPublic;
                    model.isTop = blogarticle.isTop;
                    model.digest = blogarticle.digest;
                    model.bcontent = blogarticle.bcontent;
                    model.bUpdateTime = DateTime.Now;
                    model.bRemark = blogarticle.bRemark;
                    data.success = await _blogarticleServices.Update(model);
                    if (data.success)
                    {
                        data.msg = "更新成功";
                        data.response = blogarticle?.Id.ObjToString();
                    }
                }

            }

            return data;
        }

        [HttpDelete("{id}")]
        public async Task<MessageModel<string>> Delete(int id = 0)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var detail = await _blogarticleServices.QueryById(id);

                detail.IsDeleted = true;

                if (detail != null)
                {
                    data.success = await _blogarticleServices.Update(detail);
                    if (data.success)
                    {
                        data.msg = "删除成功";
                        data.response = detail?.Id.ObjToString();
                    }
                }
            }

            return data;
        }
    }
}