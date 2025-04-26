using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Models;
using FluentValidation;

namespace RO.DevTest.Application.Features.Product.Commands.GetPagedProductsCommand;

public class GetPagedProductsCommandHandler(IProductRepository productRepo)
    : IRequestHandler<GetPagedProductsCommand, PagedResult<GetPagedProductResult>>
{
    private readonly IProductRepository _productRepo = productRepo;

    public async Task<PagedResult<GetPagedProductResult>> Handle(GetPagedProductsCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetPagedProductsCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

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
            .Select(p => new GetPagedProductResult
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price
            })
            .ToList();

        return new PagedResult<GetPagedProductResult>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize),
            Items = items
        };
    }
}