using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Persistence.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    private readonly DefaultContext _context;

    public CustomerRepository(DefaultContext context) : base(context) => _context = context;

    public IQueryable<Customer> Query()
    {
        return _context.Set<Customer>().AsQueryable();
    }
}