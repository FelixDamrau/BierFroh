using BierFroh.Components;
using BierFroh.Model.ProjectAssets;
using BierFroh.Modules.DependencyGraph;
using BierFroh.Modules.DependencyGraph.Model;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Geometry;
using Develix.Essentials.Core;
using GraphShape.Algorithms.Layout;
using Microsoft.AspNetCore.Components.Forms;
using QuikGraph;

namespace BierFroh.Pages;

partial class ProjectAssets
{
    private const long maxFileSize = 1024 * 1024 * 5; // 5MB
    private readonly Diagram diagram = new();
    private IProjectAssets? projectAssets;

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        var selectedFile = e.GetMultipleFiles().Single();
        projectAssets = await GetProjectAssets(selectedFile);
    }

    private void SelectedFrameworkChanged(string selected)
    {
        RenderDiagram(selected);
    }

    private void RenderDiagram(string selectedFramework)
    {
        ClearDiagram();
        if (projectAssets is not null)
        {
            var graph = DependencyGraph.Create(projectAssets, selectedFramework);
            UpdateDiagram(graph);
        }
    }

    private static async Task<IProjectAssets> GetProjectAssets(IBrowserFile selectedFile)
    {
        using var stream = selectedFile.OpenReadStream(maxFileSize);
        return await ProjectAssetsDeserializer.DeserializeAsync(stream);
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

    private void SetupLayout()
    {
        var qGraph = new BidirectionalGraph<Dependency, Edge<Dependency>>();
        var nodes = diagram.Nodes.OfType<Dependency>().ToList();
        var edges = diagram.Links.OfType<DependencyLink>().Select(lm => new Edge<Dependency>(lm.SourceNode, lm.TargetNode)).ToList();
        qGraph.AddVertexRange(nodes);
        qGraph.AddEdgeRange(edges);

        var positions = nodes.ToDictionary(nm => nm, dn => new GraphShape.Point(dn.Position.X, dn.Position.Y));
        var sizes = nodes.ToDictionary(nm => nm, dn => new GraphShape.Size(dn.Size?.Width ?? 100, dn.Size?.Height ?? 100));

        var algorithm = CreateLayoutAlgorithm(qGraph, positions, sizes);
        algorithm.Compute();
        UpdatePositions(algorithm);
    }

    private static ILayoutAlgorithm<Dependency, Edge<Dependency>, BidirectionalGraph<Dependency, Edge<Dependency>>> CreateLayoutAlgorithm(
        BidirectionalGraph<Dependency, Edge<Dependency>> qGraph,
        IDictionary<Dependency, GraphShape.Point> positions,
        IDictionary<Dependency, GraphShape.Size> sizes)
    {
        var layoutContext = new LayoutContext<Dependency, Edge<Dependency>, BidirectionalGraph<Dependency, Edge<Dependency>>>(qGraph, positions, sizes, LayoutMode.Simple);
        var algorithmFactory = new StandardLayoutAlgorithmFactory<Dependency, Edge<Dependency>, BidirectionalGraph<Dependency, Edge<Dependency>>>();
        var layoutParameters = new SimpleTreeLayoutParameters()
        {
            Direction = LayoutDirection.LeftToRight,
            SpanningTreeGeneration = SpanningTreeGeneration.DFS,
            LayerGap = 100,
            VertexGap = 25,
        };
        var algorithm = algorithmFactory.CreateAlgorithm("Tree", layoutContext, layoutParameters);
        return algorithm;
    }

    private void UpdatePositions(ILayoutAlgorithm<Dependency, Edge<Dependency>, BidirectionalGraph<Dependency, Edge<Dependency>>> algorithm)
    {
        try
        {
            diagram.SuspendRefresh = true;
            foreach (var vertPos in algorithm.VerticesPositions)
            {
                vertPos.Key.SetPosition(vertPos.Value.X, vertPos.Value.Y);
            }
        }
        finally
        {
            diagram.SuspendRefresh = false;
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        diagram.RegisterModelComponent<Dependency, DependencyNode>();
    }
}
