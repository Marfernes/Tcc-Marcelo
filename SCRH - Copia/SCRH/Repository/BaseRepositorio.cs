using SCRH.Context;
using System;
using System.Data.Entity;

namespace SCRH.Repository
{
    public class BaseRepositorio<TEntity> : IDisposable where TEntity : class
    {
        protected ConexaoBanco Context;
        protected DbSet<TEntity> DbSet;

        public BaseRepositorio()
        {
            Context = new ConexaoBanco();
            DbSet = Context.Set<TEntity>();
        }
        public BaseRepositorio(ConexaoBanco db)
        {
            Context = db;
            DbSet = Context.Set<TEntity>();
        }

        public virtual TEntity Adicionar(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Added;
            Context.SaveChanges();
            return entity;
        }

        public virtual TEntity Atualizar(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }

        public virtual TEntity Excluir(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();
            return entity;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Context.Dispose();
        }
    }
}