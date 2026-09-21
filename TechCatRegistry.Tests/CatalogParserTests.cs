using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using TechCatRegistry.Data;
using TechCatRegistry.Data.Ingest;
using TechCatRegistry.Data.Parsing;

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
    public void ParserReadsAllRowsCount()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", "technology_data_for_el_and_dh_updated_alldatalong.xlsx");

        var parser = new CatalogParser();
        var rows = parser.ParseFromExcelFilePath(path);

        Assert.Equal(17508, rows.Count);
    }

    [Fact]
    public void NewFirstRowTest()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "TestData", "technology_data_for_el_and_dh_updated_alldatalong.xlsx");

        var rows = new CatalogParser().ParseFromExcelFilePath(path);
        var first = rows[0];

        Assert.Equal("ELH", first.CatalogueKey);
        Assert.Equal("01 Coal CHP", first.Ws);
        Assert.Equal("Energy/technical data", first.Cat);
        Assert.Equal("ctrl", first.Est);
        Assert.Equal(2015, first.Year);
        Assert.Null(first.Unit);
        Assert.Null(first.PriceYear);
        Assert.Equal("A", first.Note);
    }

    [Fact]
    public void IngestMakesCorrectAmount()
    {
        var options = new DbContextOptionsBuilder<TechCatDbContext>()
            .UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=TechCatRegistry_Test;Trusted_Connection=True").Options;
        // learn.microsoft.com/ef/core/testing/testing-without-the-database

        using var db = new TechCatDbContext(options);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        var path = Path.Combine(AppContext.BaseDirectory, "TestData", "technology_data_for_el_and_dh_updated_alldatalong.xlsx");
        var rows = new CatalogParser().ParseFromExcelFilePath(path);

        new CatalogIngester(db).Ingest(rows, "0019");

        Assert.Equal(1, db.Catalog.Count());
        Assert.Equal(6, db.ParameterGroup.Count());
        Assert.Equal(77, db.Component.Count());
        Assert.Equal(17508, db.DataPoint.Count());
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
