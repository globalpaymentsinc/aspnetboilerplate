using System;
using System.Web;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;
using Castle.Core.Logging;

namespace Abp.Web.MultiTenancy
{
    public class HttpHeaderTenantResolveContributor : ITenantResolveContributor, ITransientDependency
    {
        public ILogger Logger { get; set; }

        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public HttpHeaderTenantResolveContributor(IMultiTenancyConfig multiTenancyConfig)
        {
            _multiTenancyConfig = multiTenancyConfig;

            Logger = NullLogger.Instance;
        }

        public int? ResolveTenantId()
        {
            var httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return null;
            }

            try
            {
                var tenantIdHeader = httpContext.Request.Headers[_multiTenancyConfig.TenantIdResolveKey];
                if (tenantIdHeader.IsNullOrEmpty())
                {
                    return null;
                }


                return int.TryParse(tenantIdHeader, out var tenantId) ? tenantId : (int?)null;
            }
            catch (HttpException)
            {
                /* Workaround:
                 * Accessing HttpContext.Request during Application_Start or Application_End will throw exception.
                 * This behavior is intentional from microsoft
                 * See https://stackoverflow.com/questions/2518057/request-is-not-available-in-this-context/23908099#comment2514887_2518066
                 */
                Logger.Warn("HttpContext.Request access when it is not suppose to");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message);
                return null;
            }
        }
    }
}
