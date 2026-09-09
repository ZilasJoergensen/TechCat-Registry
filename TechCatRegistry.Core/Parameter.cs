using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class Parameter : ParamGroup
{
    public int Id { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public int SortOrder { get; set; }
    public ParamGroup Group { get; set; }
    public List<DataPoint> Points { get; set; }
}
