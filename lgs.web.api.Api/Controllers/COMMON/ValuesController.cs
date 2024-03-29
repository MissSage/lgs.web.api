﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using lgs.web.api.Common;
using lgs.web.api.Common.HttpContextUser;
using lgs.web.api.Common.HttpRestSharp;
using lgs.web.api.Common.WebApiClients.HttpApis;
using lgs.web.api.Filter;
using lgs.web.api.IServices;
using lgs.web.api.Model;
using lgs.web.api.Model.Models;
using lgs.web.api.Model.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lgs.web.api.Controllers
{
	/// <summary>
	/// Values控制器
	/// </summary>
	[Route("api/[controller]/[action]")]
	[ApiController]
	//[Authorize]
	//[Authorize(Roles = "Admin,Client")]
	//[Authorize(Policy = "SystemOrAdmin")]
	//[Authorize(PermissionNames.Permission)]
	[Authorize]
	public class ValuesController : ControllerBase
	{
		private IMapper _mapper;
		private readonly IAdvertisementServices _advertisementServices;
		private readonly Love _love;
		private readonly IRoleModulePermissionServices _roleModulePermissionServices;
		private readonly IUser _user;
		private readonly IPasswordLibServices _passwordLibServices;
		private readonly IBlogApi _blogApi;
		private readonly IRedisBasketRepository _redisBasketRepository;
		private readonly IDoubanApi _doubanApi;
		readonly IblogarticleServices _blogarticleServices;

		/// <summary>
		/// ValuesController
		/// </summary>
		/// <param name="blogarticleServices"></param>
		/// <param name="mapper"></param>
		/// <param name="advertisementServices"></param>
		/// <param name="love"></param>
		/// <param name="roleModulePermissionServices"></param>
		/// <param name="user"></param>
		/// <param name="passwordLibServices"></param>
		/// <param name="blogApi"></param>
		/// <param name="redisBasketRepository"></param>
		/// <param name="doubanApi"></param>
		public ValuesController(IblogarticleServices blogarticleServices,
			IMapper mapper, IAdvertisementServices advertisementServices,
			Love love,
			IRoleModulePermissionServices roleModulePermissionServices,
			IUser user, IPasswordLibServices passwordLibServices,
			IBlogApi blogApi,
			IRedisBasketRepository redisBasketRepository,
			IDoubanApi doubanApi)
		{
			// 测试 Authorize 和 mapper
			_mapper = mapper;
			_advertisementServices = advertisementServices;
			_love = love;
			// 测试 Httpcontext
			_user = user;
			_roleModulePermissionServices = roleModulePermissionServices;
			// 测试多库
			_passwordLibServices = passwordLibServices;
			// 测试http请求
			_blogApi = blogApi;
			_redisBasketRepository = redisBasketRepository;
			_doubanApi = doubanApi;
			// 测试AOP加载顺序，配合 return
			_blogarticleServices = blogarticleServices;
			// 测试redis消息队列
			_blogarticleServices = blogarticleServices;
		}
		/// <summary>
		/// Get方法
		/// </summary>
		/// <returns></returns>
		// GET api/values
		[HttpGet]
		[AllowAnonymous]
		public async Task<MessageModel<ResponseEnum>> Get()
		{
			var data = new MessageModel<ResponseEnum>();

			/*
             *  测试 sql 查询
             */
			var queryBySql = await _blogarticleServices.QuerySql("SELECT bsubmitter,btitle,bcontent,bCreateTime FROM blogarticle WHERE bID>5");


			/*
             *  测试 sql 更新
             * 
             * 【SQL参数】：@bID:5
             *  @bsubmitter:laozhang619
             *  @IsDeleted:False
             * 【SQL语句】：UPDATE `blogarticle`  SET
             *  `bsubmitter`=@bsubmitter,`IsDeleted`=@IsDeleted  WHERE `bID`=@bID
             */
			var updateSql = await _blogarticleServices.Update(new { bsubmitter = $"laozhang{DateTime.Now.Millisecond}", IsDeleted = false, bID = 5 });


			// 测试模拟异常，全局异常过滤器拦截
			var i = 0;
			var d = 3 / i;


			// 测试 AOP 缓存
			//var blogarticles = await _blogarticleServices.GetBlogs();


			// 测试多表联查
			var roleModulePermissions = await _roleModulePermissionServices.QueryMuchTable();


			// 测试多个异步执行时间
			var roleModuleTask = _roleModulePermissionServices.Query();
			var listTask = _advertisementServices.Query();
			var ad = await roleModuleTask;
			var list = await listTask;


			// 测试service层返回异常
			_advertisementServices.ReturnExp();

			return data;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task RedisMq()
		{
			var msg = "这里是一条日志";
			await _redisBasketRepository.ListLeftPushAsync(RedisMqKey.Loging, msg);
		}

		/// <summary>
		/// Get(int id)方法
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		// GET api/values/5
		[HttpGet("{id}")]
		[AllowAnonymous]
		//[TypeFilter(typeof(DeleteSubscriptionCache),Arguments =new object[] { "1"})]
		[TypeFilter(typeof(UseServiceDIAttribute), Arguments = new object[] { "laozhang" })]
		public ActionResult<string> Get(int id)
		{
			var loveu = _love.SayLoveU();

			return "value";
		}

		/// <summary>
		/// 测试参数是必填项
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[Route("/api/values/RequiredPara")]
		public string RequiredP([Required] string id)
		{
			return id;
		}


		/// <summary>
		/// 通过 HttpContext 获取用户信息
		/// </summary>
		/// <param name="ClaimType">声明类型，默认 jti </param>
		/// <returns></returns>
		[HttpGet]
		[Route("/api/values/UserInfo")]
		public MessageModel<List<string>> GetUserInfo(string ClaimType = "jti")
		{
			var getUserInfoByToken = _user.GetUserInfoFromToken(ClaimType);
			return new MessageModel<List<string>>()
			{
				success = _user.IsAuthenticated(),
				msg = _user.IsAuthenticated() ? _user.Name.ObjToString() : "未登录",
				response = _user.GetClaimValueByType(ClaimType)
			};
		}

		/// <summary>
		/// to redirect by route template name.
		/// </summary>
		[HttpGet("/api/custom/go-destination")]
		[AllowAnonymous]
		public void Source()
		{
			var url = Url.RouteUrl("Destination_Route");
			Response.Redirect(url);
		}

		/// <summary>
		/// route with template name.
		/// </summary>
		/// <returns></returns>
		[HttpGet("/api/custom/destination", Name = "Destination_Route")]
		[AllowAnonymous]
		public string Destination()
		{
			return "555";
		}


		/// <summary>
		/// 测试 post 一个对象 + 独立参数
		/// </summary>
		/// <param name="blogarticle">model实体类参数</param>
		/// <param name="id">独立参数</param>
		[HttpPost]
		[AllowAnonymous]
		public object Post([FromBody] blogarticle blogarticle, int id)
		{
			return Ok(new { success = true, data = blogarticle, id = id });
		}


		/// <summary>
		/// 测试 post 参数
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		[HttpPost]
		[Route("TestPostPara")]
		[AllowAnonymous]
		public object TestPostPara(string name)
		{
			return Ok(new { success = true, name = name });
		}

		/// <summary>
		/// 测试http请求 RestSharp Get
		/// </summary>
		/// <returns></returns>
		[HttpGet("RestsharpGet")]
		[AllowAnonymous]
		public MessageModel<BlogViewModels> RestsharpGet()
		{
			return HttpHelper.GetApi<MessageModel<BlogViewModels>>("http://apk.neters.club/", "api/Blog/DetailNuxtNoPer", "id=1");
		}
		/// <summary>
		/// 测试http请求 RestSharp Post
		/// </summary>
		/// <returns></returns>
		[HttpGet("RestsharpPost")]
		[AllowAnonymous]
		public TestRestSharpPostDto RestsharpPost()
		{
			return HttpHelper.PostApi<TestRestSharpPostDto>("http://apk.neters.club/api/Values/TestPostPara?name=老张", new { age = 18 });
		}

		/// <summary>
		/// 测试多库连接
		/// </summary>
		/// <returns></returns>
		[HttpGet("TestMutiDBAPI")]
		[AllowAnonymous]
		public async Task<object> TestMutiDBAPI()
		{
			// 从主库（Sqlite）中，操作blogs
			var blogs = await _blogarticleServices.Query(d => d.Id == 1);

			// 从从库（Sqlserver）中，获取pwds
			var pwds = await _passwordLibServices.Query(d => d.PLID > 0);

			return new
			{
				blogs,
				pwds
			};
		}

		/// <summary>
		/// 测试http请求 WebApiClient Get
		/// </summary>
		/// <returns></returns>
		[HttpGet("WebApiClientGetAsync")]
		[AllowAnonymous]
		public async Task<object> WebApiClientGetAsync()
		{
			int id = 1;
			string isbn = "9787544270878";
			var doubanVideoDetail = await _doubanApi.VideoDetailAsync(isbn);
			return await _blogApi.DetailNuxtNoPerAsync(id);
		}

		/// <summary>
		/// Put方法
		/// </summary>
		/// <param name="id"></param>
		/// <param name="value"></param>
		// PUT api/values/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}
		/// <summary>
		/// Delete方法
		/// </summary>
		/// <param name="id"></param>
		// DELETE api/values/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}
	}
}
