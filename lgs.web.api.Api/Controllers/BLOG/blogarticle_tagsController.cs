using lgs.web.api.Common.HttpContextUser;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace lgs.web.api.Api.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Authorize(Permissions.Name)]
	public class blogarticle_tagsController : ControllerBase
	{
		/// <summary>
		/// 服务器接口，因为是模板生成，所以首字母是大写的，自己可以重构下
		/// </summary>
		private readonly Iblogarticle_tagsServices _blogarticle_tagsServices;
		private readonly IUser _user;

		public blogarticle_tagsController(Iblogarticle_tagsServices blogarticle_tagsServices, IUser user)
		{
			_user = user;
			_blogarticle_tagsServices = blogarticle_tagsServices;
		}

		[HttpGet]
		public MessageModel<List<string>> Get(int page = 1, int intPageSize = 50)
		{


			return new MessageModel<List<string>>()
			{
				msg = "获取成功",
				success = true,
				response = _blogarticle_tagsServices.GetTags(page, intPageSize)
			};
		}

		[HttpGet("{id}")]
		public async Task<MessageModel<blogarticle_tags>> Get(int id = 0)
		{
			return new MessageModel<blogarticle_tags>()
			{
				msg = "获取成功",
				success = true,
				response = await _blogarticle_tagsServices.QueryById(id)
			};
		}

		[HttpPost]
		public async Task<MessageModel<string>> Post([FromBody] blogarticle_tags request)
		{
			var data = new MessageModel<string>();
			request.CreateBy = _user.ID;
			request.CreateTime = DateTime.Now;
			request.IsDeleted = false;
			var id = await _blogarticle_tagsServices.Add(request);
			data.success = id > 0;
			if (data.success)
			{
				data.response = id.ObjToString();
				data.msg = "添加成功";
			}
			return data;
		}

		[HttpPut]
		public async Task<MessageModel<string>> Put([FromBody] blogarticle_tags request)
		{
			var data = new MessageModel<string>();
			if (request.Id > 0)
			{
				data.success = await _blogarticle_tagsServices.Update(request);
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
				var detail = await _blogarticle_tagsServices.QueryById(id);

				detail.IsDeleted = true;

				if (detail != null)
				{
					data.success = await _blogarticle_tagsServices.Update(detail);
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