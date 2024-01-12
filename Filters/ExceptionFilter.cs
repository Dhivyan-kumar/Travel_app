using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Webapplication.Filters
{
    public class ExceptionFilter:ActionFilterAttribute,IExceptionFilter
    {
        public  void OnException(ExceptionContext filterContext)
        {
            Exception e = filterContext.Exception;
            filterContext.ExceptionHandled=true;
            filterContext.Result=new ViewResult()
            {
                ViewName="SomeException"
            };
        }

    }
}