using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.PathGenerators;

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
        PathGenerator = new SmoothPathGenerator();
    }

    public Dependency SourceNode { get; }
    public Dependency TargetNode { get; }
}
