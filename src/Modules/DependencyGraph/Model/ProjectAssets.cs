namespace BierFroh.Modules.DependencyGraph.Model;

internal class ProjectAssets : IProjectAssets
{

    internal Dependency.DependencyCollection DependencyCollection { get; } = new();
    public string ProjectName { get; set; } = "Unknown";
    public string Version { get; set; } = "Unknown";
    public HashSet<string> Frameworks { get; set; } = new();
    public IReadOnlyCollection<IDependency> Dependencies => DependencyCollection;
    IReadOnlyCollection<string> IProjectAssets.Frameworks => Frameworks;
}
