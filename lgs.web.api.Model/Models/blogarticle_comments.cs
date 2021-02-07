using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SqlSugar;


namespace lgs.web.api.Model.Models
{
    ///<summary>
    ///
    ///</summary>
    [SugarTable( "blogarticle_comments", "WMBLOG_MYSQL")]
    public class blogarticle_comments:RootEntity
    {
        public int bBlogID { get; set; }

        [SugarColumn(ColumnDataType = "nvarchar", Length = 200, IsNullable = true)]
        public string bContent { get; set; }

        public DateTime CreateTime { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100, IsNullable = true)]


        public int CreatorID { get; set; }

        public int? ParentID { get; set; }

        public int CallBackTo { get; set; }

        public bool isRootComment { get; set; }

        public int Prefers { get; set; }

        public bool? IsDeleted { get; set; }
        /// <summary>
        /// 评论回复
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<blogarticle_comments> Children { set; get; }
        /// <summary>
        /// 评论作者
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public sysUserInfo Author { set; get; }
        /// <summary>
        /// 回复的用户
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public sysUserInfo CallbackUser { set; get; }
    }
}