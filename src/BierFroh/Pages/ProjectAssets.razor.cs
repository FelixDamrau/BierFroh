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
        var graph = DependencyGraph.Create(projectAssets);
        ClearDiagram();
        UpdateDiagram(graph);
    }

    private void ClearDiagram()
    {
        diagram.Nodes.Clear();
        diagram.Links.Clear();
    }

    private void UpdateDiagram(IDirectedGraph<GraphNode> graph)
    {
        var x = 0;
        var cache = new Dictionary<GraphNode, Dependency>();
        foreach (var dep in graph.Vertices)
        {
            var dependencyNode = AddDiagramNode(dep.Value, cache, ref x);
            foreach (var successor in dep.Successors)
            {
                var successorNode = AddDiagramNode(successor.Value, cache, ref x);
                var link = new LinkModel(dependencyNode, successorNode)
                {
                    TargetMarker = LinkMarker.Arrow,
                    PathGenerator = PathGenerators.Straight
                };
                diagram.Links.Add(link);
            }
        }
    }

    private Dependency AddDiagramNode(GraphNode graphNode, Dictionary<GraphNode, Dependency> cache, ref int counter)
    {
        if (cache.TryGetValue(graphNode, out var dependecy))
            return dependecy;

        var addedDependency = new Dependency(new Point(counter * 170, 0))
        {
            Title = graphNode.Name,
            Framework = graphNode.Framework,
            Version = graphNode.Version
        };
        diagram.Nodes.Add(addedDependency);
        counter++;
        return addedDependency;
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
