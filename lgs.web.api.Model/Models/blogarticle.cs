using SqlSugar;
using System;
using System.Collections.Generic;

namespace lgs.web.api.Model.Models
{
	///<summary>
	///
	///</summary>
	[SugarTable("blogarticle", "WMBLOG_MYSQL")]
	public class blogarticle : RootEntity
	{
		/// <summary>
		/// 作者id
		/// </summary>
		public int bsubmitter { get; set; }

		[SugarColumn(ColumnDataType = "nvarchar", Length = 255, IsNullable = true)]
		public string btitle { get; set; }
		[SugarColumn(ColumnDataType = "nvarchar", Length = 50, IsNullable = true)]

		public string bcategory { get; set; }
		[SugarColumn(ColumnDataType = "longtext", IsNullable = true)]
		public string bcontent { get; set; }
		/// <summary>
		/// 摘要
		/// </summary>
		public string digest { get; set; }
		/// <summary>
		/// 访问量
		/// </summary>
		public int btraffic { get; set; }
		/// <summary>
		/// 评论数量
		/// </summary>
		public int bcommentNum { get; set; }
		//public string tag1 { set; get; }
		//public string tag2 { set; get; }
		//public string tag3 { set; get; }

		public DateTime bUpdateTime { get; set; }

		public bool isPublic { get; set; }

		public bool isTop { get; set; }

		public DateTime bCreateTime { get; set; }
		[SugarColumn(ColumnDataType = "nvarchar", Length = 2000, IsNullable = true)]

		public string bRemark { get; set; }

		public bool? IsDeleted { get; set; }
		
		/// <summary>
		/// 上一篇
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public string previous { get; set; }

		/// <summary>
		/// 上一篇id
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public int previousID { get; set; }

		/// <summary>
		/// 下一篇
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public string next { get; set; }

		/// <summary>
		/// 下一篇id
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public int nextID { get; set; }
		/// <summary>
		/// 作者
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public sysUserInfo Author { set; get; }
		/// <summary>
		/// 标签列表
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public List<blogarticle_tags> Tags { set; get; }
		/// <summary>
		/// 收藏列表
		/// </summary>
		[SugarColumn(IsIgnore = true)]
		public List<blogarticle_prefers> Prefers { set; get; }
	}
}