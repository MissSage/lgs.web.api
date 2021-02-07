using lgs.web.api.IServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace lgs.web.api.Filter
{
    public class UseServiceDIAttribute : ActionFilterAttribute
    {

        protected readonly ILogger<UseServiceDIAttribute> _logger;
        private readonly IblogarticleServices _blogarticleServices;
        private readonly string _name;

        public UseServiceDIAttribute(ILogger<UseServiceDIAttribute> logger, IblogarticleServices blogarticleServices,string Name="")
        {
            _logger = logger;
            _blogarticleServices = blogarticleServices;
            _name = Name;
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            //var dd =await _blogarticleServices.Query();
            base.OnActionExecuted(context);
            DeleteSubscriptionFiles();
        }

        private void DeleteSubscriptionFiles()
        {
           
        }
    }
}
