using NetBet.DataAccess.Models;
using System.Linq.Expressions;

namespace NetBet.Infrastracture
{
    public class ExpressionHelper
    {
        public static Expression<Func<TInstance, bool>> CreateCondition<TInstance>(Expression<Func<TInstance, bool>> existing, Expression<Func<TInstance, bool>> newCondition) where TInstance:class
        {
            if (existing == null)
            {
                return newCondition;
            }
            else
            {
                var parameter = Expression.Parameter(typeof(TInstance));
                var combinedBody = Expression.AndAlso(
                    Expression.Invoke(existing, parameter),
                    Expression.Invoke(newCondition, parameter)
                );

                return Expression.Lambda<Func<TInstance, bool>>(combinedBody, parameter);
            }
        }
    }
}
