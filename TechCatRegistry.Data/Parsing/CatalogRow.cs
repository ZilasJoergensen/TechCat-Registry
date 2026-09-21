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

    public decimal? NumericValue { get; set; }
    public string? TxtValue { get; set; }

    public string CatalogueKey { get; set; }
    public string TechnologyKey { get; set; }
    public string CatKey { get; set; }
    public string ParKey { get; set; }

    public string TechId {  get; set; }
    public string CatId { get; set; }
    public string CatIdSource {  get; set; }
    public string ParId { get; set; }
}
