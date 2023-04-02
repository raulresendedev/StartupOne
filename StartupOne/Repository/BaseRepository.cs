using StartupOne.Mapping;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace StartupOne.Repository
{
    public abstract class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly DataBaseContext _dbContext = new DataBaseContext();

        public virtual void Adicionar(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
        }

        public virtual void Atualizar(TEntity entity)
        {
            _dbContext.Set<TEntity>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public virtual void Remover(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public virtual TEntity Obter(int id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }
    }
}
