using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class DataPoint
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public int ParameterId  { get; set; }
    public int Year { get; set; }
    public string Estimate { get; set; }
    public decimal? NumericValue { get; set; }
    public string? TxtValue { get; set; }
    public int? PriceYear { get; set; }

    public Component Component { get; set; }
    public Parameter Parameter { get; set; }
}
