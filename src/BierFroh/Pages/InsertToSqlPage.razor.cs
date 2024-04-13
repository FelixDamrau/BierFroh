using BierFroh.Model;
using BierFroh.Modules.InsertToSql;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BierFroh.Pages;
public partial class InsertToSqlPage
{
    private string rawData = string.Empty;
    private string sqlQuery = string.Empty;
    private string tableName = "table";
    private IReadOnlyList<IList<string>> tableData = [];
    private bool[] activeColumns = [];

    [Inject]
    private IJSRuntime jsRuntime { get; set; } = null!;

    private void ActiveColumsChanged(bool[] activeColumns)
    {
        this.activeColumns = activeColumns;
    }

    private async Task ParseClickAsync(MouseEventArgs e)
    {
        var reader = new StringReader(rawData);
        var parser = new RawDataParser(reader);
        var list = new List<IList<string>>();
        while (await parser.ReadAsync())
        {
            var line = parser.GetLineData();
            list.Add(line);
        }
        tableData = list;

        UpdateSqlStatement();
    }

    private void RefreshQueryClick(MouseEventArgs e)
    {
        UpdateSqlStatement();
    }

    private void GenerateTestDataClick(MouseEventArgs e)
    {
        rawData = $"""
            Text{'\t'}Number{'\t'}DateTime
            foo{'\t'}5{'\t'}2021-03-04 01:03:04.000
            bar{'\t'}12{'\t'}2021-05-04 13:43:32.302
            """;
    }

    private void UpdateSqlStatement()
    {
        if (tableData.Count < 2)
        {
            sqlQuery = string.Empty;
            return;
        }

        var valueRows = tableData.Skip(1).Select(t => $"  {CreateValueRow(t)}");
        var projection = CreateSelectedRows(tableData[0]);
        sqlQuery = $"""
            INSERT INTO [{tableName}] ( {projection} )
            SELECT * FROM ( VALUES
            {string.Join("," + Environment.NewLine, valueRows)}
            ) AS temp ( {projection} )
            """;
    }


    private string CreateValueRow(IEnumerable<string> row)
    {
        var parsedCells = row
            .Where((_, index) => IsActive(index))
            .Select(c => SqlValueFormatter.Parse(c));
        return $"( {string.Join(", ", parsedCells)} )";
    }

    private string CreateSelectedRows(IEnumerable<string> row)
    {
        var rowNames = row
            .Where((_, index) => IsActive(index))
            .Select(c => $"[{c}]");
        return string.Join(", ", rowNames);
    }

    private bool IsActive(int index)
    {
        if (activeColumns.Length <= index)
            return true;
        return activeColumns[index];
    }

    private async Task UploadFile(InputFileChangeEventArgs e)
    {
        rawData = await BrowserFileReader.ReadFile(e.File, 4_096_000);
        StateHasChanged();
    }

    private async Task DownloadQuery()
    {
        await jsRuntime.InvokeAsync<object>(
            "downloadFileFromStream", 
            tableName + "Query.sql", 
            Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sqlQuery)));
    }
}
