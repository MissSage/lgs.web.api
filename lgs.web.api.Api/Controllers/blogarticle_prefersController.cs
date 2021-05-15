using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lgs.web.api.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
    [Authorize(Permissions.Name)]
     public class blogarticle_prefersController : ControllerBase
        {
             /// <summary>
             /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
             /// </summary>
            private readonly Iblogarticle_prefersServices _blogarticle_prefersServices;
    
            public blogarticle_prefersController(Iblogarticle_prefersServices blogarticle_prefersServices)
            {
                _blogarticle_prefersServices = blogarticle_prefersServices;
            }
    
            [HttpGet]
            public async Task<MessageModel<PageModel<blogarticle_prefers>>> Get(int page = 1, string key = "",int intPageSize = 50)
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrWhiteSpace(key))
                {
                    key = "";
                }
    
                Expression<Func<blogarticle_prefers, bool>> whereExpression = a => a.Id > 0;
    
                return new MessageModel<PageModel<blogarticle_prefers>>()
                {
                    msg = "获取成功",
                    success = true,
                    response = await _blogarticle_prefersServices.QueryPage(whereExpression, page, intPageSize)
                };

    }

    [HttpGet("{id}")]
    public async Task<MessageModel<blogarticle_prefers>> Get(int id = 0)
    {
        return new MessageModel<blogarticle_prefers>()
        {
            msg = "获取成功",
            success = true,
            response = await _blogarticle_prefersServices.QueryById(id)
        };
    }

    [HttpPost]
    public async Task<MessageModel<string>> Post([FromBody] blogarticle_prefers request)
    {
        var data = new MessageModel<string>();

        var id = await _blogarticle_prefersServices.Add(request);
        data.success = id > 0;

        if (data.success)
        {
            data.response = id.ObjToString();
            data.msg = "添加成功";
        }

        return data;
    }

    [HttpPut]
    public async Task<MessageModel<string>> Put([FromBody] blogarticle_prefers request)
    {
        var data = new MessageModel<string>();
        if (request.Id > 0)
        {
            data.success = await _blogarticle_prefersServices.Update(request);
            if (data.success)
            {
                data.msg = "更新成功";
                data.response = request?.Id.ObjToString();
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
            var detail = await _blogarticle_prefersServices.QueryById(id);

                if (detail != null)
                {
                    data.success = await _blogarticle_prefersServices.Delete(detail);
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