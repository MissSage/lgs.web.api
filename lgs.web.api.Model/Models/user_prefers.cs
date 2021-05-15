using System;
using System.Linq;
using System.Text;
using SqlSugar;


namespace lgs.web.api.Model.Models
{
    ///<summary>
    ///用户关注表
    ///</summary>
    [SugarTable( "user_prefers", "WMBLOG_MYSQL")]
    public class user_prefers
    {
        public user_prefers()
        {
        }
        
           [SugarColumn(IsPrimaryKey=true,IsIdentity=true)]
           public int Id { get; set; }

           public int uUID { get; set; }

           public int pUID { get; set; }
    }
}