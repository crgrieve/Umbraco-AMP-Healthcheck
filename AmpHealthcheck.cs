using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Hosting;
using Umbraco.Core.Logging;
using Umbraco.Core.Services;

namespace Umbraco.Web.HealthCheck.Checks.SEO
{
    [HealthCheck("17e136d8-10f5-4a85-825c-dcae2a9b4068", "AmpValidation",
    Description = "Check AMP validation on pages",
    Group = "SEO")]
    public class AmpHealthcheck : HealthCheck
    {
        private readonly ILocalizedTextService _textService;

        public AmpHealthcheck(HealthCheckContext healthCheckContext) : base(healthCheckContext)
        {
            _textService = healthCheckContext.ApplicationContext.Services.TextService;
        }

        public override IEnumerable<HealthCheckStatus> GetStatus()
        {
            return new[] { CheckAMP() };
        }

        public override HealthCheckStatus ExecuteAction(HealthCheckAction action)
        {
            switch (action.Alias)
            {
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private HealthCheckStatus CheckAMP()
        {

            var message = "";

            var actions = new List<HealthCheckAction>();
            var home = UmbracoContext.Current.ContentCache.GetByRoute("/");
            var success = true;
            foreach (var page in home.Descendants())
            {
                //this bool property must be on the doc type to be validated.
                //see blog post here for implementation details: http://carolelogan.net/blog/amp-implementation-in-umbraco/ 
                if (page.GetPropertyValue("hasAMP") != null)
                {
                    var hasAMP = (bool)page.GetPropertyValue("hasAMP");
                    //if the page has "hasAMP" ticked as true, validate it.
                    if (hasAMP)
                    {
                        var pageURL = page.Url;
                        var domain = HttpContext.Current.Request.Url.Host;
                        var protocol = HttpContext.Current.Request.IsSecureConnection ? "https://" : "http://";
                        var client = new WebClient();
                        var content = client.DownloadString("https://ampbench.appspot.com/raw?url=" + protocol + domain + "/" + pageURL + "?amp=1");
                        if (content.Contains("FAIL"))
                        {
                            success = false;
                            message += "AMP validation failed for page: " + pageURL;
                        }
                    }
                }
            }

            //Nothing failed, yay! Let the user know.
            if (String.IsNullOrEmpty(message))
            {
                message = "All AMP pages passed validation.";
            }
            return
                new HealthCheckStatus(message)
                {
                    ResultType = success ? StatusResultType.Success : StatusResultType.Error,
                    Actions = actions
                };
        }

       
    }
}
