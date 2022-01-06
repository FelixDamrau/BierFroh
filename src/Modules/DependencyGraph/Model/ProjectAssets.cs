namespace BierFroh.Modules.DependencyGraph.Model;

internal class ProjectAssets : IProjectAssets
{
    public string ProjectName { get; set; } = "Unknown";
    public List<Dependency> Dependencies { get; set; } = new List<Dependency>();

    IReadOnlyList<IDependency> IProjectAssets.Dependencies => Dependencies;
}
