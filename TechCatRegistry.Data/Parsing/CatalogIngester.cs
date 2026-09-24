using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TechCatRegistry.Core;
using TechCatRegistry.Data.Parsing;

namespace TechCatRegistry.Data.Ingest;

public class CatalogIngester
{
    private readonly TechCatDbContext _db;

    public CatalogIngester(TechCatDbContext db)
    {
        _db = db;
    }

    public void Ingest(List<CatalogRow> rows, string version)
    {
        if (_db.Catalog.Any(c => c.Version == version))
            throw new InvalidOperationException($"Version '{version}' already exists somewhere in the db");

        var catalog = new Catalog
        {
            Name = "Technology Data for El and DH",
            Version = version
        };

        _db.Catalog.Add(catalog);

        var estimateTypes = new Dictionary<string, int>();
        foreach (var estimateType in _db.Set<EstimateType>())
        { 
            estimateTypes[estimateType.EstimateCode] = estimateType.EstimateTypeId;
        }

        var groups = new Dictionary<string, ParameterGroup>();
        foreach (var row in rows)
        {
            if (groups.ContainsKey(row.Cat))
                continue;

            var group = new ParameterGroup { Name = row.Cat };

            _db.ParameterGroup.Add(group);
            groups[row.Cat] = group;
        }

        var parameters = new Dictionary<(string, string), Parameter>();
        foreach (var row in rows)
        {
            var key = (row.Cat, row.Par);
            if (parameters.ContainsKey(key))
                continue;

            var parameter = new Parameter
            {
                Name = row.Par,
                Unit = row.Unit,
                Group = groups[row.Cat]
            };

            _db.Parameter.Add(parameter);
            parameters[key] = parameter;
        }
        var components = new Dictionary<string, Component>();
        foreach (var row in rows)
        {
            if (components.ContainsKey(row.Ws))
                continue;

            var component = new Component
            {
                Name = row.Technology,
                SheetCode = row.Ws,
                Catalog = catalog
            };

            _db.Component.Add(component);
            components[row.Ws] = component;
        }

        foreach (var row in rows)
        {
            var dataPoint = new DataPoint
            {
                Component = components[row.Ws],
                Parameter = parameters[(row.Cat, row.Par)],
                EstimateTypeId = estimateTypes[row.Est],
                Year = row.Year,
                PriceYear = row.PriceYear,
                NumericValue = row.NumericValue,
                TxtValue = row.TxtValue
            };

            _db.DataPoint.Add(dataPoint);
        }

        _db.SaveChanges();
    }
}