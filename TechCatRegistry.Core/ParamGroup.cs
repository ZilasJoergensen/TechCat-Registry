using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class ParameterGroup
{
    public int ParameterGroupId { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public List<Parameter> Parameters { get; set; }
}
