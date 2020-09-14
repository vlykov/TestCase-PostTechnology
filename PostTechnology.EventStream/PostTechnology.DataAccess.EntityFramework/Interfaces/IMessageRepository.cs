using System.Collections.Generic;
using System.Threading.Tasks;

namespace PostTechnology.DataAccess.EntityFramework.Interfaces
{
    public interface IMessageRepository<T>
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        int GetLastMessageNumber();
        int CalculateCheckSum();

        Task<bool> Add(T customer);
        Task<bool> Update(T customer);
        Task<bool> Remove(int id);
    }
}
