using BierFroh.Modules.DependencyGraph.Model;
using Develix.Essentials.Core;

namespace BierFroh.Modules.DependencyGraph;

public class DependencyGraph
{
    public static IDirectedGraph<GraphNode> Create(IProjectAssets projectAssets)
    {
        var graphBuilder = GraphBuilder<GraphNode>.Create()
            .AddVertex(new GraphNode(projectAssets.ProjectName, projectAssets.Frameworks, projectAssets.Version))
            .WithSuccessors(projectAssets.Dependencies.Select(d => new GraphNode(d.Name, Array.Empty<string>(), d.Version)));

        return graphBuilder.Complete();
    }
}

public record GraphNode(
    string Name,
    IReadOnlyCollection<string> Frameworks,
    string Version);
