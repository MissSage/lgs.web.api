using AspNetCoreRateLimit;
using lgs.web.api.Common;
using log4net;
using Microsoft.AspNetCore.Builder;
using System;

namespace lgs.web.api.Extensions
{
    /// <summary>
    /// ip 限流
    /// </summary>
    public static class IpLimitMildd
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(IpLimitMildd));
        public static void UseIpLimitMildd(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                if (Appsettings.app("Middleware", "IpRateLimit", "Enabled").ObjToBool())
                {
                    app.UseIpRateLimiting();
                }
            }
            catch (Exception e)
            {
                log.Error($"Error occured limiting ip rate.\n{e.Message}");
                throw;
            }
        }
    }
}
