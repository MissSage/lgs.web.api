
using lgs.web.api.Common.HttpContextUser;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;
using AutoMapper;

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
            Expression<Func<blogarticle, bool>> where = a => true;
			if (!string.IsNullOrEmpty(key))
			{
                where = a => a.btitle.Contains(key) || a.digest.Contains(key) || a.bcontent.Contains(key);

            }
            var bloglist = await _blogarticleServices.QueryMuchTable(where,page, intPageSize,"bCreateTime desc");
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
        //[Authorize]
        //[Authorize(Policy = "Scope_BlogModule_Policy")]
        public async Task<MessageModel<blogarticle>> Get(int id = 0)
        {


            return new MessageModel<blogarticle>()
            {
                msg = "获取成功",
                success = true,
                response = await _blogarticleServices.GetBlogDetails(id)
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