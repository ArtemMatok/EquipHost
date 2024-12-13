using EquipHostWebApi.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EquipHostWebApi.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToResponse<T>(this Result<T> result)
        {
            var response = new ApiResponse<T>(result);

            if (result.IsSuccess)
            {
                return new OkObjectResult(response.Data);
            }
            else
            {
                return new BadRequestObjectResult(response);
            }
        }

    }
}
