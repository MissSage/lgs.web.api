﻿
using lgs.web.api.Common.HttpContextUser;
using lgs.web.api.IRepository.Base;
using lgs.web.api.IServices;
using lgs.web.api.Model.Models;
using lgs.web.api.Services.BASE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace lgs.web.api.Services
{
	public class blogarticle_tagsServices : BaseServices<blogarticle_tags>, Iblogarticle_tagsServices
    {
        private readonly IUser _user;
        private readonly IBaseRepository<blogarticle_tags> _dal;
        public blogarticle_tagsServices(IBaseRepository<blogarticle_tags> dal, IUser user)
        {
            this._user = user;
            this._dal = dal;
            base.BaseDal = dal;
        }

        public List<string> GetTags(int page = 1, int intPageSize = 50)
        {
            Expression<Func<blogarticle_tags, bool>> whereExpression = a => a.Id > 0 && a.CreateBy == _user.ID;

            var result = _dal.Query(p => p.CreateBy==_user.ID).Result.Select(p => p.bTag).ToList();
            return result;
        }
    }
}