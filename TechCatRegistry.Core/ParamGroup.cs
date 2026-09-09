using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class ParamGroup
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SortOrder { get; set; }
    public List<Parameter> Parameters { get; set; }
}
