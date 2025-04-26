using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Models;

namespace RO.DevTest.Application.Features.Product.Queries.GetPagedProducts;

public class GetPagedProductsCommandHandler(IProductRepository productRepo)
    : IRequestHandler<GetPagedProductsCommand, PagedResult<ProductResult>>
{
    private readonly IProductRepository _productRepo = productRepo;

    public async Task<PagedResult<ProductResult>> Handle(GetPagedProductsCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepo.Query();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(request.SortBy))
        {
            query = request.SortBy.ToLower() switch
            {
                "name" => request.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "price" => request.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                _ => query
            };
        }

        var totalItems = query.Count();

        var items = query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => new ProductResult
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price
            })
            .ToList();

        return new PagedResult<ProductResult>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize),
            Items = items
        };
    }
}