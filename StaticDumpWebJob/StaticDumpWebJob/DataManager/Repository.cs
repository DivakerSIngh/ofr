using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using StaticDumpWebJob.Models;
using EntityFramework.BulkInsert.Extensions;
using System.Linq.Expressions;
using StaticDumpWebJob.DatabaseContext;
using System.Configuration;

namespace StaticDumpWebJob.DataManagers
{
    public class Repository<T> where T : class
    {
        private readonly OFR_DataContext _context;
        private IDbSet<T> _entities;

        public Repository()
        {
            this._context = new OFR_DataContext();
        }

        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public virtual T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        public T Get(Expression<Func<T, bool>> @where)
        {
            return Entities.Where(where).FirstOrDefault();
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Add(entity);

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5)))
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;

                    if (entities.Count() > 300000)
                    {
                        var firstChunk = entities.Take(300000);
                        _context.BulkInsert(firstChunk);
                        _context.SaveChanges();

                        var nextChunk = entities.Skip(300000);
                        _context.BulkInsert(nextChunk);
                        _context.SaveChanges();
                    }
                    else
                    {
                        _context.BulkInsert(entities);
                        _context.SaveChanges();
                    }


                    transactionScope.Complete();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual async Task InsertAsync(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                using (var transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5)))
                {
                    _context.Configuration.AutoDetectChangesEnabled = false;

                    if (entities.Count() > 300000)
                    {
                        var firstChunk = entities.Take(300000);
                        _context.BulkInsert(firstChunk);
                        await _context.SaveChangesAsync();

                        var nextChunk = entities.Skip(300000);
                        _context.BulkInsert(nextChunk);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _context.BulkInsert(entities);
                        await _context.SaveChangesAsync();
                    }


                    transactionScope.Complete();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }


        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }

                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    _context.Set<T>().Attach(entity);
                }
                _context.Entry(entity).State = EntityState.Modified;

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                foreach (var item in entities)
                {
                    if (_context.Entry(item).State == EntityState.Detached)
                    {
                        _context.Set<T>().Attach(item);
                    }
                }
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }


        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                this.Entities.Remove(entity);

                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                {
                    throw new ArgumentNullException("entities");
                }
                foreach (var entity in entities)
                {
                    this.Entities.Remove(entity);
                }
                this._context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }


        public IQueryable<T> GetAll()
        {
            return this.Entities;
        }


        public IQueryable<T> GetMany(Expression<Func<T, bool>> @where)
        {
            return this.Entities.Where(where);
        }


        /// <summary>
        /// Entities
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities;
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
            {
                foreach (var error in validationErrors.ValidationErrors)
                {
                    msg += string.Format("VenderId: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
                }
            }
            return msg;
        }

        public void Truncate()
        {
            _context.Database.ExecuteSqlCommand(string.Format("DELETE FROM [dbo].[{0}]", typeof(T).Name));
            try
            {
                _context.Database.ExecuteSqlCommand(string.Format("DBCC CHECKIDENT ('[dbo].[{0}]', RESEED, 0)", typeof(T).Name));
            }
            catch (Exception)
            {           }
            
        }

        #endregion
    }
}
