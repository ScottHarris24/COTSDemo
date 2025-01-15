using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Collections.Concurrent;
using COTSDemo.Abstractions.Entities;

namespace COTSDemo.Services;


// NOTE: These classes were taken from the sample at:
//       https://stackoverflow.com/questions/76102803/how-to-convert-type-expression-to-work-on-different-type-in-c-sharp
//
//       The code was refactored to make the code a little cleaner
//       and to follow the coding standards for the project.


public static class ExpressionExt
{
    public static Expression<Func<TTo, bool>> ReplaceLambdaParameter<TFrom, TTo>(this Expression<Func<TFrom, bool>> orig) =>
        (Expression<Func<TTo, bool>>)(new LambdaParameterReplacer<TFrom, TTo>().Visit(orig));
}


public class LambdaParameterReplacer<TFrom, TTo> : ExpressionVisitor
{

    static ConcurrentDictionary<string, Dictionary<string, string>> concurrentDictionaryMaps = new ConcurrentDictionary<string, Dictionary<string, string>>();

    private ParameterExpression _oldParameterExpression;

    private readonly Dictionary<string, string> _parameterMap;
    private readonly ParameterExpression _newParameterExpression = Expression.Parameter(typeof(TTo), "TTo");
    public LambdaParameterReplacer()
    {
        _oldParameterExpression = _newParameterExpression;
        _parameterMap = GetParameterMap();
    }

    private Dictionary<string, string> GetParameterMap()
    {
        var name = typeof(TTo).Name;
        var map = concurrentDictionaryMaps.GetOrAdd(name, _ =>
            {
                var dictionary = name switch
                {
                    nameof(CustomerEntity) => new Dictionary<string, string>
                    {
                        { "LastName", "LastName" },
                        { "FirstName", "FirstName" },
                        { "BillingAddress", "BillingAddress" },
                        { "ShippingAddress", "ShippingAddress" },
                    },

                    nameof(OrderEntity) => new Dictionary<string, string>
                    {
                        { "CustomerId", "CustomerId" },
                        { "OrderDate", "OrderDate" },
                        { "ShippedDate", "ShippedDate" },
                    },

                    nameof(OrderDetailEntity) => new Dictionary<string, string>
                    {
                        { "OrderId", "OrderId" },
                        { "Quantity", "Quantity" },
                        { "ProductId", "ProductId" },
                    },

                    _ => new Dictionary<string, string>()
                };

                // Add common properties
                if (dictionary.Count > 0)
                {
                    dictionary.Add("Id", "Id");
                    dictionary.Add("Created", "Created");
                    dictionary.Add("CreatedBy", "CreatedBy");
                    dictionary.Add("LastUpdated", "LastUpdated");
                    dictionary.Add("LastUpdatedBy", "LastUpdatedBy");

                }
                return dictionary;
            });

        return map;
    }

    [return: NotNullIfNotNull("node")]
    public override Expression Visit(Expression? node)
    {
        if (node is Expression<Func<TFrom, bool>> lambdaExpr)
        {
            _oldParameterExpression = lambdaExpr.Parameters[0]; // save original parameter to replace later
        }

        return base.Visit(node!);
    }

    protected override Expression VisitLambda<T>(Expression<T> lambdaExpr)
        => Expression.Lambda(Visit(lambdaExpr.Body), _newParameterExpression);

    protected override Expression VisitParameter(ParameterExpression parmExpr)
    {
        if (parmExpr == _oldParameterExpression)
        {
            return _newParameterExpression;
        }
        else
        {
            var expression = base.VisitParameter(parmExpr);
            return expression;
        }
    }
    protected override Expression VisitMember(MemberExpression memberExpr)
    {
        if (memberExpr.Expression == _oldParameterExpression)
        {
            return Expression.Property(_newParameterExpression, _parameterMap[memberExpr.Member.Name]);
        }
        else
        {
            return base.Visit(memberExpr);
        }
    }
}