using Application.Exceptions;
using Application.Exceptions.Messages;
using Domain.Enums;
using System.Security.Claims;
using Web.AuthorizationData;

namespace Web.Extension;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal cp)
    {
        return Int32.Parse(cp.FindFirst(CustomClaimNames.UserId)!.Value);
    }

    public static void CheckAccessClaim(this ClaimsPrincipal cp, int userId)
    {
        if (!(cp.HasClaim(CustomClaimNames.UserId, userId.ToString()) ||
            cp.HasClaim(CustomClaimNames.RoleId, ((int)UserRole.Admin).ToString())))
        {
            throw new ForbiddenException(UserExceptionsMessages.ForbiddenRead);
        }
    }
}
