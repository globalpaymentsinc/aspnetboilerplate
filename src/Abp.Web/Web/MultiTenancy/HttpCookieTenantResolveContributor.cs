using System;
using System.Web;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Extensions;
using Abp.MultiTenancy;
using Castle.Core.Logging;

namespace Abp.Web.MultiTenancy
{
    public class HttpCookieTenantResolveContributor : ITenantResolveContributor, ITransientDependency
    {
        private readonly IMultiTenancyConfig _multiTenancyConfig;

        public ILogger Logger { get; set; }

        public HttpCookieTenantResolveContributor(IMultiTenancyConfig multiTenancyConfig)
        {
            _multiTenancyConfig = multiTenancyConfig;

            Logger = NullLogger.Instance;
        }

        public int? ResolveTenantId()
        {
            try
            {
                var cookie = HttpContext.Current?.Request.Cookies[_multiTenancyConfig.TenantIdResolveKey];
                if (cookie == null || cookie.Value.IsNullOrEmpty())
                {
                    return null;
                }

                return int.TryParse(cookie.Value, out var tenantId) ? tenantId : (int?)null;
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
