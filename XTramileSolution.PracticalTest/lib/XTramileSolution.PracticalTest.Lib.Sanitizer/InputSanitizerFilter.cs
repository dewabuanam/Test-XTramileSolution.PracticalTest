using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace XTramileSolution.PracticalTest.Lib.Sanitizer
{
    public class InputSanitizerFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var param in context.ActionArguments)
            {
                if (param.Value is string strValue)
                {
                    context.ActionArguments[param.Key] = Sanitize(strValue);
                }
                else if (param.Value != null)
                {
                    SanitizeObject(param.Value);
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // No action needed after execution
        }

        private static string Sanitize(string input)
        {
            return string.IsNullOrWhiteSpace(input)
                ? string.Empty
                : Regex.Replace(input, @"[<>""'/]", string.Empty).Trim();
        }

        private static void SanitizeObject(object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                if (prop.PropertyType == typeof(string) && prop.CanWrite)
                {
                    var value = (string)prop.GetValue(obj);
                    prop.SetValue(obj, Sanitize(value));
                }
            }
        }
    }
}