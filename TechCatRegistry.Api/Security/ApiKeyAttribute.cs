using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechCatRegistry.Api.Security;

// JEG HAR FÅET HJÆLP AF AI TIL AT FINDE DEN RETTE MÅDE AT GØRE DETTE PÅ
// DEN HAR GIVET MIG DE MARKEREDE LINJER/INFO OM DE FUNKTIONER OSV NEDENUDEN

public class ApiKeyAttribute : Attribute, IAsyncActionFilter // AI HJULPET
{
    private const string HeaderName = "X-Api-Key"; // AI HJULPET

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) // DE 2 PARAMETER TYPER ER AI HJULPET
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var provided)) // https://stackoverflow.com/questions/42361812/unable-to-get-request-header-in-asp-net-core-web-api
        {
            context.Result = new UnauthorizedObjectResult("No API key"); // AI HJULPET
            return;
        }

        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expected = configuration["ApiKey"];

        if (string.IsNullOrEmpty(expected) || provided != expected)
        {
            context.Result = new UnauthorizedObjectResult("Wrong API key"); // AI HJULPET
            return;
        }

        await next();
    }
}