using App.AppCore.Models;
using App.AppCore.Models.Dtos;
using App.AppCore.Interfaces;
using App.AppCore.ToolInformations;
using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace App.Mcp.Tools;

[McpServerToolType]
public class CustomerTools
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerTools> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerTools(
        ICustomerService customerService,
        ILogger<CustomerTools> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _customerService = customerService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    [McpServerTool, Description(CustomerToolInformation.GetCustomerById_Description)]
    [Authorize]
    public async Task<CustomerDto?> GetCustomerById(
        [Description(CustomerToolInformation.GetCustomerById_Id_Description)] Guid id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email
        };
    }

    [McpServerTool, Description(CustomerToolInformation.GetCustomerByPhone_Description)]
    [Authorize]
    public async Task<CustomerDto?> GetCustomerByPhone(
        [Description(CustomerToolInformation.GetCustomerByPhone_Phone_Description)] string phoneNumber)
    {
        var customer = await _customerService.GetCustomerByPhoneAsync(phoneNumber);
        if (customer == null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email
        };
    }

    [McpServerTool, Description(CustomerToolInformation.GetAllCustomers_Description)]
    [Authorize]
    public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user != null)
        {
            foreach (var claim in user.Claims)
            {
                _logger.LogWarning("User claim: {Type} = {Value}", claim.Type, claim.Value);
            }
        }

        var customers = await _customerService.GetAllCustomersAsync();
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email
        });
    }

    [McpServerTool, Description(CustomerToolInformation.AddCustomer_Description)]
    [Authorize]
    public async Task AddCustomer(
        [Description(CustomerToolInformation.AddCustomer_Name_Description)] string name,
        [Description(CustomerToolInformation.AddCustomer_Phone_Description)] string phoneNumber,
        [Description(CustomerToolInformation.AddCustomer_Email_Description)] string email)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email
        };
        await _customerService.AddCustomerAsync(customer);
    }

    [McpServerTool, Description(CustomerToolInformation.UpdateCustomer_Description)]
    [Authorize]
    public async Task UpdateCustomer(
        [Description(CustomerToolInformation.UpdateCustomer_Id_Description)] Guid id,
        [Description(CustomerToolInformation.UpdateCustomer_Name_Description)] string name,
        [Description(CustomerToolInformation.UpdateCustomer_Phone_Description)] string phoneNumber,
        [Description(CustomerToolInformation.UpdateCustomer_Email_Description)] string email)
    {
        var customer = new Customer
        {
            Id = id,
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email
        };
        await _customerService.UpdateCustomerAsync(customer);
    }
}