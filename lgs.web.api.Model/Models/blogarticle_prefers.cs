using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace lgs.web.api.Model.Models
{
    ///<summary>
    ///收藏表
    ///</summary>
    [SugarTable( "blogarticle_prefers", "WMBLOG_MYSQL")]
    public class blogarticle_prefers
    {
        public blogarticle_prefers()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id { get; set; }

           public int pBlogID { get; set; }

           public int pUID { get; set; }
    }
}