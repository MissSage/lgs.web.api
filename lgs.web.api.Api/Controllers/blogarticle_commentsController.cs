using lgs.web.api.Common.HttpContextUser;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace lgs.web.api.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/blogarticle_comments")]
    public class blogarticle_commentsController : ControllerBase
    {
        /// <summary>
        /// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
        /// </summary>
        private readonly Iblogarticle_commentsServices _blogarticle_commentsServices;
        readonly ISysUserInfoServices _sysUserInfoServices;
        private readonly IUser _user;
        public blogarticle_commentsController(Iblogarticle_commentsServices blogarticle_commentsServices,ISysUserInfoServices sysUserInfoServices, IUser user)
        {
            _sysUserInfoServices = sysUserInfoServices;
            _blogarticle_commentsServices = blogarticle_commentsServices;
            _user = user;
        }
    
        [HttpGet]
        public async Task<MessageModel<PageModel<blogarticle_comments>>> Get(int bid,int page = 1,int intPageSize = 20,bool isRootComment=true)
        {
            Expression<Func<blogarticle_comments, bool>> whereExpression = a => a.Id>0&&a.bBlogID==bid&&a.isRootComment== isRootComment;
            var res = await _blogarticle_commentsServices.QueryPage(whereExpression, page, intPageSize, " CreateTime desc ");
            
            res.data.ForEach(item =>
            {
                item.Author = _sysUserInfoServices.QueryById(item.CreatorID).Result;
                Expression<Func<blogarticle_comments, bool>> whereExpression1 = a => a.Id > 0 && a.ParentID == item.Id;
                item.Children = _blogarticle_commentsServices.Query(whereExpression1, intPageSize, " CreateTime desc ").Result;
                item.Children.ForEach(obj =>
                {
                    obj.Author = _sysUserInfoServices.QueryById(obj.CreatorID).Result;
                    obj.CallbackUser = _sysUserInfoServices.QueryById(obj.CallBackTo).Result;
                });
            });
            
            return new MessageModel<PageModel<blogarticle_comments>>()
            {
                msg = "获取成功",
                success = true,
                response = res
            };
        }
        [HttpGet("{id}")]
        public async Task<MessageModel<blogarticle_comments>> Get(int id = 0)
        {
            var item = await _blogarticle_commentsServices.QueryById(id);
            item.Author = _sysUserInfoServices.QueryById(item.CreatorID).Result;
            Expression<Func<blogarticle_comments, bool>> whereExpression1 = a => a.Id > 0 && a.ParentID == item.Id;
            item.Children = _blogarticle_commentsServices.Query(whereExpression1, 20, " CreateTime desc ").Result;
            item.Children.ForEach(obj =>
            {
                obj.Author = _sysUserInfoServices.QueryById(obj.CreatorID).Result;
                obj.CallbackUser = _sysUserInfoServices.QueryById(obj.CallBackTo).Result;
            });
            return new MessageModel<blogarticle_comments>()
            {
                msg = "获取成功",
                success = true,
                response = item
            };
        }

        [HttpPost]
        public async Task<MessageModel<blogarticle_comments>> Post(blogarticle_comments request)
        {
            request.CreateTime = DateTime.Now;
            if (_user != null)
            {
                request.CreatorID = _user.ID;
                request.IsDeleted = false;

            }
            else
            {
                return new MessageModel<blogarticle_comments>()
                {
                    msg = "添加失败",
                    success = false,
                    response = null
                };
            }
            var id = await _blogarticle_commentsServices.Add(request);
            var item = await _blogarticle_commentsServices.QueryById(id);
            item.Author = _sysUserInfoServices.QueryById(item.CreatorID).Result;
            Expression<Func<blogarticle_comments, bool>> whereExpression1 = a => a.Id > 0 && a.ParentID == item.Id;
            item.Children = _blogarticle_commentsServices.Query(whereExpression1, 20, " CreateTime desc ").Result;
            item.Children.ForEach(obj =>
            {
                obj.Author = _sysUserInfoServices.QueryById(obj.CreatorID).Result;
                obj.CallbackUser = _sysUserInfoServices.QueryById(obj.CallBackTo).Result;
            });
            return new MessageModel<blogarticle_comments>()
            {
                msg = "获取成功",
                success = id > 0,
                response = item
            };
        }
        [HttpPut]
        public async Task<MessageModel<string>> Put([FromBody] blogarticle_comments request)
        {
            var data = new MessageModel<string>();
            if (request.Id > 0)
            {
                data.success = await _blogarticle_commentsServices.Update(request);
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
                var detail = await _blogarticle_commentsServices.QueryById(id);

                detail.IsDeleted = true;

                    if (detail != null)
                    {
                        data.success = await _blogarticle_commentsServices.Update(detail);
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