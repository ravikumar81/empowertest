
using Empower.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Empower.Repository {
    public interface IGenericRepository<T> where T : AuditableEntity
    {
        void SetUserId(int userId);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllForSync(DateTime syncDateTime);
        IQueryable<T> GetAll(params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
        T GetById(int id);
        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindByQueryable(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
        IQueryable<T> FindByQueryable(System.Linq.Expressions.Expression<Func<T, bool>> predicate, params System.Linq.Expressions.Expression<Func<T, object>>[] includes);
        T Add(T entity, DateTime? UserCreatedDateTime = null);
        T Add(T entity, DateTime userUpdatedDateTime);
        T AddWithOutSaveCall(T entity, DateTime? UserCreatedDateTime = null);
        void Delete(int id, object[] excludeProperties = null, DateTime? UserCreatedDateTime = null);
        void DeleteWithOutSaveCall(int id, object[] excludeProperties = null);
        void Edit(T entity, DateTime? UserCreatedDateTime = null);
        void Edit(T entity, DateTime userUpdatedDateTime);
        void EditWithOutSaveCall(T entity, DateTime? UserCreatedDateTime = null);
        void Save();
    }
}
