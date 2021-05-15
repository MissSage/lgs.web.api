using SqlSugar;
using System;


namespace lgs.web.api.Model.Models
{
	///<summary>
	///
	///</summary>
	[SugarTable("blogarticle_tags", "WMBLOG_MYSQL")]
	public class blogarticle_tags : RootEntity
	{
		public bool? IsDeleted { get; set; }
		public int CreateBy { get; set; }

		public DateTime CreateTime { get; set; }

		public int bBlogID { get; set; }

		[SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]
		public string bTag { get; set; }
	}
}