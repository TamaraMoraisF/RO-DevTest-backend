using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Features.Queries.GetPagedCustomers;
using RO.DevTest.Application.Models;

namespace RO.DevTest.Application.Features.Customer.Queries.GetPagedCustomers;

public class GetPagedCustomersQueryHandler(ICustomerRepository customerRepo)
    : IRequestHandler<GetPagedCustomersQuery, PagedResult<CustomerResult>>
{
    private readonly ICustomerRepository _customerRepo = customerRepo;

    public async Task<PagedResult<CustomerResult>> Handle(GetPagedCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = _customerRepo.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(c => c.Name.Contains(request.Search) || c.Email.Contains(request.Search));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "name" => request.Descending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
                "email" => request.Descending ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
                _ => query
            };
        }

        var totalItems = await Task.FromResult(query.Count());

        var items = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CustomerResult
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Email = c.Email
            })
            .ToList();

        return new PagedResult<CustomerResult>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize),
            Items = items
        };
    }
}