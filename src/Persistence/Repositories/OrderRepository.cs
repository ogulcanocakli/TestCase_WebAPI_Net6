using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class OrderRepository : EfEntityRepositoryBase<Order, CaseDbContext, int>, IOrderRepository
    {
        public OrderRepository(CaseDbContext context) : base(context)
        {
        }
    }
}
