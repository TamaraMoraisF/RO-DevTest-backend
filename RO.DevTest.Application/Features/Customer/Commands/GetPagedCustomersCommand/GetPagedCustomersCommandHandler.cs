using FluentValidation;
using MediatR;
using RO.DevTest.Application.Contracts.Persistance.Repositories;
using RO.DevTest.Application.Models;

namespace RO.DevTest.Application.Features.Customer.Commands.GetPagedCustomersCommand;

public class GetPagedCustomersCommandHandler(ICustomerRepository customerRepo)
    : IRequestHandler<GetPagedCustomersCommand, PagedResult<GetPagedCustomerResult>>
{
    private readonly ICustomerRepository _customerRepo = customerRepo;

    public async Task<PagedResult<GetPagedCustomerResult>> Handle(GetPagedCustomersCommand request, CancellationToken cancellationToken)
    {
        var validator = new GetPagedCustomersCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

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
            .Select(c => new GetPagedCustomerResult
            {
                Id = c.Id.ToString(),
                Name = c.Name,
                Email = c.Email
            })
            .ToList();

        return new PagedResult<GetPagedCustomerResult>
        {
            Page = request.Page,
            PageSize = request.PageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize),
            Items = items
        };
    }
}