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

    [Fact]
    public void TestFirstLineInAllData()
    {
        var TestDataPath = Path.Combine(AppContext.BaseDirectory, "TestData", "technology_data_for_el_and_dh_updated_alldatalong.xlsx");

        var workbook = new XLWorkbook(TestDataPath);
        var alldata = workbook.Worksheet("alldata_long");

        var CatalougeKey = alldata.Cell("A3").GetString();
        var Ws = alldata.Cell("B3").GetString();
        var Tech = alldata.Cell("C3").GetString();
        var Cat = alldata.Cell("D3").GetString();
        var Par = alldata.Cell("E3").GetString();
        var Unit = alldata.Cell("F3").GetString();
        var PriceYear = alldata.Cell("G3").GetString();
        var Note = alldata.Cell("H3").GetString();
        var Ref = alldata.Cell("I3").GetString();
        var Est = alldata.Cell("J3").GetString();
        var Year = alldata.Cell("K3").GetString();
        var Val = alldata.Cell("L3").GetString();
        var TechKey = alldata.Cell("M3").GetString();
        var CatKey = alldata.Cell("N3").GetString();
        var ParKey = alldata.Cell("O3").GetString();
        var TechId = alldata.Cell("P3").GetString();
        var CatId = alldata.Cell("Q3").GetString();
        var CatSourceId = alldata.Cell("R3").GetString();
        var ParId = alldata.Cell("S3").GetString();

        Assert.Equal("ELH", CatalougeKey);
        Assert.Equal("01 Coal CHP", Ws);
        Assert.Equal("Coal power plant, supercritical - extraction - coal - medium", Tech);
        Assert.Equal("Energy/technical data", Cat);
        Assert.Equal("Cb coefficient (50°C/100°C)", Par);
        Assert.Equal("", Unit);
        Assert.Equal("", PriceYear);
        Assert.Equal("A", Note);
        Assert.Equal("", Ref);
        Assert.Equal("ctrl", Est);
        Assert.Equal("2015", Year);
        Assert.Equal("0.75", Val);
        Assert.Equal("coal power plant, supercritical - extraction - coal - medium", TechKey);
        Assert.Equal("energy/technical data", CatKey);
        Assert.Equal("cb coefficient (50°c/100°c)", ParKey);
        Assert.Equal("ELH_0020", TechId);
        Assert.Equal("tk_etd", CatId);
        Assert.Equal("manual_category_map", CatSourceId);
        Assert.Equal("PAR_0015", ParId);
    }
}
