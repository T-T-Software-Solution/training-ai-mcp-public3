using App.AppCore.Applications;
using App.AppCore.ToolInformations;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Mcp;
using Microsoft.Extensions.Logging;

namespace App.Function.Tools;

public class UtilityFuncTool
{
    private readonly ILogger<UtilityFuncTool> _logger;

    public UtilityFuncTool(ILogger<UtilityFuncTool> logger)
    {
        _logger = logger;
    }

    [Function(nameof(GetHashByName))]
    public string GetHashByName(
        [McpToolTrigger("GetHashByName", "Get a SHA256 hash from the provided name. / รับค่าแฮช SHA256 จากชื่อที่ระบุ")] ToolInvocationContext context,
        [McpToolProperty("name", "string", "The name to hash. / ชื่อที่ต้องการแฮช")] string name,
        [McpToolProperty("userId", "string", CommonToolInformation.UserId_Description)] string? userId = null,
        [McpToolProperty("userRole", "string", CommonToolInformation.UserRole_Description)] string? userRole = null)
    {
        var hash = HashUtility.ComputeSha256(name);
        _logger.LogInformation("Generated hash for name '{Name}': {Hash}", name, hash);
        return $"Hello, {name}\nHash: {hash}";
    }
}
