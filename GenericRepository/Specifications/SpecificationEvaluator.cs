using GenericRepository.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace GenericRepository.Specifications
{
    public class SpecificationEvaluator<TEntity, TIdentity>
         where TEntity : BaseEntity<TIdentity>
         where TIdentity : IComparable, IComparable<TIdentity>, IEquatable<TIdentity>//IConvertible, 
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity>? specification)
        {
            var query = inputQuery;
            if (specification == null)
            {
                return query;
            }
            if (specification.Include != null)
            {
                query = specification.Include(query);
            }
            if (specification.AsNoTracking)
                query = query.AsNoTracking();

            if (specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            if (specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPagingEnabled)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }
            //query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }
    }
}
