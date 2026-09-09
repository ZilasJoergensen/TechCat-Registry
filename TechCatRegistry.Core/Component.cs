using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Core;

public class Component
{
    public int Id  { get; set; }
    public int CatalogId { get; set; }
    public string SheetCode { get; set; }
    public string Name { get; set; }
    public Catalog Catalog { get; set; }
    public List<DataPoint> DataPoints { get; set; }
}
