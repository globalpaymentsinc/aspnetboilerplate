using System;
using Abp.Dependency;
using Abp.EntityHistory;
using Abp.Runtime;
using JetBrains.Annotations;
using System.Web;
using System.Security.Policy;

namespace Abp.Web.EntityHistory
{
    /// <summary>
    /// Implements <see cref="IEntityChangeSetReasonProvider"/> to get reason from HTTP request.
    /// </summary>
    public class HttpRequestEntityChangeSetReasonProvider : EntityChangeSetReasonProviderBase, ISingletonDependency
    {
        [CanBeNull]
        public override string Reason
        {
            get
            {
                if (OverridedValue != null)
                {
                    return OverridedValue.Reason;
                }

                try
                {
                    return Uri.TryCreate(HttpContext.Current?.Request.Url.AbsoluteUri, UriKind.Absolute, out var uri) ? uri.ToString() : null;
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

        public HttpRequestEntityChangeSetReasonProvider(
            IAmbientScopeProvider<ReasonOverride> reasonOverrideScopeProvider
            ) : base(reasonOverrideScopeProvider)
        {
        }
    }
}
