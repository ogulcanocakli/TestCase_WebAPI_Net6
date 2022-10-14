using Application.Interfaces.Repositories;
using Domain.Entities;
using Persistence.Context;

namespace Persistence.Repositories
{
    public class ProductRepository : EfEntityRepositoryBase<Product, CaseDbContext, int>, IProductRepository
    {
        public ProductRepository(CaseDbContext context) : base(context)
        {
        }
    }
}
