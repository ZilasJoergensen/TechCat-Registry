using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class DataPoint
{
    public int DataPointId { get; set; }
    public int ComponentId { get; set; }
    public int ParameterId  { get; set; }
    public int EstimateTypeId { get; set; }
    public int Year { get; set; }
    public decimal? NumericValue { get; set; }
    public string? TxtValue { get; set; }
    public int? PriceYear { get; set; } // valutabasis. fks "2020 eur"

    public Component Component { get; set; }
    public Parameter Parameter { get; set; }
    public EstimateType EstimateType { get; set; }
}
