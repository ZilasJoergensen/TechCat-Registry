using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using TechCatRegistry.Data;
using TechCatRegistry.Data.Ingest;
using TechCatRegistry.Data.Parsing;

namespace TechCatRegistry.Api.Controllers;

[ApiController]
[Route("[controller]")]
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

        using var stream = file.OpenReadStream();
        var parsed = new CatalogParser().ParseFromStream(stream);

        new CatalogIngester(_db).Ingest(parsed.Rows, parsed.Version);

        return Ok(new { rows = parsed.Rows.Count, version = parsed.Version });
    }
}
