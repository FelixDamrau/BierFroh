using BierFroh.Modules.DependencyGraph.Model;
using Develix.Essentials.Core;

namespace BierFroh.Modules.DependencyGraph;

public class DependencyGraph
{
    public static IDirectedGraph<GraphNode> Create(IProjectAssets projectAssets, string framework)
    {
        var graphBuilder = GraphBuilder<GraphNode>.Create();
        foreach (var dependency in projectAssets.Dependencies.Where(d => d.Framework == framework))
        {
            graphBuilder = graphBuilder.AddVertex(new GraphNode(dependency.Name, dependency.Framework, dependency.Version))
                .WithSuccessors(dependency.Dependencies.Select(d => new GraphNode(d.Name, d.Framework, d.Version)));
        }
        return graphBuilder.Complete();
    }
}

public record GraphNode(
    string Name,
    string Framework,
    string Version);
