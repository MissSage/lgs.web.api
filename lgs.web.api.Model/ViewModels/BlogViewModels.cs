﻿using lgs.web.api.Model.Models;
using System;
using System.Collections.Generic;

namespace lgs.web.api.Model.ViewModels
{
    /// <summary>
    /// 博客信息展示类
    /// </summary>
    public class BlogViewModels
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>创建人
        /// 
        /// </summary>
        public int bsubmitter { get; set; }

        /// <summary>博客标题
        /// 
        /// </summary>
        public string btitle { get; set; }

        /// <summary>摘要
        /// 
        /// </summary>
        public string digest { get; set; }

        /// <summary>
        /// 上一篇
        /// </summary>
        public string previous { get; set; }

        /// <summary>
        /// 上一篇id
        /// </summary>
        public int previousID { get; set; }

        /// <summary>
        /// 下一篇
        /// </summary>
        public string next { get; set; }

        /// <summary>
        /// 下一篇id
        /// </summary>
        public int nextID { get; set; }

        /// <summary>类别
        /// 
        /// </summary>
        public string bcategory { get; set; }

        /// <summary>内容
        /// 
        /// </summary>
        public string bcontent { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int btraffic { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int bcommentNum { get; set; }

        /// <summary> 修改时间
        /// 
        /// </summary>
        public DateTime bUpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime bCreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string bRemark { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public List<string> tags { set; get; }
        /// <summary>
        /// 作者
        /// </summary>
        public sysUserInfo Author { set; get; }
        //public string tag1 { set; get; }
        //public string tag2 { set; get; }
        //public string tag3 { set; get; }
    }
}
