using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Runtime.Caching;
using System.Web.Http.Filters;

namespace HeatMapWebApi.Custom_Attributes
{
    public class WebApiOutputCacheAttribute : ActionFilterAttribute
    {
        // cache length in seconds
        private readonly int timespan;
        // client cache length in seconds
        private readonly int clientTimeSpan;
        // cache for anonymous users only?
        private readonly bool anonymousOnly;
        // cache key
        private string cachekey;
        // cache repository
        private static readonly ObjectCache WebApiCache = MemoryCache.Default;

        public WebApiOutputCacheAttribute(int timespan, int clientTimeSpan, bool anonymousOnly)
        {
            this.timespan = timespan;
            this.clientTimeSpan = clientTimeSpan;
            this.anonymousOnly = anonymousOnly;
        }

        private bool IsCacheable(HttpActionContext actionContext)
        {
            if (timespan > 0 && clientTimeSpan > 0)
            {
                if (anonymousOnly)
                    if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                        return false;
                if (actionContext.Request.Method == HttpMethod.Get)
                    return true;
            }
            else
            {
                throw new InvalidOperationException("Wrong Arguments");
            }
            return false;
        }

        private CacheControlHeaderValue SetClientCache()
        {
            var cacheControl = new CacheControlHeaderValue
                                   {
                                       MaxAge = TimeSpan.FromSeconds(clientTimeSpan),
                                       MustRevalidate = true
                                   };
            return cacheControl;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext != null)
            {
                if (IsCacheable(actionContext))
                {
                    cachekey = string.Join(":", new string[] { actionContext.Request.RequestUri.AbsolutePath, actionContext.Request.Headers.Accept.FirstOrDefault().ToString() });
                    if (WebApiCache.Contains(cachekey))
                    {
                        var val = (string)WebApiCache.Get(cachekey);
                        if (val != null)
                        {
                            actionContext.Response = actionContext.Request.CreateResponse();
                            actionContext.Response.Content = new StringContent(val);

                            var contenttype = (MediaTypeHeaderValue)WebApiCache.Get(cachekey + ":response-ct") ?? new MediaTypeHeaderValue(cachekey.Split(':')[1]);

                            actionContext.Response.Content.Headers.ContentType = contenttype;
                            actionContext.Response.Headers.CacheControl = SetClientCache();
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("actionContext");
            }
        }

        //need to add latest version of HttpRequestMessageExtensions.cs
        //public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        //{
        //    if (!(WebApiCache.Contains(cachekey)))
        //    {
        //        var body = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;
        //        WebApiCache.Add(cachekey, body, DateTime.Now.AddSeconds(timespan));
        //        WebApiCache.Add(cachekey + ":response-ct", actionExecutedContext.Response.Content.Headers.ContentType, DateTime.Now.AddSeconds(timespan));
        //    }
        //    if (IsCacheable(actionExecutedContext.ActionContext))
        //        actionExecutedContext.ActionContext.Response.Headers.CacheControl = SetClientCache();
        //}
    }
}