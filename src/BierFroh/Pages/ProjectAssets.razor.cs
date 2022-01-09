using BierFroh.Components;
using BierFroh.Model.ProjectAssets;
using BierFroh.Modules.DependencyGraph;
using BierFroh.Modules.DependencyGraph.Model;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Develix.Essentials.Core;
using GraphShape.Algorithms.Layout;
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
        diagram.SuspendRefresh = true;
        ClearDiagram();
        UpdateDiagram(graph);
        diagram.SuspendRefresh = false;
    }

    private void ClearDiagram()
    {
        diagram.Nodes.Clear();
        diagram.Links.Clear();
    }

    private void UpdateDiagram(IDirectedGraph<GraphNode> graph)
    {
        var cache = new Dictionary<GraphNode, Dependency>();
        foreach (var dep in graph.Vertices)
        {
            var dependencyNode = AddDiagramNode(dep.Value, cache);
            foreach (var successor in dep.Successors)
            {
                var successorNode = AddDiagramNode(successor.Value, cache);
                var link = new DependencyLink(dependencyNode, successorNode);
                diagram.Links.Add(link);
            }
        }
        SetupLayout();
    }

    private void SetupLayout()
    {
        var qGraph = new QuikGraph.BidirectionalGraph<Dependency, QuikGraph.Edge<Dependency>>();
        var nodes = diagram.Nodes.OfType<Dependency>().ToList();
        var edges = diagram.Links.OfType<DependencyLink>().Select(lm => new QuikGraph.Edge<Dependency>(lm.SourceNode, lm.TargetNode)).ToList();
        qGraph.AddVertexRange(nodes);
        qGraph.AddEdgeRange(edges);

        var positions = nodes.ToDictionary(nm => nm, dn => new GraphShape.Point(dn.Position.X, dn.Position.Y));
        var sizes = nodes.ToDictionary(nm => nm, dn => new GraphShape.Size(dn.Size?.Width ?? 100, dn.Size?.Height ?? 100));
        var layoutCtx = new LayoutContext<Dependency, QuikGraph.Edge<Dependency>, QuikGraph.BidirectionalGraph<Dependency, QuikGraph.Edge<Dependency>>>(qGraph, positions, sizes, LayoutMode.Simple);
        var algoFact = new StandardLayoutAlgorithmFactory<Dependency, QuikGraph.Edge<Dependency>, QuikGraph.BidirectionalGraph<Dependency, QuikGraph.Edge<Dependency>>>();
        var algo = algoFact.CreateAlgorithm("Tree", layoutCtx, new SimpleTreeLayoutParameters()
        {
            Direction = LayoutDirection.LeftToRight,
            SpanningTreeGeneration = SpanningTreeGeneration.DFS,
            LayerGap = 100,
            VertexGap = 25,
        });

        algo.Compute();

        try
        {
            diagram.SuspendRefresh = true;
            foreach (var vertPos in algo.VerticesPositions)
            {
                vertPos.Key.SetPosition(vertPos.Value.X, vertPos.Value.Y);
            }
        }
        finally
        {
            diagram.SuspendRefresh = false;
        }
    }

    private Dependency AddDiagramNode(GraphNode graphNode, Dictionary<GraphNode, Dependency> cache)
    {
        if (cache.TryGetValue(graphNode, out var dependecy))
            return dependecy;

        var addedDependency = new Dependency(new Point(0, 0))
        {
            Title = graphNode.Name,
            Framework = graphNode.Framework,
            Version = graphNode.Version
        };
        diagram.Nodes.Add(addedDependency);
        cache.Add(graphNode, addedDependency);
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
