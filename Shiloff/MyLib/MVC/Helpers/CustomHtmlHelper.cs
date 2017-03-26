using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Helpers
{
    public static class ExtendedHtmlHelper
    {
        public static string CustomLink<T>(string link, Expression<Func<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var expr = expression.Body as MemberInitExpression;
            if (expr == null)
            {
                throw new InvalidOperationException($"expression.Body must be MemberInitExpression type.");
            }

            var paramParts = new List<string>();
            var obj = expression.Compile().Invoke();
            var objType = obj.GetType();
            foreach (var binding in expr.Bindings)
            {
                var property = objType.GetProperty(binding.Member.Name);
                var value = property.GetValue(obj);
                if (value == null) continue;
                if (typeof (IEnumerable).IsAssignableFrom(property.PropertyType) 
                    && !typeof (string).IsAssignableFrom(property.PropertyType))
                {
                    foreach (var val in (IEnumerable) value)
                    {
                        paramParts.Add($"{property.Name.ToLower()}={val.ToString().ToLower()}");
                    }
                }
                else
                {
                    paramParts.Add($"{property.Name.ToLower()}={value.ToString().ToLower()}");
                }
            }

            return $"{link}?{string.Join("&", paramParts)}";
        }
    }
}
