using System.Linq;
using PagedList.Core;

namespace CompetencyMatrix.Models.Extension
{
    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedListByQuery<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return new PagedList<T>(superset, pageNumber, pageSize);
        }
    }
}