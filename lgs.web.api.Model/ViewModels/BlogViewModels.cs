using lgs.web.api.Model.Models;
using System.Collections.Generic;

namespace lgs.web.api.Model.ViewModels
{
	/// <summary>
	/// 博客信息展示类
	/// </summary>
	public class BlogViewModels
    {
        /// <summary>
        /// 博客
        /// </summary>
        public blogarticle blog { set; get; }
        

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

        /// <summary>
        /// 标签
        /// </summary>
        public List<blogarticle_tags> tags { set; get; }
        /// <summary>
        /// 作者
        /// </summary>
        public sysUserInfo Author { set; get; }
    }
}
