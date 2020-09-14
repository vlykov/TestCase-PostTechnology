using Microsoft.EntityFrameworkCore;
using PostTechnology.DataAccess.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostTechnology.DataAccess.EntityFramework.Repository
{
    //todo: в идеале сделать UnitOfWork и вынести туда сохранение данных в БД (context.SaveChanges())
    public class MessageRepository<T> : IMessageRepository<T> where T : Entity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public MessageRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public int GetLastNumber()
        {
            if (!_dbSet.Any())
                return 0;

            return _dbSet.Max(message => message.Number);
        }

        public int CalculateCheckSum()
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT CHECKSUM_AGG(CHECKSUM(Number,Sent,Content,Hash)) FROM SentMessages";
                    _context.Database.OpenConnection();
                    using (var result = command.ExecuteReader())
                    {
                        if(result.Read() == false)
                            return 0;
                        var checksum = result.GetInt32(0);
                        _context.Database.CloseConnection();
                        return checksum;
                        // do something with result
                    }
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }


        public async Task<T> GetById(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Add(T message)
        {
            await _dbSet.AddAsync(message);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Update(T message)
        {
            _context.Entry(message).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(message.Id))
                    return false;
                else
                    throw;
            }

            return true;
        }

        private bool MessageExists(int id)
        {
            return _dbSet.Any(e => e.Id == id);
        }

        public async Task<bool> Remove(int id)
        {
            var message = _dbSet.Find(id);
            if (message == null)
                return false;

            _dbSet.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
