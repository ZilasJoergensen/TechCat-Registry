using Microsoft.AspNetCore.Mvc;
using TechCatRegistry.Data;

namespace TechCatRegistry.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogsController : ControllerBase
{
    private readonly TechCatDbContext _db;

    public CatalogsController(TechCatDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult GetCatalogs()
    {
        var catalogs = _db.Catalog.Select(c => new { c.CatalogId, c.Name, c.Version }).ToList();

        return Ok(catalogs);
    }

    [HttpGet("{version}/components")]
    public IActionResult GetComponents(string version)
    {
        var catalog = _db.Catalog.FirstOrDefault(c => c.Version == version);

        if (catalog == null)
            return NotFound($"'{version}' Version doesn't exist");

        var components = _db.Component.Where(c => c.CatalogId == catalog.CatalogId)
            .Select(c => new { c.ComponentId, c.SheetCode, c.Name }).ToList();

        return Ok(components);
    }

    [HttpGet("{version}/datapoints")]
    public IActionResult GetDataPoints(string version, string? sheetCode, string? parameter, int? year, string? estimate)
    {
        var catalog = _db.Catalog.FirstOrDefault(c => c.Version == version);

        if (catalog == null)
            return NotFound($"'{version}' Version doesn't exist");

        var query = _db.DataPoint.Where(d => d.Component.CatalogId == catalog.CatalogId);

        if (!string.IsNullOrEmpty(sheetCode))
            query = query.Where(d => d.Component.SheetCode == sheetCode);

        if (!string.IsNullOrEmpty(parameter))
            query = query.Where(d => d.Parameter.Name.Contains(parameter));

        if (year != null)
            query = query.Where(d => d.Year == year);

        if (!string.IsNullOrEmpty(estimate))
            query = query.Where(d => d.EstimateType.EstimateCode == estimate);

        var dataPoints = query.Take(1000).Select(d => new
        {
            component = d.Component.SheetCode,
            parameter = d.Parameter.Name,
            unit = d.Parameter.Unit,
            year = d.Year,
            estimate = d.EstimateType.EstimateCode,
            numericValue = d.NumericValue,
            txtValue = d.TxtValue,
            priceYear = d.PriceYear
        }).ToList();

        return Ok(dataPoints);
    }
}
