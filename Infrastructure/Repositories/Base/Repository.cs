using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Application.Repositories.Base;
using Infrastructure.Database;

namespace Infrastructure.Repositories.Base
{
	public class Repository<T> : IRepository<T> where T : class
	{
		protected readonly KycDBContext _dbContext;
		protected readonly DbSet<T> _dbSet;

		public Repository(KycDBContext dbContext)
		{
			_dbContext = dbContext;
			_dbSet = _dbContext.Set<T>();
		}

		#region IRepository Members
		public List<T> GetAll(bool asNoTracking, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes);

			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}

			return query.ToList();
		}

		public List<T> GetAll(bool asNoTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes).Where(predicate);

			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}

			return query.ToList();
		}

		public List<T> GetAll(bool asNoTracking, IQueryable<T> query, params Expression<Func<T, object>>[] includes)
		{
			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}

			return query.ToList();
		}

		public async Task<List<T>> GetAllAsync(bool asNoTracking, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes);

			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}

			return await query.ToListAsync();
		}

		public async Task<List<T>> GetAllAsync(bool asNoTracking, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes).Where(predicate);

			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}

			return await query.ToListAsync();
		}

		public async Task<List<T>> GetAllAsync(bool asNoTracking, IQueryable<T> query, params Expression<Func<T, object>>[] includes)
		{
			if (asNoTracking)
			{
				query = query.AsNoTracking();
			}

			return await query.ToListAsync();
		}

		public List<T> GetAllWithPagination(int? pageNumber, int? pageSize, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes).AsNoTracking();

			if (pageNumber != null && pageSize != null)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			return query.ToList();
		}

		public List<T> GetAllWithPagination(int? pageNumber, int? pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes).Where(predicate).AsNoTracking();

			if (pageNumber != null && pageSize != null)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			return query.ToList();
		}

		public List<T> GetAllWithPagination(int? pageNumber, int? pageSize, IQueryable<T> query, params Expression<Func<T, object>>[] includes)
		{
			if (pageNumber != null && pageSize != null)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			return query.ToList();
		}

		public async Task<List<T>> GetAllWithPaginationAsync(int? pageNumber, int? pageSize, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes).AsNoTracking();

			if (pageNumber != null && pageSize != null)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			return await query.ToListAsync();
		}

		public async Task<List<T>> GetAllWithPaginationAsync(int? pageNumber, int? pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			var query = Include(includes).Where(predicate).AsNoTracking();

			if (pageNumber != null && pageSize != null)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			return await query.ToListAsync();
		}

		public async Task<List<T>> GetAllWithPaginationAsync(int? pageNumber, int? pageSize, IQueryable<T> query, params Expression<Func<T, object>>[] includes)
		{
			if (pageNumber != null && pageSize != null)
			{
				query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
			}

			return await query.ToListAsync();
		}

		public T GetById(int id)
		{
			return _dbSet.Find(id);
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await _dbSet.FindAsync(id);
		}
		public async Task<T> GetByIdAsync(long id)
		{
			return await _dbSet.FindAsync(id);
		}
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			return Include(includes).FirstOrDefault(predicate);
		}

		public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			return await Include(includes).FirstOrDefaultAsync(predicate);
		}

		public int Count(Expression<Func<T, bool>> predicate = null)
		{
			return predicate == null ? _dbSet.Count() : _dbSet.Count(predicate);
		}

		public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
		{
			return predicate == null ? await _dbSet.CountAsync() : await _dbSet.CountAsync(predicate);
		}

		public int Count(IQueryable<T> query)
		{
			return query == null ? _dbSet.Count() : query.Count();
		}

		public async Task<int> CountAsync(IQueryable<T> query)
		{
			return query == null ? await _dbSet.CountAsync() : await query.CountAsync();
		}

		public bool IsExist(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			return Include(includes).Any(predicate);
		}

		public async Task<bool> IsExistAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
		{
			return await Include(includes).AnyAsync(predicate);
		}

		public void Add(T entity)
		{
			_dbSet.Add(entity);
		}

		public async Task AddAsync(T entity)
		{
			await _dbSet.AddAsync(entity);
		}

		public void AddRange(IEnumerable<T> entities)
		{
			_dbSet.AddRange(entities);
		}
		public async Task AddRangeAsync(IEnumerable<T> entities)
		{
			await _dbSet.AddRangeAsync(entities);
		}

		public void Update(T entity)
		{
			_dbSet.Attach(entity);
			_dbContext.Entry(entity).State = EntityState.Modified;
		}

		public bool Delete(int id)
		{
			T entity = GetById(id);
			if (entity == null) return false;
			Delete(entity);
			return true;
		}

		public void Delete(T entity)
		{
			if (entity == null) return;
			_dbSet.Remove(entity);
		}

		public void DeleteRange(IEnumerable<T> entities)
		{
			if (entities.Count() < 1) return;
			_dbSet.RemoveRange(entities);
		}

		private IQueryable<T> Include(params Expression<Func<T, object>>[] includes)
		{
			IQueryable<T> query = _dbSet;
			includes.ForEach(includeItem => query = query.Include(includeItem));
			return query;
		}

		
		#endregion
	}
}
