using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Microsoft.CSharp.RuntimeBinder;
using System.Dynamic;
using System.Reflection;
using Empower.DTO;

namespace Empower.Service.Extensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, string OrderByValue)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, SortField, null, new CSharpArgumentInfo[] {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
        });
            var prop = Expression.Dynamic(binder, typeof(T), param);
            var exp = Expression.Lambda(prop, param);
            //  string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), OrderByValue, types, q.Expression, exp);

            return q.Provider.CreateQuery<T>(mce);
        }

        public static IQueryable<T> OrderByFieldForTypeObject<T>(this IQueryable<T> q, string SortField, string OrderByValue,Type typeOfObject)
        {
            var param = Expression.Parameter(typeOfObject, "p");
            var prop = Expression.Property(param, SortField); //x.Id
            var exp = Expression.Lambda(prop, param);
            //  string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), OrderByValue, types, q.Expression, exp);

            return q.Provider.CreateQuery<T>(mce);
        }

        public static object GetPropertyValueForGrouping(object ob, string prop)
        {
            if (ob is ExpandoObject)
            {
                return ((ExpandoObject)ob).Single(e => e.Key == prop).Value.ToString();
            }
            Type type = ob.GetType();
            PropertyInfo pr = type.GetProperty(prop);
            return pr.GetValue(ob, null).ToString();
        }

        public static object GetPropertyValueForExpandoObject(object ob, string prop)
        {
            if (ob is ExpandoObject)
            {
                if (((IDictionary<String, object>)ob).ContainsKey(prop))
                {
                    // object result = ((ExpandoObject)ob).Single(e => e.Key == prop);
                    if (((ExpandoObject)ob).Single(e => e.Key == prop).Value != null)
                    {
                        return ((ExpandoObject)ob).Single(e => e.Key == prop).Value.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }

            }
            Type type = ob.GetType();
            PropertyInfo pr = type.GetProperty(prop);
            if (pr != null)
            { return pr.GetValue(ob, null).ToString(); }
            else
            {
                return "";
            }
            
        }

        public static IQueryable<T> DynamicWhere<T>(this IQueryable<T> q, string fieldName, bool isNull)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var binder = Microsoft.CSharp.RuntimeBinder.Binder.GetMember(CSharpBinderFlags.None, fieldName, null, new CSharpArgumentInfo[] {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
            });
            //var member = Expression.Property(parameter, fieldName); //x.Id
            var member = Expression.Dynamic(binder, typeof(T), parameter);
            var emptyConstant = Expression.Constant("");
            var nullConstant = Expression.Constant(null);
            
            Expression body;
            if (isNull)
            {
                var emptyexpression = Expression.Equal(member, emptyConstant);
                var nullexpression = Expression.Equal(member, nullConstant);
                body = Expression.OrElse(emptyexpression, nullexpression); //is null or is empty
            }
            else
            {
                var emptyexpression = Expression.NotEqual(member, emptyConstant);
                var nullexpression = Expression.NotEqual(member, nullConstant);
                body = Expression.AndAlso(emptyexpression, nullexpression);//is not null and is not empty
            }

            var finalExpression = Expression.Lambda<Func<T, bool>>(body, parameter);
            Type[] types = new Type[] { q.ElementType, finalExpression.Body.Type };
            var whereCallExpression = Expression.Call( typeof(Queryable),  "Where",   new Type[] { q.ElementType }, q.Expression,Expression.Quote(finalExpression));

            return q.Provider.CreateQuery<T>(whereCallExpression);
            
        }
    }
}
