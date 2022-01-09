using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace BierFroh.Model.ProjectAssets;
public class Dependency : NodeModel
{
    public string? Version { get; set; }
    public string? Framework { get; set; }

    public Dependency(Point point)
        : base(point)
    {
    }
}
