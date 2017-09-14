using System;
using System.Collections.Generic;

namespace TestAppStudy.DAL.Interfaces
{
    public interface IUserManager<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(string username);     
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        bool Delete(string username);
    }
}
