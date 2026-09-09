using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class Catalog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? PublishedOn { get; set; }
    public List<Component> Components { get; set; }
}
