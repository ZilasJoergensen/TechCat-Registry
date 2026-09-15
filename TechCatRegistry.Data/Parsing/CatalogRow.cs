using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Data.Parsing;

public class CatalogRow
{
    public string Ws { get; set; }
    public string Technology { get; set; }
    public string Cat { get; set; }
    public string Par { get; set; }

    public string? Unit { get; set; }
    public string? Note { get; set; }
    public string? Ref { get; set; }

    public string Est { get; set; }

    public int Year { get; set; }

    public int? PriceYear { get; set; }

    public string Val { get; set; }
}
