using Empower.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
//using Microsoft.EntityFrameworkCore;

namespace Empower.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T>
       where T : AuditableEntity
    {
        protected DbContext _entities;
        private int _userId;

        public GenericRepository(DbContext context)
        {
            _entities = context;
        }

        public void SetUserId(int userId)
        {
            _userId = userId;
        }

        public virtual IQueryable<T> GetAll()
        {
            return _entities.Set<T>().Where(x => x.DeleteFlag == false).AsQueryable<T>();
        }

        public virtual IQueryable<T> GetAllForSync(DateTime syncDateTime)
        {
            return _entities.Set<T>().Where(x => x.UpdatedDateTime > syncDateTime).AsQueryable<T>();
        }

        public IQueryable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = _entities.Set<T>().Where(x => x.DeleteFlag == false);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public T GetById(int id)
        {
            return _entities.Set<T>().Find(id);
        }

        public IEnumerable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _entities.Set<T>().Where(predicate).AsEnumerable();
            return query;
        }

        public IQueryable<T> FindByQueryable(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _entities.Set<T>().Where(predicate).AsQueryable();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = _entities.Set<T>().Where(predicate);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public IQueryable<T> FindByQueryable(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes)
        {
            var query = _entities.Set<T>().Where(predicate);
            return includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        public virtual T Add(T entity, DateTime? UserCreatedDateTime = null)
        {
            entity.CreatedBy = _userId;
            entity.SystemCreatedDateTime = DateTime.UtcNow;
            entity.UserCreatedDateTime = DateTime.UtcNow;
            entity.UpdatedDateTime = DateTime.UtcNow;
            if (UserCreatedDateTime != null)
            {                
                entity.UserCreatedDateTime = Convert.ToDateTime(UserCreatedDateTime);
                entity.UpdatedDateTime = UserCreatedDateTime;
            }
            entity.UpdatedBy = _userId;
            entity.DeleteFlag = false;
            var data = _entities.Set<T>().Add(entity);
            Save();
            return data;
        }

        public virtual T Add(T entity, DateTime userUpdatedDateTime)
        {
            entity.CreatedBy = _userId;
            entity.SystemCreatedDateTime = DateTime.UtcNow;
            entity.UserCreatedDateTime = userUpdatedDateTime;
            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.UpdatedBy = _userId;
            entity.DeleteFlag = false;
            var data = _entities.Set<T>().Add(entity);
            Save();
            return data;
        }

        public virtual T AddWithOutSaveCall(T entity, DateTime? UserCreatedDateTime = null)
        {
            entity.CreatedBy = _userId;
            entity.SystemCreatedDateTime = DateTime.UtcNow;
            entity.UserCreatedDateTime = DateTime.UtcNow;
            entity.UpdatedDateTime = DateTime.UtcNow;
            if (UserCreatedDateTime != null)
            {
                entity.UserCreatedDateTime = Convert.ToDateTime(UserCreatedDateTime);
                entity.UpdatedDateTime = UserCreatedDateTime;
            }
            entity.UpdatedBy = _userId;
            entity.DeleteFlag = false;
            var data = _entities.Set<T>().Add(entity);
            return data;
        }

        public virtual void AddRange(T[] entities)
        {
            foreach (var entity in entities)
            {
                entity.CreatedBy = _userId;
                entity.SystemCreatedDateTime = DateTime.UtcNow;
                entity.UserCreatedDateTime = DateTime.UtcNow;
                entity.UpdatedDateTime = DateTime.UtcNow;
                entity.UpdatedBy = _userId;
                entity.DeleteFlag = false;
                var data = _entities.Set<T>().Add(entity);
            }
            Save();
        }

        public virtual void Delete(int id, object[] excludeProperties = null, DateTime? UserCreatedDateTime = null)
        {
            T entity = _entities.Set<T>().SingleOrDefault(x => x.Id == id);

            if (excludeProperties != null)
            {
                foreach (object exclude in excludeProperties)
                {
                    _entities.Entry(exclude).State = EntityState.Unchanged;
                }
            }

            entity.UpdatedBy = _userId;
            entity.UpdatedDateTime = DateTime.UtcNow;
            if (UserCreatedDateTime != null)
            {
                entity.UserCreatedDateTime = Convert.ToDateTime(UserCreatedDateTime);
                entity.UpdatedDateTime = UserCreatedDateTime;
            }
            entity.DeleteFlag = true;
            Save();
        }

        public virtual void DeleteWithOutSaveCall(int id, object[] excludeProperties = null)
        {
            T entity = _entities.Set<T>().SingleOrDefault(x => x.Id == id);

            if (excludeProperties != null)
            {
                foreach (object exclude in excludeProperties)
                {
                    _entities.Entry(exclude).State = EntityState.Unchanged;
                }
            }

            entity.UpdatedBy = _userId;
            entity.UpdatedDateTime = DateTime.UtcNow;
            entity.DeleteFlag = true;
        }

        public virtual void Edit(T entity, DateTime? UserCreatedDateTime = null)
        {
            entity.UpdatedBy = _userId;
            entity.UpdatedDateTime = DateTime.UtcNow;
            if (UserCreatedDateTime != null)
            {
                entity.UserCreatedDateTime = Convert.ToDateTime(UserCreatedDateTime);
                entity.UpdatedDateTime = UserCreatedDateTime;
            }
            Save();
        }

        public virtual void Edit(T entity, DateTime userUpdatedDateTime)
        {
            entity.UpdatedBy = _userId;
            entity.UpdatedDateTime = userUpdatedDateTime;
            Save();
        }

        public virtual void EditWithOutSaveCall(T entity, DateTime? UserCreatedDateTime = null)
        {
            entity.UpdatedBy = _userId;
            entity.UpdatedDateTime = DateTime.UtcNow;
            if (UserCreatedDateTime != null)
            {
                entity.UserCreatedDateTime = Convert.ToDateTime(UserCreatedDateTime);
                entity.UpdatedDateTime = UserCreatedDateTime;
            }
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
            //try
            //{
            //    _entities.SaveChanges();
            //}
            //catch (DbEntityValidationException ex)
            //{

            //}
        }
    }
}
