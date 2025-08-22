namespace MaduveSiteBackend.Models.Authorization;

public static class AdminEndpointsConfig
{
    public static readonly HashSet<string> AdminOnlyEndpoints = new()
    {
        // User management endpoints
        "DELETE /api/users/{id}",
        
        // Admin management endpoints (already in AdminController)
        "POST /api/admin",
        "PUT /api/admin/{id}",
        "DELETE /api/admin/{id}",
        
        // User request management endpoints (already in AdminController)
        "POST /api/admin/requests/{requestId}/approve",
        "POST /api/admin/requests/{requestId}/reject",
        "DELETE /api/admin/requests",
        "DELETE /api/admin/requests/{id}",
        
        // Dashboard endpoints (already in AdminController)
        "GET /api/admin/dashboard",
        "GET /api/admin/requests",
        "GET /api/admin/requests/{id}",
        
        // Future admin endpoints can be added here
        // "POST /api/admin/bulk-actions",
        // "GET /api/admin/analytics",
        // "POST /api/admin/system-settings",
    };

    public static bool IsAdminOnlyEndpoint(string method, string path)
    {
        var endpoint = $"{method.ToUpper()} {path}";
        return AdminOnlyEndpoints.Contains(endpoint);
    }

    public static void AddAdminEndpoint(string method, string path)
    {
        var endpoint = $"{method.ToUpper()} {path}";
        AdminOnlyEndpoints.Add(endpoint);
    }

    public static void RemoveAdminEndpoint(string method, string path)
    {
        var endpoint = $"{method.ToUpper()} {path}";
        AdminOnlyEndpoints.Remove(endpoint);
    }
}
