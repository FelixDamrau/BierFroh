using BierFroh.Model.ProjectAssets;
using BierFroh.Modules.DependencyGraph;
using BierFroh.Modules.DependencyGraph.Model;
using Blazor.Diagrams;
using Blazor.Diagrams.Core.Geometry;
using Develix.Essentials.Core;
using GraphShape.Algorithms.Layout;
using Microsoft.AspNetCore.Components.Forms;
using QuikGraph;

namespace BierFroh.Pages;

partial class ProjectAssets
{
    private const long maxFileSize = 1024 * 1024 * 5; // 5MB
    private BlazorDiagram diagram = null!;
    private IProjectAssets? projectAssets;
    private string? selectedFramework;
    private IEnumerable<string>? selectedDependencyKeys;
    private DependencyFilterFactory? filterFactory;

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        var selectedFile = e.GetMultipleFiles().Single();
        projectAssets = await GetProjectAssets(selectedFile);
        selectedFramework = projectAssets.Frameworks.FirstOrDefault();
        filterFactory = new DependencyFilterFactory(projectAssets);
        selectedDependencyKeys = filterFactory.GetKeys();
        ClearDiagram();
    }

    private static async Task<IProjectAssets> GetProjectAssets(IBrowserFile selectedFile)
    {
        using var stream = selectedFile.OpenReadStream(maxFileSize);
        return await ProjectAssetsDeserializer.DeserializeAsync(stream);
    }

    private void RenderDiagram()
    {
        ClearDiagram();
        if (projectAssets is not null && filterFactory is not null && selectedDependencyKeys is not null && selectedFramework is not null)
        {
            var dependecyFilter = filterFactory.CreateFilter(selectedDependencyKeys);
            var graph = DependencyGraph.Create(projectAssets, dependecyFilter, selectedFramework);
            UpdateDiagram(graph);
        }
    }

    private void ClearDiagram()
    {
        diagram.Nodes.Clear();
        diagram.Links.Clear();
    }

    private void UpdateDiagram(IDirectedGraph<GraphNode> graph)
    {
        try
        {
            diagram.SuspendRefresh = true;
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
        finally
        {
            diagram.SuspendRefresh = false;
            diagram.Refresh();
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

    private static void UpdatePositions(ILayoutAlgorithm<Dependency, Edge<Dependency>, BidirectionalGraph<Dependency, Edge<Dependency>>> algorithm)
    {
        foreach (var vertPos in algorithm.VerticesPositions)
        {
            vertPos.Key.SetPosition(vertPos.Value.X, vertPos.Value.Y);
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        diagram = new BlazorDiagram();
    }
}
