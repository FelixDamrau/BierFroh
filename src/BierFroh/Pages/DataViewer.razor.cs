using System.Xml;
using BierFroh.Model;
using BierFroh.Modules.DataViewer;
using DevLab.JmesPath;
using Microsoft.AspNetCore.Components.Forms;
using Newtonsoft.Json;

namespace BierFroh.Pages;
public partial class DataViewer
{
    private const long maxFileSize = 209_715_200; // 200MB
    private string? xmlText;
    private string? jsonText;
    private string? query;
    private string queryResult = string.Empty;
    private string cleanResults = string.Empty;

    private void Parse()
    {
        if (string.IsNullOrEmpty(xmlText))
        {
            jsonText = string.Empty;
            return;
        }
        var xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlText);
        RemoveIrrelevantXmlNodes(xmlDocument);

        jsonText = JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented, omitRootObject: true);
    }

    private static void RemoveIrrelevantXmlNodes(XmlDocument xmlDocument)
    {
        var nodes = xmlDocument.ChildNodes.OfType<XmlNode>().ToList();
        foreach (var irrelevantNode in nodes.Where(n => n is XmlDocumentType or XmlDeclaration))
            xmlDocument.RemoveChild(irrelevantNode);
    }

    private void Filter()
    {
        var jmesPath = new JmesPath();
        jmesPath.FunctionRepository.Register("unique", new UniqueFunction());
        try
        {
            queryResult = jmesPath.Transform(jsonText, query);
        }
        catch (Exception ex)
        {
            queryResult = $"""
                Could not parse query. See exception message for details:
                {ex.Message}
                """;
        }
    }

    private void Extract()
    {
        try
        {
            var data = JsonConvert.DeserializeObject<List<string>>(queryResult) ?? [];
            cleanResults = string.Join(Environment.NewLine, data);
        }
        catch (Exception ex)
        {
            cleanResults = $"""
                Could not extract data. See exception message for details:
                {ex.Message}
                """;
        }
    }

    private async Task UploadXml(InputFileChangeEventArgs e)
    {
        xmlText = await BrowserFileReader.ReadFile(e.File, maxFileSize);
    }

    private async Task UploadJson(InputFileChangeEventArgs e)
    {
        xmlText = string.Empty;
        jsonText = await BrowserFileReader.ReadFile(e.File, maxFileSize);
    }
}
