using ModelContextProtocol.Server;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace App.Mcp.Tools;

[McpServerToolType]
public sealed class UserDetailsTools
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserDetailsTools(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [McpServerTool, Description("Retrieves details about the currently logged-in user from their token. / ดึงข้อมูลผู้ใช้ที่ล็อกอินอยู่จากโทเค็น")]
    [Authorize]
    public UserDetail GetMyDetails()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("No authenticated user found.");
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value
                     ?? user.FindFirst("oid")?.Value
                     ?? string.Empty;
        var username = user.FindFirst(ClaimTypes.Upn)?.Value
                       ?? user.FindFirst(ClaimTypes.Email)?.Value
                       ?? user.FindFirst("name")?.Value
                       ?? string.Empty;

        var email = user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        var firstName = user.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;
        var lastName = user.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;
        var tenantId = user.FindFirst("tid")?.Value ?? string.Empty;

        var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();

        return new UserDetail
        {
            UserId = userId,
            Username = username,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            TenantId = tenantId,
            Roles = roles,
            AllClaims = user.Claims.Select(c => new ClaimDetail { Type = c.Type, Value = c.Value }).ToList()
        };
    }

    [McpServerTool, Description("Checks if the logged-in user belongs to a specific role. / ตรวจสอบว่าผู้ใช้ที่ล็อกอินอยู่เป็นสมาชิกของบทบาทที่ระบุหรือไม่")]
    [Authorize]
    public bool IsInRole(
        [Description("Role name / ชื่อบทบาท")] string roleName)
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return false;
        }
        return user.IsInRole(roleName);
    }
}

public class ClaimDetail
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class UserDetail
{
    public string UserId { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new List<string>();
    public List<ClaimDetail> AllClaims { get; set; } = new List<ClaimDetail>();
}