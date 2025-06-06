﻿using RO.DevTest.Domain.Entities;

namespace RO.DevTest.Application.Contracts.Persistance.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    IQueryable<Customer> Query();
}