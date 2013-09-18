namespace DevProductivity.DataTools
{
    using System;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ReflectionHelper<T>
    {
        public static string GetFullPropertyName<TV>(Expression<Func<T, TV>> fullProperty)
        {
            string fullPropertyName = string.Empty;
            Expression expression = fullProperty.Body;

            do
            {
                if (expression is ParameterExpression)
                {
                    break;
                }

                var memberExpression = expression as MemberExpression;
                if (memberExpression == null)
                {
                    var unaryExpression = expression as UnaryExpression;
                    if (unaryExpression == null)
                    {
                        throw new ArgumentException("Expression refers to a method, not a property.");
                    }

                    memberExpression = unaryExpression.Operand as MemberExpression;
                }

                if (memberExpression == null)
                {
                    throw new ArgumentException("Expression refers to a method, not a property.");
                }

                var propertyInfo = memberExpression.Member as PropertyInfo;
                if (propertyInfo == null)
                {
                    throw new ArgumentException("Expression refers to a field, not a property.");
                }

                var memberName = memberExpression.Member.Name;
                fullPropertyName = string.IsNullOrEmpty(fullPropertyName) ? memberName : string.Format(CultureInfo.InvariantCulture, "{0}.{1}", memberName, fullPropertyName);
                expression = memberExpression.Expression;
            }
            while (true);

            return fullPropertyName;
        } 
    }
}