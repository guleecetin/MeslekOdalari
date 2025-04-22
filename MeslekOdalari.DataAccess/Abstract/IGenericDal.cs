﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MeslekOdalari.DataAccess.Abstract
{
    public interface IGenericDal<T>where T:class
    {
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(ObjectId id);
        Task<List<T>> GetListAsync();
        Task<T> GetByIdAsync(Object id);
        Task<int> CountAsync();
        Task<List<T>> GetFilteredListAsync(Expression<Func<T,bool>> predicate);

    }
}
