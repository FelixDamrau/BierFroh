using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;

namespace BierFroh.Model.ProjectAssets;
public class DependencyLink : LinkModel
{
    public DependencyLink(Dependency sourceNode, Dependency targetNode)
        : base(sourceNode, targetNode)
    {
        SourceNode = sourceNode;
        TargetNode = targetNode;
        TargetMarker = LinkMarker.Arrow;
        PathGenerator = PathGenerators.Straight;
    }

    public new Dependency SourceNode { get; }
    public new Dependency TargetNode { get; }
}
