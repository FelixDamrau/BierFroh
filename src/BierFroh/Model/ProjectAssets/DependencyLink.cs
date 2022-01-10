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
        SourceMarker = LinkMarker.Circle;
        TargetMarker = LinkMarker.Arrow;
        PathGenerator = PathGenerators.Smooth;
    }

    public new Dependency SourceNode { get; }
    public new Dependency TargetNode { get; }
}
