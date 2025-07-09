using App.AppCore.Interfaces;
using App.AppCore.Models;
using App.AppCore.Models.Dtos;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace App.Function.Tools;

public class CustomerFuncTool
{
    private readonly ICustomerService _customerService;
    private readonly ILogger<CustomerFuncTool> _logger;

    public CustomerFuncTool(ICustomerService customerService, ILogger<CustomerFuncTool> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }

    [Function(nameof(GetCustomerById))]
    public async Task<CustomerDto?> GetCustomerById(
        [McpToolTrigger("GetCustomerById", CustomerToolInformation.GetCustomerById_Description)] ToolInvocationContext context,
        [McpToolProperty("id", "string", CustomerToolInformation.GetCustomerById_Id_Description + " (send as Guid string)")] string id,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole)
    {
        if (!Guid.TryParse(id, out var guidId))
            return null;
        var customer = await _customerService.GetCustomerByIdAsync(guidId);
        if (customer == null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email
        };
    }

    [Function(nameof(GetCustomerByPhone))]
    public async Task<CustomerDto?> GetCustomerByPhone(
        [McpToolTrigger("GetCustomerByPhone", CustomerToolInformation.GetCustomerByPhone_Description)] ToolInvocationContext context,
        [McpToolProperty("phoneNumber", "string", CustomerToolInformation.GetCustomerByPhone_Phone_Description)] string phoneNumber,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole)
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

    [Function(nameof(GetAllCustomers))]
    public async Task<IEnumerable<CustomerDto>> GetAllCustomers(
        [McpToolTrigger("GetAllCustomers", CustomerToolInformation.GetAllCustomers_Description)] ToolInvocationContext context,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole)
    {
        _logger.LogWarning("GetAllCustomers called by userId: {UserId}, userRole: {UserRole}", userId, userRole);

        var customers = await _customerService.GetAllCustomersAsync();
        return customers.Select(c => new CustomerDto
        {
            Id = c.Id,
            Name = c.Name,
            PhoneNumber = c.PhoneNumber,
            Email = c.Email
        });
    }

    [Function(nameof(AddCustomer))]
    public async Task AddCustomer(
        [McpToolTrigger("AddCustomer", CustomerToolInformation.AddCustomer_Description)] ToolInvocationContext context,
        [McpToolProperty("name", "string", CustomerToolInformation.AddCustomer_Name_Description)] string name,
        [McpToolProperty("phoneNumber", "string", CustomerToolInformation.AddCustomer_Phone_Description)] string phoneNumber,
        [McpToolProperty("email", "string", CustomerToolInformation.AddCustomer_Email_Description)] string email,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole)
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

    [Function(nameof(UpdateCustomer))]
    public async Task UpdateCustomer(
        [McpToolTrigger("UpdateCustomer", CustomerToolInformation.UpdateCustomer_Description)] ToolInvocationContext context,
        [McpToolProperty("id", "string", CustomerToolInformation.UpdateCustomer_Id_Description + " (send as Guid string)")] string id,
        [McpToolProperty("name", "string", CustomerToolInformation.UpdateCustomer_Name_Description)] string name,
        [McpToolProperty("phoneNumber", "string", CustomerToolInformation.UpdateCustomer_Phone_Description)] string phoneNumber,
        [McpToolProperty("email", "string", CustomerToolInformation.UpdateCustomer_Email_Description)] string email,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole)
    {
        if (!Guid.TryParse(id, out var guidId))
            return;
        var customer = new Customer
        {
            Id = guidId,
            Name = name,
            PhoneNumber = phoneNumber,
            Email = email
        };
        await _customerService.UpdateCustomerAsync(customer);
    }
}
