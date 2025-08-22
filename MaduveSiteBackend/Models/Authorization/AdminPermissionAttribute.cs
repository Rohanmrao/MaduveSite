using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MaduveSiteBackend.Services;

namespace MaduveSiteBackend.Models.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminPermissionAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var adminService = context.HttpContext.RequestServices.GetService<IAdminService>();
        if (adminService == null)
        {
            context.Result = new StatusCodeResult(500);
            return;
        }

        var adminId = GetAdminIdFromRequest(context);
        if (adminId == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var isAdmin = await adminService.IsAdminAsync(adminId.Value);
        if (!isAdmin)
        {
            context.Result = new ForbidResult();
            return;
        }
    }

    private Guid? GetAdminIdFromRequest(AuthorizationFilterContext context)
    {
        // Try to get admin ID from query string
        if (context.HttpContext.Request.Query.TryGetValue("adminId", out var adminIdQuery))
        {
            if (Guid.TryParse(adminIdQuery, out var adminId))
                return adminId;
        }

        // Try to get admin ID from route values
        if (context.RouteData.Values.TryGetValue("adminId", out var adminIdRoute))
        {
            if (Guid.TryParse(adminIdRoute.ToString(), out var adminId))
                return adminId;
        }

        // Try to get admin ID from headers
        if (context.HttpContext.Request.Headers.TryGetValue("X-Admin-Id", out var adminIdHeader))
        {
            if (Guid.TryParse(adminIdHeader, out var adminId))
                return adminId;
        }

        // For DELETE operations, try to get admin ID from query string as fallback
        if (context.HttpContext.Request.Method == "DELETE")
        {
            if (context.HttpContext.Request.Query.TryGetValue("adminId", out var adminIdQueryDelete))
            {
                if (Guid.TryParse(adminIdQueryDelete, out var adminId))
                    return adminId;
            }
        }

        return null;
    }
}
