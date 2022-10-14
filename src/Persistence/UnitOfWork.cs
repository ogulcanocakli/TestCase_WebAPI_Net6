using Application.Interfaces;
using Persistence.Context;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CaseDbContext _context;

        public UnitOfWork(CaseDbContext context)
        {
            _context = context;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
