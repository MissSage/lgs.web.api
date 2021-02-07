using AutoMapper;
using lgs.web.api.Model.Models;
using lgs.web.api.Model.ViewModels;

namespace lgs.web.api.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<blogarticle, BlogViewModels>();
            CreateMap<BlogViewModels, blogarticle>();
        }
    }
}
