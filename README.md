# Umbraco-AMP-Healthcheck

Put this file in /App_Code folder and it will add to the healthcheck dashboard.

A health check dashboard to run all your Accelerated Mobile Pages (AMP) through the validator.

Note: this doesn't make your pages AMP valid, it just checks they pass validation.

It works based on the doctypes to be validated having hasAMP true/false field and AMP markup being shown when ?amp=1 is appended to query string. Check out this blog post to see my investigations in to AMP in Umbraco: http://carolelogan.net/blog/amp-implementation-in-umbraco/


