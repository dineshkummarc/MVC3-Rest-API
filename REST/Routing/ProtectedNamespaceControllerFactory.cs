using System;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Web.Routing;

namespace REST.Routing
{
    public class ProtectedNamespaceControllerFactory : DefaultControllerFactory
    {
        private NameValueCollection _matches;

        public ProtectedNamespaceControllerFactory(NameValueCollection matches)
        {
            _matches = matches;
        }
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            // When wildcard mapping is turned on, hits going to favicon.ico comes to ASP.NET MVC
            if (controllerName == "favicon.ico") return null;

            IController controller = base.CreateController(requestContext, controllerName);
            foreach (string key in _matches.Keys)
                if (controller.GetType().Namespace == key)
                    if (requestContext.HttpContext.Request.Url.AbsolutePath
                        .StartsWith(_matches[key], StringComparison.OrdinalIgnoreCase))
                        return controller;
                    else
                        return null;

            return controller;
        }
    }
}
