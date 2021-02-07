using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lgs.web.api.Common.Helper;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Model.ViewModels;
using lgs.web.api.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Profiling;
using static lgs.web.api.Extensions.CustomApiVersion;

namespace lgs.web.api.Controllers
{
    /// <summary>
    /// 博客管理
    /// </summary>
    [Produces("application/json")]
    [Route("api/Blog")]
    public class BlogController : Controller
    {
        readonly IblogarticleServices _blogarticleServices;
        private readonly ILogger<BlogController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="blogarticleServices"></param>
        /// <param name="logger"></param>
        public BlogController(IblogarticleServices blogarticleServices, ILogger<BlogController> logger)
        {
            _blogarticleServices = blogarticleServices;
            _logger = logger;
        }


        /// <summary>
        /// 获取博客列表【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <param name="page"></param>
        /// <param name="bcategory"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<MessageModel<PageModel<blogarticle>>> Get(int id, int page = 1, string bcategory = "技术博文", string key = "")
        {
            int intPageSize = 6;
            if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
            {
                key = "";
            }

            Expression<Func<blogarticle, bool>> whereExpression = a => (a.bcategory == bcategory && a.IsDeleted == false) && ((a.btitle != null && a.btitle.Contains(key)) || (a.bcontent != null && a.bcontent.Contains(key)));

            var pageModelBlog = await _blogarticleServices.QueryPage(whereExpression, page, intPageSize, " Id desc ");

            using (MiniProfiler.Current.Step("获取成功后，开始处理最终数据"))
            {
                foreach (var item in pageModelBlog.data)
                {
                    if (!string.IsNullOrEmpty(item.bcontent))
                    {
                        item.bRemark = (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Length >= 200 ? (HtmlHelper.ReplaceHtmlTag(item.bcontent)).Substring(0, 200) : (HtmlHelper.ReplaceHtmlTag(item.bcontent));
                        int totalLength = 500;
                        if (item.bcontent.Length > totalLength)
                        {
                            item.bcontent = item.bcontent.Substring(0, totalLength);
                        }
                    }
                }
            }

            return new MessageModel<PageModel<blogarticle>>()
            {
                success = true,
                msg = "获取成功",
                response = new PageModel<blogarticle>()
                {
                    page = page,
                    dataCount = pageModelBlog.dataCount,
                    data = pageModelBlog.data,
                    pageCount = pageModelBlog.pageCount,
                }
            };
        }


        /// <summary>
        /// 获取博客详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "Scope_BlogModule_Policy")]
        public async Task<MessageModel<BlogViewModels>> Get(int id)
        {
            return new MessageModel<BlogViewModels>()
            {
                msg = "获取成功",
                success = true,
                response = await _blogarticleServices.GetBlogDetails(id)
            };
        }


        /// <summary>
        /// 获取详情【无权限】
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DetailNuxtNoPer")]
        public async Task<MessageModel<BlogViewModels>> DetailNuxtNoPer(int id)
        {
            _logger.LogInformation("xxxxxxxxxxxxxxxxxxx");
            return new MessageModel<BlogViewModels>()
            {
                msg = "获取成功",
                success = true,
                response = await _blogarticleServices.GetBlogDetails(id)
            };
        }

        [HttpGet]
        [Route("GoUrl")]
        public async Task<IActionResult> GoUrl(int id = 0)
        {
            var response = await _blogarticleServices.QueryById(id);
            if (response != null && response.bsubmitter.IsNotEmptyOrNull())
            {
                response.btraffic += 1;
                await _blogarticleServices.Update(response);
                return Redirect(response.bsubmitter.ToString());
            }

            return Ok();
        }

        [HttpGet]
        [Route("GetBlogsByTypesForMVP")]
        public async Task<MessageModel<List<blogarticle>>> GetBlogsByTypesForMVP(string types = "", int id = 0)
        {
            if (types.IsNotEmptyOrNull())
            {
                var blogs = await _blogarticleServices.Query(d => d.bcategory != null && types.Contains(d.bcategory) && d.IsDeleted == false);
                return new MessageModel<List<blogarticle>>()
                {
                    msg = "获取成功",
                    success = true,
                    response = blogs
                };
            }

            return new MessageModel<List<blogarticle>>() { };
        }

        [HttpGet]
        [Route("GetBlogByIdForMVP")]
        public async Task<MessageModel<blogarticle>> GetBlogByIdForMVP(int id = 0)
        {
            if (id > 0)
            {
                return new MessageModel<blogarticle>()
                {
                    msg = "获取成功",
                    success = true,
                    response = await _blogarticleServices.QueryById(id)
                };
            }

            return new MessageModel<blogarticle>() { };
        }

        /// <summary>
        /// 获取博客测试信息 v2版本
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        ////MVC自带特性 对 api 进行组管理
        //[ApiExplorerSettings(GroupName = "v2")]
        ////路径 如果以 / 开头，表示绝对路径，反之相对 controller 的想u地路径
        //[Route("/api/v2/blog/Blogtest")]
        //和上边的版本控制以及路由地址都是一样的

        [CustomRoute(ApiVersions.V2, "Blogtest")]
        public MessageModel<string> V2_Blogtest()
        {
            return new MessageModel<string>()
            {
                msg = "获取成功",
                success = true,
                response = "我是第二版的博客信息"
            };
        }

        /// <summary>
        /// 添加博客【无权限】
        /// </summary>
        /// <param name="blogarticle"></param>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Policy = "Scope_BlogModule_Policy")]
        [Authorize]
        public async Task<MessageModel<string>> Post([FromBody] blogarticle blogarticle)
        {
            var data = new MessageModel<string>();

            blogarticle.bCreateTime = DateTime.Now;
            blogarticle.bUpdateTime = DateTime.Now;
            blogarticle.IsDeleted = false;
            //blogarticle.bcategory = "技术博文";

            var id = (await _blogarticleServices.Add(blogarticle));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }

        //[HttpGet("{id}")]
        //[Authorize(Policy = "Scope_BlogModule_Policy")]
        //public async Task<MessageModel<blogarticle_Tags>> GetTags(int id)
        //{
        //    return new MessageModel<blogarticle_Tags>()
        //    {
        //        msg = "获取成功",
        //        success = true,
        //        response = await _blogarticleServices.GetBlogDetails(id)
        //    };
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="blogarticle"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddForMVP")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> AddForMVP([FromBody] blogarticle blogarticle)
        {
            var data = new MessageModel<string>();

            blogarticle.bCreateTime = DateTime.Now;
            blogarticle.bUpdateTime = DateTime.Now;
            blogarticle.IsDeleted = false;

            var id = (await _blogarticleServices.Add(blogarticle));
            data.success = id > 0;
            if (data.success)
            {
                data.response = id.ObjToString();
                data.msg = "添加成功";
            }

            return data;
        }
        /// <summary>
        /// 更新博客信息
        /// </summary>
        /// <param name="blogarticle"></param>
        /// <returns></returns>
        // PUT: api/User/5
        [HttpPut]
        [Route("Update")]
        [Authorize(Permissions.Name)]
        public async Task<MessageModel<string>> Put([FromBody] blogarticle blogarticle)
        {
            var data = new MessageModel<string>();
            if (blogarticle != null && blogarticle.Id > 0)
            {
                var model = await _blogarticleServices.QueryById(blogarticle.Id);

                if (model != null)
                {
                    model.btitle = blogarticle.btitle;
                    model.bcategory = blogarticle.bcategory;
                    model.bsubmitter = blogarticle.bsubmitter;
                    model.bcontent = blogarticle.bcontent;
                    model.btraffic = blogarticle.btraffic;

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



        /// <summary>
        /// 删除博客
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Permissions.Name)]
        [Route("Delete")]
        public async Task<MessageModel<string>> Delete(int id)
        {
            var data = new MessageModel<string>();
            if (id > 0)
            {
                var blogarticle = await _blogarticleServices.QueryById(id);
                blogarticle.IsDeleted = true;
                data.success = await _blogarticleServices.Update(blogarticle);
                if (data.success)
                {
                    data.msg = "删除成功";
                    data.response = blogarticle?.Id.ObjToString();
                }
            }

            return data;
        }
        /// <summary>
        /// apache jemeter 压力测试
        /// 更新接口
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ApacheTestUpdate")]
        public async Task<MessageModel<bool>> ApacheTestUpdate()
        {
            return new MessageModel<bool>()
            {
                success = true,
                msg = "更新成功",
                response = await _blogarticleServices.Update(new { bsubmitter = $"laozhang{DateTime.Now.Millisecond}", Id = 1 })
            };
        }
    }
}