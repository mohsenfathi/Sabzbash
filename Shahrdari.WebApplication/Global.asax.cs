using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;

namespace Shahrdari.WebApplication
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //RouteTable.Routes.MapHubs();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;
            if (httpException != null)
            {
                RouteData routeData = new RouteData();
                routeData.Values.Add("controller", "Common");
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        routeData.Values.Add("action", "Error404");
                        break;
                    //case 500:
                    //    // server error
                    //    routeData.Values.Add("action", "HttpError500");
                    //    break;
                    default:
                        routeData.Values.Add("action", "Index");
                        break;
                }
                //routeData.Values.Add("error", exception.Message);
                // clear error on server
                Server.ClearError();

                Response.RedirectToRoute(routeData.Values);
                // at this point how to properly pass route data to error controller?
                //Response.Redirect(String.Format("~/Error/{0}/?message={1}", "Index", exception.Message));
            }
        }
    }
}