using BackEnd.Application.Common.Interfaces;
using BackEnd.Application.DTOs;

namespace BackEnd.Application.Services;

public class CustomerService
{
    private readonly IApplicationDbContext _context;

    public CustomerService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CustomerDto>> GetAllCustomersAsync()
    {
        return await _context.Customers
            .Select(c => new CustomerDto
            {
                Id = c.Id,
                UserName = c.UserName,
                Email = c.Email,
                Phone = c.Phone,
                Role = c.Role
            })
            .ToListAsync();
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            UserName = customer.UserName,
            Email = customer.Email,
            Phone = customer.Phone,
            Role = customer.Role
        };
    }
}
