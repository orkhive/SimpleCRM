using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCRM.Common.Models
{
    public class PagedQuery
    {
        const int DefaultPageSize = 25;
        const int MaxPageSize = 100;

        private int pageIndex;
        public int PageNumber
        {
            get { return pageIndex; }
            set { pageIndex = value > 0 ? value : 0; }
        }

        private int pageSize;
        /// <summary>
        /// Maximum: 100, Default: 25
        /// </summary>
        public int PageSize
        {
            get { return (pageSize > 0 && pageSize <= MaxPageSize ? pageSize : DefaultPageSize); }
            set { pageSize = value; }
        }

        public string OrderBy { get; set; } = string.Empty;

        protected int RecordsToSkip
        {
            get { return PageNumber * PageSize; }
        }

        public IQueryable<T> ApplyPaging<T>(IQueryable<T> Query)
        {
            if (!String.IsNullOrEmpty(OrderBy))
                Query = Query.OrderBy(ToLambda<T>(OrderBy));

            return Query.Skip(RecordsToSkip).Take(PageSize);
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
    }
}
