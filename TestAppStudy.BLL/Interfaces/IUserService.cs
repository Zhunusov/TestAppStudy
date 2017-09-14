using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAppStudy.BLL.Infrastructure;

namespace TestAppStudy.BLL.Interfaces
{
    public interface IUserService<T> : IDisposable
    {
        Task<OperationDetails> Create(T itemDto);        
        Task<OperationDetails> Update(T itemDto);
        OperationDetails GetPersonalInfo(string username, out T itemDto);
        Task<string> GetRole(string username);
        Task<ICollection<T>> GetAll();
        Task<OperationDetails> RemoveUser(string username);
    }
}
