using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class Parameter
{
    public int ParameterId { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string? Unit { get; set; }
    public int SortOrder { get; set; }
    public ParameterGroup Group { get; set; }
    public List<DataPoint> Points { get; set; }
}
