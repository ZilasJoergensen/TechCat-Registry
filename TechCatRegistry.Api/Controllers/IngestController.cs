using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using TechCatRegistry.Data;
using TechCatRegistry.Data.Ingest;
using TechCatRegistry.Data.Parsing;
using TechCatRegistry.Api.Security;

namespace TechCatRegistry.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ApiKey]
public class IngestController : ControllerBase
{
    private readonly TechCatDbContext _db;

    public IngestController(TechCatDbContext db) 
    { 
        _db = db;
    }
    
    [HttpPost("upload")]
    public IActionResult Upload(IFormFile file)
    {
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.iformfile?view=aspnetcore-10.0
        if (file == null || file.Length == 0)
            return BadRequest("No File");

        if (!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            return BadRequest("Wrong file ending type thing");

        ParsedCatalog parsed;

        try
        {
            using var stream = file.OpenReadStream();
            parsed = new CatalogParser().ParseFromStream(stream);
        }
        catch (ArgumentException)
        {
            return BadRequest("Expected ark missing");
        }
        catch (KeyNotFoundException)
        {
            return BadRequest("Ark collumns not as expected");
        }

        try
        {
            new CatalogIngester(_db).Ingest(parsed.Rows, parsed.Version);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }


        return Ok(new { rows = parsed.Rows.Count, version = parsed.Version });
    }
}
