using BierFroh.Components;
using BierFroh.Model;
using BierFroh.Modules.DependencyGraph;
using BierFroh.Modules.DependencyGraph.Model;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using Develix.Essentials.Core;
using Microsoft.AspNetCore.Components.Forms;

namespace BierFroh.Pages;

partial class ProjectAssets
{
    private readonly Diagram diagram = new();

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        var selectedFile = e.GetMultipleFiles().Single();
        var projectAssets = await GetProjectAssets(selectedFile);
        var graph = new DependencyGraph().GetDirectedGraph(projectAssets);
        UpdateDiagram(graph);
    }

    private void UpdateDiagram(IDirectedGraph<GraphNode> graph)
    {
        var projectNode = graph.Vertices[0];
        var rootNode = new Dependency(new Point(0, 0))
        {
            Title = projectNode.Value.Name,
            Version = projectNode.Value.Version,
        };
        diagram.Nodes.Add(rootNode);
        var x = -170;
        foreach (var dep in graph.Vertices.Skip(1))
        {
            var dependencyNode = new Dependency(new Point(x += 170, 200))
            {
                Title = dep.Value.Name,
                Version = dep.Value.Version
            };
            diagram.Nodes.Add(dependencyNode);
            var link = new LinkModel(rootNode, dependencyNode)
            {
                TargetMarker = LinkMarker.Arrow,
                PathGenerator = PathGenerators.Straight
            };
            diagram.Links.Add(link);
        }
    }

    private static async Task<IProjectAssets> GetProjectAssets(IBrowserFile selectedFile)
    {
        using var stream = selectedFile.OpenReadStream();
        return await ProjectAssetsDeserializer.DeserializeAsync(stream);
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        diagram.RegisterModelComponent<Dependency, DependencyNode>();
    }
}
