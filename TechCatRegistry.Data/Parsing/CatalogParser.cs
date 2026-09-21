using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Text;

namespace TechCatRegistry.Data.Parsing;

public class CatalogParser
{
    private const int _headerRow = 2;
    private const int _firstDataRow = 3;

    public List<CatalogRow> ParseFromExcelFilePath(string filepath)
    {
        using var stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
        return ParseFromStream(stream).Rows;
    }

    public ParsedCatalog ParseFromStream(Stream stream)
    {
        using var workbook = new XLWorkbook(stream);

        var intro = workbook.Worksheet("Intro");
        var version = intro.Cell("D3").GetString();

        var alldata = workbook.Worksheet("alldata_long");
        int lastRow = alldata.LastRowUsed().RowNumber();

        Dictionary<string, int> coloumns = ReadHeaders(alldata);

        int ws = coloumns["ws"];
        int tech = coloumns["technology"];
        int cat = coloumns["cat"];
        int par = coloumns["par"];
        int unit = coloumns["unit"];
        int note = coloumns["note"];
        int reference = coloumns["ref"];
        int est = coloumns["est"];
        int year = coloumns["year"];
        int priceYear = coloumns["priceyear"];
        int val = coloumns["val"];
        int catalogueKey = coloumns["catalogue_key"];
        int technologyKey = coloumns["technology_key"];
        int catKey = coloumns["cat_key"];
        int parKey = coloumns["par_key"];
        int techId = coloumns["technology_id"];
        int catId = coloumns["cat_id"];
        int catIdSource = coloumns["cat_id_source"];
        int parId = coloumns["par_id"];

        var rows = new List<CatalogRow>();

        for (int row = _firstDataRow; row <= lastRow; row++)
        {
            if (alldata.Cell(row, tech).IsEmpty())
                continue;

            var valCell = alldata.Cell(row, val);

            rows.Add(new CatalogRow
            {
                Ws = alldata.Cell(row, ws).GetString(),
                Technology = alldata.Cell(row, tech).GetString(),
                Cat = alldata.Cell(row, cat).GetString(),
                Par = alldata.Cell(row, par).GetString(),
                Unit = alldata.Cell(row, unit).IsEmpty() ? null : alldata.Cell(row, unit).GetString(),
                Note = alldata.Cell(row, note).IsEmpty() ? null : alldata.Cell(row, note).GetString(),
                Ref = alldata.Cell(row, reference).IsEmpty() ? null : alldata.Cell(row, reference).GetString(),
                Est = alldata.Cell(row, est).GetString(),
                Year = alldata.Cell(row, year).GetValue<int>(),
                PriceYear = alldata.Cell(row, priceYear).IsEmpty() ? null : alldata.Cell(row, priceYear).GetValue<int>(),
                NumericValue = valCell.DataType == XLDataType.Number ? valCell.GetValue<decimal>() : null,
                TxtValue = valCell.DataType == XLDataType.Number ? null : valCell.GetString(),
                CatalogueKey = alldata.Cell(row, catalogueKey).GetString(),
                TechnologyKey = alldata.Cell(row, technologyKey).GetString(),
                CatKey = alldata.Cell(row, catKey).GetString(),
                ParKey = alldata.Cell(row, parKey).GetString(),
                TechId = alldata.Cell(row, techId).GetString(),
                CatId = alldata.Cell(row, catId).GetString(),
                CatIdSource = alldata.Cell(row, catIdSource).GetString(),
                ParId = alldata.Cell(row, parId).GetString()
            });
        }

        return new ParsedCatalog { Version = version, Rows = rows };
    }

    private static Dictionary<string, int> ReadHeaders(IXLWorksheet sheet)
    {
        var map = new Dictionary<string, int>();
        var lastColumn = sheet.LastColumnUsed().ColumnNumber();

        for (int col = 1; col <= lastColumn; col++)
        {
            var name = sheet.Cell(_headerRow, col).GetString();
            if (name.Length > 0)
                map[name] = col;
        }

        return map;
    }
}
