using BierFroh.Modules.DependencyGraph.Model;
using Develix.Essentials.Core;

namespace BierFroh.Modules.DependencyGraph;

public class DependencyGraph
{
    public IDirectedGraph<GraphNode> GetDirectedGraph(IProjectAssets projectAssets)
    {
        var graphBuilder = GraphBuilder<GraphNode>.Create()
            .AddVertex(new GraphNode(projectAssets.ProjectName))
            .WithSuccessors(projectAssets.Dependencies.Select(d => new GraphNode(d.Name)));

        return graphBuilder.Complete();
    }
}

public record GraphNode(string Name);
