﻿using System.Linq.Expressions;

namespace AgileTech_API.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);

        Task<List<T>> GetAll(Expression<Func<T,bool>>? filter = null);

        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool traked = true);

        Task Delete(T entity);

        Task Save();


    }
}
