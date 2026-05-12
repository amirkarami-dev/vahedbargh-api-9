using System;
using System.Linq;
using System.Reflection;
using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Coreapi.Api.Attributes
{

    public class SanitizeInputFilter:IActionFilter
    {

        private readonly IHtmlSanitizer _sanitizer;

        public SanitizeInputFilter(IHtmlSanitizer sanitizer)
        {
            _sanitizer = sanitizer;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionArguments != null)
            {
                foreach (var parameter in filterContext.ActionArguments.ToList())
                {
                    if (parameter.Value != null)
                    {
                        var type = parameter.Value.GetType();
                        if (type == typeof(string))
                        {
                            var sanitized = _sanitizer.Sanitize(parameter.Value.ToString());
                            filterContext.ActionArguments[parameter.Key] = sanitized;
                        }
                        else
                        {
                            var properties = parameter.Value.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Where(x => x.CanRead && x.CanWrite && x.PropertyType == typeof(string) &&
                                            x.GetGetMethod(true).IsPublic && x.GetSetMethod(true).IsPublic);



                            foreach (var propertyInfo in properties)
                            {
                                if (propertyInfo.GetValue(parameter.Value) != null)
                                    propertyInfo.SetValue(parameter.Value,
                                        _sanitizer.Sanitize(propertyInfo.GetValue(parameter.Value).ToString()));
                            }
                        }
                    }
                }
            }

        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }
    }
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SanitizeInputAttribute:TypeFilterAttribute
    {
        public SanitizeInputAttribute() : base(typeof(SanitizeInputFilter))
        {
        }
    }
}
