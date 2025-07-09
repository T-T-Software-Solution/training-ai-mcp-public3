using App.AppCore.Applications;
using ModelContextProtocol.Server;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;

namespace App.Mcp.Tools;

[McpServerToolType]
public class UtilityTools
{
    private readonly ILogger<UtilityTools> _logger;

    public UtilityTools(ILogger<UtilityTools> logger)
    {
        _logger = logger;
    }

    [McpServerTool, Description("Get a SHA256 hash from the provided name. / รับค่าแฮช SHA256 จากชื่อที่ระบุ")]
    [Authorize]
    public string GetHashByName(
        [Description("The name to hash. / ชื่อที่ต้องการแฮช")] string name)
    {
        var hash = HashUtility.ComputeSha256(name);

        _logger.LogInformation("Generated hash for name '{Name}': {Hash}", name, hash);

        return $"Hello, {name}\nHash: {hash}";
    }
}