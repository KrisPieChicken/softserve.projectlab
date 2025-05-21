using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.Utilities;

namespace API.Utils.Extensions;

public static class ActionResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(result.Data);

        if (result.IsNoContent)
            return new NoContentResult();

        return result.ErrorCode switch
        {
            StatusCodes.Status404NotFound => new NotFoundObjectResult(result.ErrorMessage),
            StatusCodes.Status400BadRequest => new BadRequestObjectResult(result.ErrorMessage),
            _ => new BadRequestObjectResult(result.ErrorMessage)
        };
    }
}