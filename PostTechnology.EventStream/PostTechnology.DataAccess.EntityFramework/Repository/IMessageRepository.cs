using PostTechnology.DataAccess.EntityFramework.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostTechnology.DataAccess.EntityFramework.Repository
{
    public interface IMessageRepository<T>
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        int GetLastNumber();
        int CalculateCheckSum();

        Task<bool> Add(T customer);
        Task<bool> Update(T customer);
        Task<bool> Remove(int id);
    }
}
