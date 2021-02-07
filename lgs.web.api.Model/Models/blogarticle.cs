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
        [SugarColumn(ColumnDataType = "longtext",IsNullable = true)]
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
        [SugarColumn(ColumnDataType = "nvarchar", Length =2000, IsNullable = true)]

        public string bRemark { get; set; }

        public bool? IsDeleted { get; set; }
    }
}