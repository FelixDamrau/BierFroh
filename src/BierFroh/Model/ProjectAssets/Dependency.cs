using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace BierFroh.Model.ProjectAssets;
public class Dependency(Point point) : NodeModel(point)
{
    public string? Version { get; set; }
    public string? Framework { get; set; }
}
