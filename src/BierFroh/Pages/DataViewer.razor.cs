using System.Xml;
using BierFroh.Model;
using BierFroh.Modules.DataViewer;
using DevLab.JmesPath;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BierFroh.Pages;
public partial class DataViewer
{
    private const long maxFileSize = 209_715_200; // 200MB
    private string? xmlText;
    private string? xmlErrorText;

    private string jsonText = string.Empty;
    private JToken? jToken;
    private string? jsonPreParseError;

    private string? query;
    private string queryResult = string.Empty;
    private string cleanResults = string.Empty;

    private bool parsing = false;
    private bool preParsing = false;

    private void ParseXml()
    {
        xmlErrorText = null;
        if (string.IsNullOrEmpty(xmlText))
        {
            jsonText = string.Empty;
            return;
        }
        try
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlText);
            RemoveIrrelevantXmlNodes(xmlDocument);
            jsonText = JsonConvert.SerializeXmlNode(xmlDocument, Newtonsoft.Json.Formatting.Indented, omitRootObject: true);
        }
        catch (Exception ex)
        {
            jsonText = string.Empty;
            xmlErrorText = $"""
                Could not convert XML document to json. See exception message for details:
                {ex.Message}
                """;
        }
    }

    private static void RemoveIrrelevantXmlNodes(XmlDocument xmlDocument)
    {
        var nodes = xmlDocument.ChildNodes.OfType<XmlNode>().ToList();
        foreach (var irrelevantNode in nodes.Where(n => n is XmlDocumentType or XmlDeclaration))
            xmlDocument.RemoveChild(irrelevantNode);
    }

    private async Task PreParse()
    {
        try
        {
            jsonPreParseError = null;
            preParsing = true;
            jToken = await Task.Run(() => JToken.Parse(jsonText));
        }
        catch (Exception ex)
        {
            jsonPreParseError = $"""
                Could not parse json text. See exception message for details:
                {ex.Message}
                """;
            jToken = null;
        }
        finally
        {
            preParsing = false;
        }
    }

    private void CleanupPreParse()
    {
        jToken = null;
    }

    private async Task FilterAsync()
    {
        try
        {
            parsing = true;
            var jmesPath = new JmesPath();
            jmesPath.FunctionRepository.Register("unique", new UniqueFunction());
            queryResult = jToken is null
                ? await FilterFromText(jmesPath).ConfigureAwait(false)
                : await FilterFromToken(jmesPath, jToken).ConfigureAwait(false);
        }
        finally
        {
            parsing = false;
        }
    }

    private Task<string> FilterFromText(JmesPath jmesPath)
    {
        try
        {
            return Task.Run(() => jmesPath.Transform(jsonText, query));
        }
        catch (Exception ex)
        {
            return Task.FromResult($"""
                Could not transform and filter query. See exception message for details:
                {ex.Message}
                """);
        }
    }

    private async Task<string> FilterFromToken(JmesPath jmesPath, JToken jToken)
    {
        try
        {
            var transformResult = await Task.Run(async () => await jmesPath.TransformAsync(jToken, query));
            return transformResult.ToString()!;
        }
        catch (Exception ex)
        {
            return $"""
                Could not filter query. See exception message for details:
                {ex.Message}
                """;
        }
    }

    private async Task KeyInQueryPressedAsync(KeyboardEventArgs keyboardEventArgs)
    {
        if (keyboardEventArgs.Key == "Enter")
            await FilterAsync();
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
