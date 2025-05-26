using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.Repositories.Base
{
	public interface IRepository<T>
	{
		List<T> GetAll(bool asNoTracking, params Expression<Func<T, object>>[] includes);
		List<T> GetAll(bool asNoTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllAsync(bool asNoTracking, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllAsync(bool asNoTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		List<T> GetAllWithPagination(int? pageNumber, int? pageSize, params Expression<Func<T, object>>[] includes);
		List<T> GetAllWithPagination(int? pageNumber, int? pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
		List<T> GetAllWithPagination(int? pageNumber, int? pageSize, IQueryable<T> query, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllWithPaginationAsync(int? pageNumber, int? pageSize, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllWithPaginationAsync(int? pageNumber, int? pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
		Task<List<T>> GetAllWithPaginationAsync(int? pageNumber, int? pageSize, IQueryable<T> query, params Expression<Func<T, object>>[] includes);

		T GetById(int id);

		Task<T> GetByIdAsync(int id);
		Task<T> GetByIdAsync(long id);

		T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		int Count(IQueryable<T> query);
		Task<int> CountAsync(IQueryable<T> query);

		int Count(Expression<Func<T, bool>> predicate = null);
		Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

		bool IsExist(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);

		void Add(T entity);

		Task AddAsync(T entity);

		void AddRange(IEnumerable<T> entities);

		Task AddRangeAsync(IEnumerable<T> entities);

		void Update(T entity);

		bool Delete(int id);

		void Delete(T entity);

		void DeleteRange(IEnumerable<T> entities);
		
	}
}
