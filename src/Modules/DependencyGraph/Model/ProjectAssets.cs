namespace BierFroh.Modules.DependencyGraph.Model;

internal class ProjectAssets : IProjectAssets
{
    public string ProjectName { get; set; } = "Unknown";
    public string Version { get; set; } = "Unknown";
    public HashSet<string> Frameworks { get; set; } = new();
    public List<Dependency> Dependencies { get; set; } = new List<Dependency>();

    IReadOnlyList<IDependency> IProjectAssets.Dependencies => Dependencies;
    IReadOnlySet<string> IProjectAssets.Frameworks => Frameworks;
}
