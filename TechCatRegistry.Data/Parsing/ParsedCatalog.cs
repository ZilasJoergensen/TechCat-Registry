using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Data.Parsing;

public class ParsedCatalog
{
    public string Version { get; set; } = string.Empty;
    public List<CatalogRow> Rows { get; set; } = new();
}
