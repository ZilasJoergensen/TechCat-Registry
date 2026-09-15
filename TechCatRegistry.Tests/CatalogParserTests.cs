using ClosedXML.Excel;

namespace TechCatRegistry.Tests;

public class CatalogParserTests
{
    [Fact]
    public void IntroArkNameCheck()
    {
        // https://docs.closedxml.io/
        // https://github.com/ClosedXML/ClosedXML/tree/develop/ClosedXML.Examples
        var TestDataPath = Path.Combine(AppContext.BaseDirectory, "TestData", "technology_data_for_el_and_dh_updated_alldatalong.xlsx");

        var workbook = new XLWorkbook(TestDataPath);
        var intro = workbook.Worksheet("Intro");

        var version = intro.Cell("D3").GetString();

        Assert.Contains("0019", version);
    }
}
