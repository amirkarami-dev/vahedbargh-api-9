using Microsoft.EntityFrameworkCore;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Coreapi.Common.Enums;
using Coreapi.Common.Utility;
using Coreapi.Domain.SeedWork;
using static System.Double;
using Coreapi.Domain.AggregatesModel.ElectProjectAgg;
using Coreapi.Domain.AggregatesModel.ExecutorAgg;

namespace Coreapi.Persistence.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly CoreapiDbContext context;
        private double temp;

        protected BaseRepository(CoreapiDbContext context)
        {
            this.context = context;
        }


 
        public virtual IUnitOfWork UnitOfWork
        {
            get
            {
                return context;
            }
        }

        public static string GetDisplayNameEnum<T>(string enumValue)
        {
            Dictionary<string, string> displayNameMapping = new();
           
            if (!typeof(T).IsEnum) throw new ArgumentException("Invalid Enum");
            var displayAttributeType = typeof(DisplayAttribute);
            Enum.GetNames(typeof(T)).ToList().ForEach(name => {
                var member = typeof(T).GetMember(name).First();
                var displayAttrib = (DisplayAttribute)member.GetCustomAttributes(displayAttributeType, false).First();
                if (displayAttrib.Name == enumValue)
                    displayNameMapping.Add(displayAttrib.Name, Enum.Parse(typeof(T), name).ToString());

            });
            if (displayNameMapping.Count >= 1)
            {
               return displayNameMapping[enumValue];
            }

            return "notFound";
        
        }

        public virtual T Add(T entity)
        {
            return context.Add(entity).Entity;
        }

        public virtual async Task<T> GetById(Guid id)
        {
            return await context.Set<T>().FindAsync(id);
        }
        public virtual async Task<T> DeleteById(Guid id)
        {
            var entity = await GetById(id);
            if (entity == null) return null;
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public virtual void Update(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }
        public virtual async Task<long> GetCount()
        {
            return await context.Set<T>().LongCountAsync();
        }
        public virtual async Task<bool> IsExist(Guid id)
        {
            var entity = await context.Set<T>().FindAsync(id);

            if (entity is null)
                return false;

            return true;
        }

        public static IOrderedQueryable<TSource> ApplyOrderDirection<TSource>(IQueryable<TSource> source, string attributeName, int sortOrder)
        {
            if (string.IsNullOrEmpty(attributeName))
            {
                return source as IOrderedQueryable<TSource>;
            }

            var propertyInfo = typeof(TSource).GetProperty(attributeName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new ArgumentException("ApplyOrderDirection: The associated Attribute to the given AttributeName could not be resolved", attributeName);
            }

            Expression<Func<TSource, object>> orderExpression = x => propertyInfo.GetValue(x, null);

            return sortOrder > 0 ? source.OrderBy(orderExpression) : source.OrderByDescending(orderExpression);
        }
    }
}
